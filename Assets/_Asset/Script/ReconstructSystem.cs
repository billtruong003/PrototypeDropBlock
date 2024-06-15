using System;
using System.Collections;
using System.Collections.Generic;
using BillUtils.GameObjectUtilities;
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

    private bool displayUI = false;
    private bool isRotating = false;
    protected override void Awake()
    {
        base.Awake();
        mainCam = Camera.main;
    }

    // Update is called once per frame
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
                    GameObjectUtils.EnableAllMeshRenderers(blockPick.gameObject);
                    HandleUIReconstruct(blockPick.GetDropPose(), mainCam.transform.position);

                }
            }
        }
    }

    private void TurnOffMesh(BuildingHandle buildingHandle)
    {
        vFXManager.TriggerExplo(buildingHandle.transform.position);
        buildingHandle.gameObject.SetActive(false);
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
    }

    public void RotateDone()
    {
        isRotating = false;
    }

    public void RotateBlock()
    {
        if (isRotating)
            return;
        blockPick.Rotate();
        isRotating = true;
    }

    public void MoveBlock()
    {

    }


    public Transform GetSelectedObjectTransform()
    {
        return blockPick.GetPivot();
    }
}