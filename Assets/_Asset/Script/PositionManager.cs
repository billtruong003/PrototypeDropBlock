using System.Collections.Generic;
using UnityEngine;

public class PositionManager : Singleton<PositionManager>
{
    // Serialized fields
    [Header("Settings")]
    [SerializeField] private ProcessMesh processMesh;
    [SerializeField] private List<CubeData> cubeDatas = new List<CubeData>();
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float detectionRadius = 1.0f;

    // Initialization
    protected override void Awake()
    {
        base.Awake();
        processMesh = GetComponent<ProcessMesh>();
        if (processMesh == null)
        {
            Debug.LogError("ProcessMesh component is missing!");
        }
    }

    public void SaveCubeType(CubeData data)
    {
        cubeDatas.Add(data);
        if (cubeDatas.Count > 0 && processMesh != null)
        {
            DisableAllMeshRenderers(data.Cube);
            Spawn();
        }
        else if (processMesh == null)
        {
            Debug.LogError("ProcessMesh is null! Cannot spawn building.");
        }
    }

    public void Spawn()
    {
        if (processMesh == null)
        {
            Debug.LogError("Cannot spawn building because processMesh is null.");
            return;
        }

        foreach (CubeData cubeData in cubeDatas)
        {
            DisableAllMeshRenderers(cubeData.Cube);
            processMesh.SpawnBuilding(cubeData);
        }
    }

    public void CheckBlocksAround() // DOUBLE CHECK: THIS FUNCTION ONLY FOR DEBUGGING
    {
        foreach (CubeData cubeData in cubeDatas)
        {
            Vector3 cubePosition = cubeData.PositionDrop;
            CheckDirection(cubePosition, Vector3.up);
            CheckDirection(cubePosition, Vector3.down);
            CheckDirection(cubePosition, Vector3.left);
            CheckDirection(cubePosition, Vector3.right);
            CheckDirection(cubePosition, Vector3.forward);
            CheckDirection(cubePosition, Vector3.back);
        }
    }

    private void DisableAllMeshRenderers(GameObject target)
    {
        MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false;
        }
    }

    private void CheckDirection(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, detectionRadius, blockLayer))
        {
            Debug.Log($"Block detected in direction {direction} from position {origin}. Hit: {hit.collider.name}");
        }
        else
        {
            Debug.Log($"No block detected in direction {direction} from position {origin}.");
        }
    }
}
