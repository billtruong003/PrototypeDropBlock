using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class PositionManager : Singleton<PositionManager>
{
    [SerializeField] private ProcessMesh processMesh;
    [SerializeField] private List<CubeData> cubeDatas;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float detectionRadius = 1.0f;


    protected override void Awake()
    {
        base.Awake();
        processMesh = gameObject.GetComponent<ProcessMesh>();
    }

    public void SaveCubeType(CubeData data)
    {
        cubeDatas.Add(data);
        if (cubeDatas.Count < 2)
            return;
        if (processMesh != null)
        {
            DisableAllMeshRenderers(data.cube);
            processMesh.SpawnBuilding(data);

        }
        else
        {
            Debug.LogError("processMesh is null! Cannot spawn building.");
        }
    }
    public void DisableAllMeshRenderers(GameObject target)
    {
        // Lấy tất cả các thành phần MeshRenderer từ target và các con của nó
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            // Vô hiệu hóa từng MeshRenderer
            meshRenderer.enabled = false;
        }
    }


    public void CheckBlocksAround()
    {
        foreach (CubeData cubeData in cubeDatas)
        {
            // Lấy vị trí của mỗi CubeData
            Vector3 cubePosition = cubeData.positionDrop;

            // Kiểm tra các hướng trên dưới
            CheckDirection(cubePosition, Vector3.up);
            CheckDirection(cubePosition, Vector3.down);

            // Kiểm tra các mặt ngang
            CheckDirection(cubePosition, Vector3.left);
            CheckDirection(cubePosition, Vector3.right);
            CheckDirection(cubePosition, Vector3.forward);
            CheckDirection(cubePosition, Vector3.back);
        }
    }

    private void CheckDirection(Vector3 origin, Vector3 direction)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, detectionRadius, blockLayer))
        {

            // Có block trong hướng direction
            Debug.Log($"Có block ở hướng {direction} từ vị trí {origin}!");
        }
        else
        {
            // Không có block trong hướng direction
            Debug.Log($"Không có block ở hướng {direction} từ vị trí {origin}.");
        }
    }
}
