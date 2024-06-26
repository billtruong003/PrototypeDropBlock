using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using BillUtils.SerializeCustom;
using BillUtils.EnumUtilities;
using System.Collections;
using BillUtils.GameObjectUtilities;
using BillUtils.SpaceUtils;

public class BlockController : MonoBehaviour
{
    #region Fields and Properties
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float floatingSpeed = 1f;
    [SerializeField] private bool canRotate;

    [Header("Transforms")]
    [SerializeField] private Transform pivot;
    [SerializeField] private Transform centerPoint;
    [SerializeField] private GameObject visualBlock;

    [Header("Raycast and Cube Data")]
    [SerializeField] private List<RayCastDetect> rayCastDetects = new List<RayCastDetect>();
    [SerializeField] private List<GameObject> totalCube = new();

    [BillHeader("Data", 15, "#B0EBB4")]
    [BoxGroup("CubeData")]
    [SerializeField] private BlockShape blockShape;
    [BoxGroup("CubeData")]
    [SerializeField] private BlockAngle blockAngle;
    [BoxGroup("CubeData")]
    [SerializeField] private MaterialType materialType;
    [BoxGroup("CubeData")]
    [SerializeField] private CubeData cubeData;


    private BuildingHandle buildingHandle;
    private float targetHeight;
    private Vector3 targetPosition;
    private GameObject detectedObject;
    private GameObject hitObject;

    public bool DoneDrop { get; private set; } = false;

    public GameObject GetVisualBlock() => visualBlock;
    public Transform GetPivot() => pivot;
    public Vector3 GetCenter() => centerPoint.position;
    public GameObject GetCenterObj() => centerPoint.gameObject;

    public List<GameObject> GetTotalCube() => totalCube;
    public Vector3 GetDropPose() => targetPosition;
    public BlockShape getShape => blockShape;

    public void SetBuildingHandle(BuildingHandle buildingHandle) => this.buildingHandle = buildingHandle;
    public BuildingHandle GetBuildingHandle() => this.buildingHandle;

    public BlockController GetHitObjectController() => this.hitObject.transform.parent.parent.GetComponent<BlockController>();

    public ReconstructMode reconstructMode;
    public MaterialType GetMatType() => this.materialType;
    public void SetMatType(MaterialType matType) => materialType = matType;
    public void SwitchModeDoneDrop() => DoneDrop = !DoneDrop;

    public void AddCubeDate(CubeData cubeData) => this.cubeData = cubeData;
    public CubeData GetCubeData() => this.cubeData;


    #endregion

    #region Unity Methods
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!DoneDrop)
        {
            if (reconstructMode == ReconstructMode.OFF)
            {
                HandleMovement();
                HandleRotate();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    DropToCenter();
                    SoundManager.Instance.PlaySound(SoundType.S_DROP);
                }
            }
            else if (reconstructMode == ReconstructMode.ON)
            {
                HandleMovementReconstruct();
                CheckCollide();
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.J) && !isCollisionDetected)
                {
                    DropReconstruct();
                }
            }
        }

    }
    #endregion

    #region Initialization
    private void Init()
    {
        reconstructMode = ReconstructMode.OFF;
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
    #endregion

    #region Movement and Rotation
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
            SoundManager.Instance.PlaySound(SoundType.S_ROTATE);
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 90, transform.eulerAngles.z);
            transform.eulerAngles = angle;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SoundManager.Instance.PlaySound(SoundType.S_ROTATE);
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y - 90, transform.eulerAngles.z);
            transform.eulerAngles = angle;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SoundManager.Instance.PlaySound(SoundType.S_ROTATE);
            Vector3 angle = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
            transform.eulerAngles = angle;
            ChangeState();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SoundManager.Instance.PlaySound(SoundType.S_ROTATE);
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
    #endregion

    #region Drop Logic
    private void DropReconstruct()
    {
        DoneDrop = true;
        targetPosition = GetLowestPosition();
        hitObject = GetHitObjectReconstruct();
        detectedObject = GetObjectDetectReconstruct();

        if (detectedObject == null)
        {
            Debug.LogError("Detected object is null");
            return;
        }
        Vector3 currentPose = new Vector3(targetPosition.x, pivot.position.y, targetPosition.z);

        pivot.position = detectedObject.transform.position;

        SetParentAllCube();

        pivot.position = currentPose;

        Sequence mySequence = DOTween.Sequence();

        targetHeight = hitObject != null && hitObject.CompareTag("Block") ? targetPosition.y + 1 : targetPosition.y + 0.5f;
        targetPosition = new Vector3(currentPose.x, targetHeight, currentPose.z);
        mySequence.Append(pivot.DOMoveY(targetHeight, 0).SetEase(Ease.InQuad));

        Debug.Log("Sequence setup completed");
        mySequence.OnComplete(OnDropComplete);
        mySequence.Play();
        Debug.Log("Sequence started");
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

        if (SpawnManager.Instance != null && reconstructMode == ReconstructMode.OFF)
        {
            // SpawnManager.Instance.SpawnCube();
            SaveData(new Vector3(targetPosition.x, targetHeight, targetPosition.z), transform.eulerAngles);
            SpawnManager.Instance.ReadySpawn = true;
        }
        else if (SpawnManager.Instance != null && reconstructMode == ReconstructMode.ON)
        {
            reconstructMode = ReconstructMode.OFF;
            ReconstructSystem.Instance.ResetMoveMode();
            SpawnManager.Instance.ReadySpawn = true;
        }
        else
        {
            Debug.LogError("SpawnManager.Instance is null");
        }
    }

    public void ProcessMeshReconstruct()
    {
        SaveData(new Vector3(targetPosition.x, targetHeight, targetPosition.z), transform.eulerAngles);

    }
    #endregion

    #region BreakDown Methods
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

    private Vector3 GetLowestPosition()
    {
        float minY = float.MaxValue;
        Vector3 lowestPosition = Vector3.zero;

        foreach (var detector in rayCastDetects)
        {
            if (!detector.valid)
                continue;
            float y = detector.GetYHitPosition();
            if (y < minY)
            {
                minY = y;
                lowestPosition = detector.GetHitPosition();
            }
        }
        return lowestPosition;
    }

    private GameObject GetHitObjectReconstruct()
    {
        float minY = float.MaxValue;
        GameObject lowestHitObject = null;

        foreach (var detector in rayCastDetects)
        {
            if (!detector.valid)
                continue;
            float y = detector.GetYHitPosition();
            if (y < minY)
            {
                minY = y;
                lowestHitObject = detector.GetHitObject();
            }
        }
        return lowestHitObject;
    }

    private GameObject GetObjectDetectReconstruct()
    {
        float minY = float.MaxValue;
        GameObject objectDetectedReconstruct = null;

        foreach (var detector in rayCastDetects)
        {
            if (!detector.valid)
                continue;
            float y = detector.GetYHitPosition();
            if (y < minY)
            {
                minY = y;
                objectDetectedReconstruct = detector.gameObject;
            }
        }
        return objectDetectedReconstruct;
    }

    public void SetParentAllCube()
    {
        for (int i = 0; i < totalCube.Count; i++)
        {
            var cube = totalCube[i];
            cube.transform.SetParent(pivot, true);
        }

        centerPoint.SetParent(pivot, true);
    }

    public void RemovePivotParent()
    {
        for (int i = 0; i < totalCube.Count; i++)
        {
            var cube = totalCube[i];
            cube.transform.SetParent(this.transform, true);
        }

        centerPoint.SetParent(this.transform, true);
        this.transform.position += VectorUtils.HalfY; //NOTE: (0, 0.5f, 0)
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

        SpawnMesh(cubeData);
    }

    public void SpawnMesh(CubeData cubeData)
    {
        if (PositionManager.Instance != null)
        {
            PositionManager.Instance.SaveCubeType(cubeData);
            SoundManager.Instance.PlaySound(SoundType.S_TRANSFORM);
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

        ReconstructVisual.Instance.SetUpMaterialBlock(visualBlock, materialTypes[randomIndex]);

        return materialTypes[randomIndex];
    }

    public void TransparentRoof()
    {
        if (buildingHandle == null || !buildingHandle.gameObject.activeSelf)
            return;
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

    #endregion
    #region Reconstruct
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float checkInterval = 0.01f;

    private void HandleMovementReconstruct()
    {
        float moveX = Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, moveZ), Space.World);
    }

    public void Rotate()
    {
        StopAllCoroutines();
        StartCoroutine(RotateCoroutine());
        // buildingHandle.Rotate();
    }

    private IEnumerator RotateCoroutine()
    {
        Transform targetTransform = ReconstructSystem.Instance.GetSelectedObjectTransform();
        if (targetTransform == null)
        {
            Debug.LogWarning("No selected object to rotate.");
            yield break;
        }

        initialPosition = targetTransform.position;
        initialRotation = targetTransform.rotation;

        Quaternion targetRotation = targetTransform.rotation * Quaternion.Euler(0, 90, 0);

        float duration = 0.1f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float step = elapsedTime / duration;
            targetTransform.rotation = Quaternion.Slerp(initialRotation, targetRotation, step);

            if (CheckCollision(targetTransform))
            {

                targetTransform.rotation = initialRotation;
                targetTransform.position = initialPosition;

                ReconstructVisual.Instance.RotateDialogueDisplay(true);
                targetTransform.DOShakeRotation(0.5f, new Vector3(0, 5, 0), 20, 90, false)
                .OnComplete(() =>
                {
                    ReconstructVisual.Instance.RotateDialogueDisplay(false);
                    ReconstructSystem.Instance.RotateDone();
                });
                yield break;
            }

            elapsedTime += checkInterval;
            yield return new WaitForSeconds(checkInterval);
        }

        if (!CheckCollision(targetTransform))
        {
            SoundManager.Instance.PlaySound(SoundType.S_ROTATE);
        }

        targetTransform.rotation = targetRotation;
        ReconstructSystem.Instance.RotateDone();
    }

    private bool CheckCollision(Transform targetTransform)
    {
        Collider[] hitColliders = Physics.OverlapBox(
            targetTransform.position,
            targetTransform.localScale / 2.5f,
            targetTransform.rotation
        );

        foreach (var collider in hitColliders)
        {
            if (collider.gameObject != targetTransform.gameObject && collider.CompareTag("Block") && !collider.transform.IsChildOf(targetTransform))
            {
                return true;
            }
        }


        return false;
    }
    public void BackToOriginalPose(Vector3 originalPos)
    {
        SetParentAllCube();
        this.transform.position -= VectorUtils.HalfY;
        this.transform.position = originalPos;
        ProcessMeshReconstruct();
        SwitchModeDoneDrop();
        reconstructMode = ReconstructMode.OFF;
        ReconstructSystem.Instance.ResetMoveMode();
    }

    [Space]
    [Header("Reconstruct")]
    [SerializeField] private Vector3 boxSize = new Vector3(0.97f, 0.97f, 0.97f);
    [SerializeField] private LayerMask collisionLayer = 1 << 3;
    private Color originalCol = new Color(1, 1, 1, 1);
    private Color collideCol = new Color(1f, 0, 0, 0.8f);
    private bool isCollisionDetected;
    private void CheckCollide()
    {
        if (totalCube == null || totalCube.Count == 0)
            return;

        foreach (GameObject child in totalCube)
        {
            if (child == null)
                continue;

            Vector3 startPosition = child.transform.position;
            Quaternion rotation = child.transform.rotation;
            Vector3 boxHalfExtents = boxSize / 2;

            isCollisionDetected = false;

            Collider[] colliders = Physics.OverlapBox(
                startPosition,
                boxHalfExtents,
                Quaternion.identity,
                collisionLayer
            );

            foreach (var collider in colliders)
            {
                if (collider.gameObject.CompareTag("Block") && collider.transform.parent != this.transform)
                {
                    isCollisionDetected = true;
                    Debug.Log($"Hit object: {collider.name} at {collider.transform.position}");
                    break;
                }
            }

            if (isCollisionDetected)
            {
                ReconstructVisual.Instance.MoveDialogueDisplay(true);
                break;
            }
            else
            {

                ReconstructVisual.Instance.MoveDialogueDisplay(false);
            }
        }

        foreach (GameObject child in totalCube)
        {
            if (child != null)
            {
                Renderer renderer = child.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = isCollisionDetected ? collideCol : originalCol;
                }
            }
        }
    }


    private void OnDrawGizmos()
    {
        if (totalCube == null || totalCube.Count == 0)
            return;

        Bounds bounds = SpaceUtilities.CalculateBounds(totalCube);
        Vector3 center = bounds.center;
        Vector3 halfExtents = bounds.size / 2;
        Vector3 direction = transform.forward;
        Vector3 endPosition = center + direction * 0;

        Gizmos.color = isCollisionDetected ? Color.red : Color.green;

        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireCube(center, bounds.size);

        Gizmos.DrawLine(center, endPosition);
        Gizmos.DrawWireCube(endPosition, bounds.size);


        Transform targetTransform = GetPivot();
        if (targetTransform == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(initialPosition, 0.1f);
        Gizmos.DrawLine(initialPosition, initialPosition + initialRotation * Vector3.forward * 2.0f);
        Gizmos.DrawLine(initialPosition, initialPosition + initialRotation * Vector3.up * 1.0f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetTransform.position, 0.1f);
        Gizmos.DrawLine(targetTransform.position, targetTransform.position + targetTransform.rotation * Vector3.forward * 2.0f);
        Gizmos.DrawLine(targetTransform.position, targetTransform.position + targetTransform.rotation * Vector3.up * 1.0f);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(initialPosition, targetTransform.position);
    }

    public bool CheckBlockOnTop()
    {
        if (blockShape == BlockShape.BIGPLANE && blockAngle == BlockAngle.STAND) // TODO: FIND A NEW WAY TO IMPLEMENT
            return CheckBigPlaneExcept();

        return blockAngle == BlockAngle.FLAT ? CheckFlatRoof() : CheckStandRoof();
    }

    #endregion

}
