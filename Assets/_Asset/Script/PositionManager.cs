using System.Collections.Generic;
using BillUtils.GameObjectUtilities;
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
        // int index = data.GetBlockController().transform.GetSiblingIndex();
        // cubeDatas.Insert(index, data);
        cubeDatas.Add(data);
        if (cubeDatas.Count > 0 && processMesh != null)
        {
            GameObjectUtils.DisableAllMeshRenderers(data.Cube);
            SpawnMesh();
        }
        else if (processMesh == null)
        {
            Debug.LogError("ProcessMesh is null! Cannot spawn building.");
        }
    }

    public void SpawnMesh()
    {
        if (processMesh == null)
        {
            Debug.LogError("Cannot spawn building because processMesh is null.");
            return;
        }

        foreach (CubeData cubeData in cubeDatas)
        {
            // GameObjectUtils.DisableAllMeshRenderers(cubeData.Cube);
            processMesh.SpawnBuilding(cubeData);
        }
    }
    public void RemoveCubeData(Transform building)
    {
        cubeDatas.RemoveAt(building.GetSiblingIndex());
    }
}
