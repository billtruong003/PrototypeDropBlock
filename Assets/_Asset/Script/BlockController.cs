using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using BillUtils.SerializeCustom;
using BillUtils.EnumUtilities;
using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;

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
    [SerializeField] private List<RayCastDetect> rayCastDetects = new List<RayCastDetect>();
    [SerializeField] private List<GameObject> totalCube = new();

    // Block data
    [BillHeader("Data", 15, "#B0EBB4")]
    [BoxGroup("CubeData")]
    [SerializeField] private BlockShape blockShape;
    [BoxGroup("CubeData")]
    [SerializeField] private BlockAngle blockAngle;
    [BoxGroup("CubeData")]
    [SerializeField] private MaterialType materialType;


    private BuildingHandle buildingHandle;
    private float targetHeight;
    private Vector3 targetPosition;
    private GameObject detectedObject;
    private GameObject hitObject;

    public bool DoneDrop { get; private set; } = false;

    public Transform GetPivot() => pivot;
    public Vector3 GetCenter() => centerPoint.position;
    public List<GameObject> GetTotalCube() => totalCube;
    public void SetBuildingHandle(BuildingHandle buildingHandle) => this.buildingHandle = buildingHandle;
    public BlockController GetHitObjectController() => this.hitObject.transform.parent.parent.GetComponent<BlockController>();
    public GameObject GetCenterObj() => centerPoint.gameObject;
    public BlockShape getShape => blockShape;
    public Vector3 GetDropPose() => targetPosition;
    public BuildingHandle GetBuildingHandle() => this.buildingHandle;

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
        rayCastDetects = GetAllComponentRaycast(gameObject);
        GetAvailableVisualGuide();

        if (SpawnManager.Instance != null && SpawnManager.Instance.CheckMatCheat())
        {
            materialType = SpawnManager.Instance.GetCheatMat();
            UIManager.Instance.SetCurrentMat(materialType);
            return;
        }

        materialType = RandomizeMaterialType();
        UIManager.Instance.SetCurrentMat(materialType);
    }


    private void GetAvailableVisualGuide()
    {
        Debug.LogWarning("CheckVSGUide");
        StartCoroutine(Cor_GetAvailableVisualGuide());
    }

    private IEnumerator Cor_GetAvailableVisualGuide()
    {
        yield return new WaitUntil(() => rayCastDetects.Count != 0);
        foreach (RayCastDetect itemRay in rayCastDetects)
        {
            VisualGuideController vsGuide = VisualGuide.Instance.GetAvailableVisualGuide();
            if (vsGuide != null)
            {
                itemRay.SetVisualGuide(vsGuide);
                Debug.Log("Visual guide set for raycast detector.");
            }
            else
            {
                Debug.LogWarning("No available visual guide to set.");
            }
        }
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, moveZ), Space.World);
    }

    private void HandleRotate()
    {
        if (!canRotate)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
            transform.eulerAngles = angle;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
            transform.eulerAngles = angle;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
            transform.eulerAngles = angle;
            ChangeState();
        }
        if (Input.GetKeyDown(KeyCode.F))
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
        Vector3 targetPose = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        pivot.position = detectedObject.transform.position;

        SetParentAllCube();

        pivot.position = targetPose;

        Sequence mySequence = DOTween.Sequence();

        targetHeight = hitObject != null && hitObject.CompareTag("Block") ? targetPosition.y + 1 : targetPosition.y + 0.5f;
        targetPosition = new Vector3(targetPose.x, targetHeight, targetPose.z);
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

    private List<RayCastDetect> GetAllComponentRaycast(GameObject gameObject)
    {
        List<RayCastDetect> allComponents = new List<RayCastDetect>();
        gameObject.GetComponentsInChildren(true, allComponents);
        return allComponents;
    }

    private Vector3 GetHighestPosition()
    {
        float maxY = float.MinValue;
        Vector3 highestPosition = Vector3.zero;

        foreach (var detector in rayCastDetects)
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

        foreach (var detector in rayCastDetects)
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

        foreach (var detector in rayCastDetects)
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
        if (blockShape == BlockShape.BIGPLANE && blockAngle == BlockAngle.STAND) // TODO: FIND A NEW WAY TO IMPLEMENT
            return CheckBigPlaneExcept();

        return blockAngle == BlockAngle.FLAT ? CheckFlatRoof() : CheckStandRoof();
    }

    public bool CheckBigPlaneExcept()
    {
        int count = 0;
        foreach (var item in rayCastDetects)
        {
            if (item.CheckBlockOnTop() == true)
            {
                count++;
            }
        }
        if (count > 4)
            return true;
        return false;
    }


    public bool CheckFlatRoof()
    {
        foreach (var item in rayCastDetects)
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
        foreach (var item in rayCastDetects)
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
        MaterialType[] materialTypes = (MaterialType[])System.Enum.GetValues(typeof(MaterialType));
        int randomIndex = Random.Range(0, materialTypes.Length);
        return materialTypes[randomIndex];
    }

    public void TransparentRoof()
    {
        buildingHandle.SetRoofTransparent();
    }

    public void ResetRoof()
    {
        buildingHandle.ResetRoof();
    }

    public bool CheckRoofIsFull()
    {
        int count = 0;
        foreach (RayCastDetect item in rayCastDetects)
        {
            if (item.CheckBlockOnTop() == true)
            {
                count++;
            }
        }
        if (count == rayCastDetects.Count)
        {
            return true;
        }
        return false;
    }

    public static void MoveParentButKeepChildren(Transform parent, Vector3 newPosition)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }

        foreach (Transform child in children)
        {
            child.SetParent(null);
        }

        parent.position = newPosition;

        foreach (Transform child in children)
        {
            child.SetParent(parent);
        }
    }

    public void Rotate()
    {
        StartCoroutine(RotateCoroutine());
        buildingHandle.Rotate();
    }

    private IEnumerator RotateCoroutine()
    {
        Transform targetTransform = ReconstructSystem.Instance.GetSelectedObjectTransform();
        if (targetTransform == null)
        {
            Debug.LogWarning("No selected object to rotate.");
            yield break;
        }

        Quaternion targetRotation = targetTransform.rotation * Quaternion.Euler(0, 90, 0);

        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            targetTransform.rotation = Quaternion.Slerp(targetTransform.rotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.rotation = targetRotation;

        ReconstructSystem.Instance.RotateDone();
    }
}
