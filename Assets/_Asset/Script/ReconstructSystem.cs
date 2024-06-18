using System;
using System.Collections;
using System.Collections.Generic;
using BillUtils.GameObjectUtilities;
using BlockBuilder.BlockManagement;
using Unity.VisualScripting;
using UnityEngine;

public class ReconstructSystem : Singleton<ReconstructSystem>
{

    [SerializeField] private VFXManager vFXManager;

    [SerializeField] private Camera mainCam;
    [SerializeField] private PositionManager positionManager;
    [SerializeField] private LayerMask buildingLayer;

    [SerializeField] private BlockController blockPick;
    [SerializeField] private BuildingHandle buildingHandle;

    [SerializeField] private GameObject UIReconstruct;
    [SerializeField] private CubeReconstruct cubeReconstruct;
    [SerializeField] private ButtonController btnController;
    private BlockController currentBlock;
    private bool displayUI = false;
    private bool isRotating = false;
    protected override void Awake()
    {
        base.Awake();
        mainCam = Camera.main;
    }

    void Update()
    {
        ClickOnBuilding();
    }

    private void ClickOnBuilding()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (displayUI)
                return;

            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
            {
                Debug.DrawLine(mainCam.transform.position, hit.point, Color.red, 2.0f);

                if (hit.collider.gameObject.CompareTag("Block"))
                {
                    RayCastDetect rayDetect = hit.collider.gameObject.GetComponent<RayCastDetect>();
                    if (rayDetect.GetDropState() == false)
                        return;
                    blockPick = rayDetect.GetBlockController();
                    buildingHandle = blockPick.GetBuildingHandle();
                    vFXManager.TriggerExplo(blockPick.GetDropPose());
                    btnController.AddBlockController(blockPick);

                    TurnOffMesh(buildingHandle);
                    TurnOnMesh(blockPick);
                    HandleUIReconstruct(blockPick.GetDropPose(), mainCam.transform.position);
                    SwitchModeReconstruct();
                }
            }
        }
    }

    private void TurnOffMesh(BuildingHandle buildingHandle)
    {
        vFXManager.TriggerExplo(buildingHandle.transform.position);
        buildingHandle.gameObject.SetActive(false);
    }
    private void TurnOnMesh(BlockController blockController)
    {
        GameObjectUtils.EnableAllMeshRenderers(blockController.gameObject);
    }

    private void HandleUIReconstruct(Vector3 pointA, Vector3 pointB)
    {

        Vector3 direction = (pointB - pointA).normalized * 2;

        Vector3 newPosition = pointA + (direction);

        UIReconstruct.transform.position = newPosition;
        DisplayUI();
    }

    public void DisplayUI()
    {
        displayUI = true;
        UIReconstruct.SetActive(true);
    }

    public void HideUI()
    {
        displayUI = false;
        UIReconstruct.SetActive(false);
        SwitchModeReconstruct();
    }

    public void RotateDone()
    {
        Debug.Log("Rotate Done!");
        isRotating = false;
    }

    public void RotateBlock()
    {
        if (isRotating)
            return;
        isRotating = true;
        blockPick.Rotate();

    }

    public void MoveBlock()
    {
        blockPick.SwitchModeDoneDrop();
        blockPick.RemovePivotParent();
        blockPick.reconstructMode = ReconstructMode.ON;
    }

    public Transform GetSelectedObjectTransform()
    {
        return blockPick.GetPivot();
    }

    private void SwitchModeReconstruct()
    {
        currentBlock = SpawnManager.Instance.CurrentBlock;
        currentBlock.SwitchModeDoneDrop();
    }
}