using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using System.Linq;
using BillUtils.SerializeCustom;

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
    [BoxGroup("CubeData")]
    [SerializeField] private MaterialType materialType;

    public BuildingHandle BuildingHandle;
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
#if UNITY_EDITOR
        if (SpawnManager.Instance != null && SpawnManager.Instance.CheckMatCheat())
        {
            materialType = SpawnManager.Instance.GetCheatMat();
            return;
        }
#endif
        RandomMat();
    }

    private void RandomMat()
    {
        materialType = RandomizeMaterialType();
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
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
            transform.eulerAngles = angle;
        }
        if (Input.GetKeyDown(KeyCode.Q)) // Xoay quanh trục Y (left)
        {
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
            transform.eulerAngles = angle;
        }
        if (Input.GetKeyDown(KeyCode.R)) // Xoay quanh trục X (up)
        {
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
            transform.eulerAngles = angle;
            ChangeState();
        }
        if (Input.GetKeyDown(KeyCode.F)) // Xoay quanh trục X (down)
        {
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90);
            transform.eulerAngles = angle;
            ChangeState();
        }

    }
    private void ChangeState()
    {
        if (blockAngle == BlockAngle.FLAT)
        {
            blockAngle = BlockAngle.STAND;
        }
        else
        {
            blockAngle = BlockAngle.FLAT;
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
            SaveData(new Vector3(targetPosition.x, targetHeight, targetPosition.z), transform.eulerAngles);
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
            if (!detector.valid)
                continue;
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
            if (!detector.valid)
                continue;
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
            if (!detector.valid)
                continue;
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
    private void SaveData(Vector3 pos, Vector3 angle)
    {
        CubeData cubeData = new();

        cubeData.SetShape(this.blockShape);
        cubeData.SetAngle(this.blockAngle);
        cubeData.SetMaterialType(this.materialType);
        cubeData.SetPositionDrop(pos);
        cubeData.SetRotateDrop(angle);
        cubeData.SetCube(gameObject);
        cubeData.SetPivot(pivot);
        cubeData.SetBlockController(this);
        cubeData.SetCenterPoint(this.centerPoint);

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
        Debug.Log("CheckFlatRoof");
        foreach (var item in rayCastDetect)
        {
            if (item.CheckBlockOnTop() == true)
            {
                return true;
            }
        }
        return false;
    }

    public bool CheckStandRoof()
    {
        Debug.Log("CheckStandRoof");
        foreach (var item in rayCastDetect)
        {
            if (item.CheckBlockOnTop() == false)
            {
                return false;
            }
        }
        return true;
    }

    private MaterialType RandomizeMaterialType()
    {
        // Get all possible values of MaterialType enum
        MaterialType[] materialTypes = (MaterialType[])System.Enum.GetValues(typeof(MaterialType));

        // Randomly select one of the material types
        int randomIndex = Random.Range(0, materialTypes.Length);
        return materialTypes[randomIndex];
    }
}
