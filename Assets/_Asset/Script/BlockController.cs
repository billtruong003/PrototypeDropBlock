using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using UnityEngine.UIElements;
using Unity.Mathematics;
using System.Linq;

public class BlockController : MonoBehaviour
{
    // Movement settings
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float floatingSpeed = 2f;
    [SerializeField] private bool canRotate;

    // Transforms
    [Header("Transforms")]
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform centerPoint;

    // Raycast and Cube data
    [Header("Raycast and Cube Data")]
    [SerializeField] private List<RayCastDetect> rayCastDetect = new List<RayCastDetect>();
    [SerializeField] private List<GameObject> totalCube = new();

    // Block data
    [CustomHeader("Data", 15, "#B0EBB4")]
    [BoxGroup("CubeData")]
    [SerializeField] private BlockShape blockShape;
    [BoxGroup("CubeData")]
    [SerializeField] private BlockAngle blockAngle;

    // Private variables
    private float targetHeight;
    private Vector3 targetPosition;
    private GameObject detectedObject;
    private GameObject hitObject;

    public bool DoneDrop { get; private set; } = false;

    public Transform GetPivot() => pivot;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!DoneDrop)
        {
            HandleMovement();
            HandleRotate();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropToCenter();
            }
        }

    }

    private void Init()
    {
        rayCastDetect = GetAllComponents(gameObject);
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, moveZ), Space.World);
    }
    private void HandleRotate()
    {
        if (!canRotate)
            return;

        if (Input.GetKeyDown(KeyCode.E)) // Xoay quanh trục Y (right)
        {
            transform.Rotate(Vector3.up, 90, Space.World); // Xoay 90 độ quanh trục Y
        }
        if (Input.GetKeyDown(KeyCode.Q)) // Xoay quanh trục Y (left)
        {
            transform.Rotate(Vector3.up, -90, Space.World); // Xoay -90 độ quanh trục Y
        }
        if (Input.GetKeyDown(KeyCode.R)) // Xoay quanh trục X (up)
        {
            transform.Rotate(Vector3.right, 90, Space.World); // Xoay 90 độ quanh trục X
        }
        if (Input.GetKeyDown(KeyCode.F)) // Xoay quanh trục X (down)
        {
            transform.Rotate(Vector3.right, -90, Space.World); // Xoay -90 độ quanh trục X
        }
    }

    private void DropToCenter()
    {
        DoneDrop = true;
        targetPosition = GetHighestPosition();
        hitObject = GetHitObject();
        detectedObject = GetObjectDetect();

        if (detectedObject == null)
        {
            Debug.LogError("Detected object is null");
            return;
        }

        pivot.position = detectedObject.transform.position;
        SetParentAllCube();

        Vector3 targetPose = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        pivot.position = targetPose;

        Sequence mySequence = DOTween.Sequence();

        targetHeight = hitObject != null && hitObject.CompareTag("Block") ? targetPosition.y + 1 : targetPosition.y + 0.5f;
        mySequence.Append(pivot.DOMoveY(targetHeight, floatingSpeed).SetEase(Ease.InQuad));

        Debug.Log("Sequence setup completed");
        mySequence.OnComplete(OnDropComplete);
        mySequence.Play();
        Debug.Log("Sequence started");
    }


    private void OnDropComplete()
    {
        Debug.Log("OnDropComplete called");

        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.SpawnCube();
            SaveData(new Vector3(targetPosition.x, targetHeight, targetPosition.z), transform.rotation);
        }
        else
        {
            Debug.LogError("SpawnManager.Instance is null");
        }
    }

    private List<RayCastDetect> GetAllComponents(GameObject gameObject)
    {
        List<RayCastDetect> allComponents = new List<RayCastDetect>();
        gameObject.GetComponentsInChildren(true, allComponents);
        return allComponents;
    }

    private Vector3 GetHighestPosition()
    {
        float maxY = float.MinValue;
        Vector3 highestPosition = Vector3.zero;

        foreach (var detector in rayCastDetect)
        {
            float y = detector.GetYHitPosition();
            if (y > maxY)
            {
                maxY = y;
                highestPosition = detector.GetHitPosition();
            }
        }
        return highestPosition;
    }

    private GameObject GetHitObject()
    {
        float maxY = float.MinValue;
        GameObject highestHitObject = null;

        foreach (var detector in rayCastDetect)
        {
            float y = detector.GetYHitPosition();
            if (y > maxY)
            {
                maxY = y;
                highestHitObject = detector.GetHitObject();
            }
        }
        return highestHitObject;
    }

    private GameObject GetObjectDetect()
    {
        float maxY = float.MinValue;
        GameObject objectDetected = null;

        foreach (var detector in rayCastDetect)
        {
            float y = detector.GetYHitPosition();
            if (y > maxY)
            {
                maxY = y;
                objectDetected = detector.gameObject;
            }
        }
        return objectDetected;
    }

    private void SetParentAllCube()
    {
        List<Vector3> worldPositions = new List<Vector3>();
        foreach (var cube in totalCube)
        {
            worldPositions.Add(cube.transform.position);
        }

        for (int i = 0; i < totalCube.Count; i++)
        {
            var cube = totalCube[i];
            cube.transform.SetParent(pivot, true);
        }

        centerPoint.SetParent(pivot, true);
    }
    private void SaveData(Vector3 pos, quaternion rotate)
    {
        CubeData cubeData = new();
        cubeData.InitShapeAndAngle(this.blockShape, this.blockAngle);
        cubeData.InitPositionRotation(pos, rotate);
        cubeData.InitGameObjectAndPivot(gameObject, pivot);
        cubeData.AddBlockController(this);
        cubeData.centerPoint = centerPoint;

        if (PositionManager.Instance != null)
        {
            PositionManager.Instance.SaveCubeType(cubeData);
        }
    }

    public bool CheckRoof()
    {
        return blockAngle == BlockAngle.FLAT ? CheckFlatRoof() : CheckStandRoof();
    }

    public bool CheckFlatRoof()
    {
        return rayCastDetect.Any(item => item.CheckBlockOnTop());
    }

    public bool CheckStandRoof()
    {
        return rayCastDetect.All(item => item.CheckBlockOnTop());
    }
}
