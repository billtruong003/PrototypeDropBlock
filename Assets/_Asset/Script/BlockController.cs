using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using UnityEngine.UIElements;
using Unity.Mathematics;
public class BlockController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float floatingSpeed = 2f;
    [SerializeField] private Transform pivot;
    [SerializeField] private List<RayCastDetect> rayCastDetect = new List<RayCastDetect>();
    [SerializeField] private List<GameObject> totalCube = new();

    [CustomHeader("Data", 15, "#B0EBB4")]
    [BoxGroup("CubeData")]
    [SerializeField] private BlockShape blockShape;
    [BoxGroup("CubeData")]
    [SerializeField] private BlockAngle blockAngle;


    private float targetHeight;
    private bool isDropping = false;
    private Vector3 targetPosition;
    private GameObject detectedObject;
    private GameObject hitObject;



    public bool DoneDrop { get; private set; } = false;

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!DoneDrop)
        {
            HandleMovement();
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
        transform.Translate(new Vector3(moveX, 0, moveZ));
    }

    private void DropToCenter()
    {
        DoneDrop = true;
        targetPosition = GetHighestPosition();
        hitObject = GetHitObject();
        detectedObject = GetObjectDetect();

        // Ensure detectedObject is valid before proceeding
        if (detectedObject == null)
        {
            Debug.LogError("Detected object is null");
            return;
        }

        // Set pivot position before setting parent
        pivot.position = detectedObject.transform.position;

        // Set parent for all cubes
        SetParentAllCube();

        // Move pivot to target position
        Vector3 targetPose = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        pivot.position = targetPose;

        Sequence mySequence = DOTween.Sequence();

        if (hitObject != null && hitObject.CompareTag("Block"))
        {
            targetHeight = targetPosition.y + 1;

        }
        else
        {
            targetHeight = targetPosition.y + 0.5f;
        }
        mySequence.Append(pivot.DOMoveY(targetHeight, floatingSpeed).SetEase(Ease.InQuad));

        // Debug logging to ensure the sequence is set up correctly
        Debug.Log("Sequence setup completed");
        mySequence.OnComplete(() =>
        {
            OnDropComplete();
        });

        // Debug logging to ensure the sequence starts
        mySequence.Play();
        Debug.Log("Sequence started");
    }


    private void OnDropComplete()
    {
        Debug.Log("OnDropComplete called");

        if (SpawnManager.Instance != null)
        {
            PositionManager.Instance.CheckBlocksAround();
            SpawnManager.Instance.SpawnCube();
            Vector3 poseSave = new Vector3(targetPosition.x, targetHeight, targetPosition.z);
            Quaternion rotation = transform.rotation;
            SaveData(poseSave, rotation);
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

    private void SetAllBlockMaterials()
    {
        foreach (var detector in rayCastDetect)
        {
            detector.SetBackBlockMat();
        }
    }
    private void SetParentAllCube()
    {
        // Cache the current world positions of the cubes
        List<Vector3> worldPositions = new List<Vector3>();
        foreach (var cube in totalCube)
        {
            worldPositions.Add(cube.transform.position);
        }

        // Set the parent of each cube to the pivot
        for (int i = 0; i < totalCube.Count; i++)
        {
            var cube = totalCube[i];
            cube.transform.SetParent(pivot);

            // Convert the cached world positions to local positions relative to the pivot
            cube.transform.position = worldPositions[i];
        }
    }

    private void SaveData(Vector3 pos, quaternion rotate)
    {
        CubeData cubeData = new();
        cubeData.InitShapeAndAngle(this.blockShape, this.blockAngle);
        cubeData.InitPositionRotation(pos, rotate);
        cubeData.InitGameObjectAndPivot(gameObject, pivot);

        if (PositionManager.Instance != null)
        {
            PositionManager.Instance.SaveCubeType(cubeData);
        }
    }

}
