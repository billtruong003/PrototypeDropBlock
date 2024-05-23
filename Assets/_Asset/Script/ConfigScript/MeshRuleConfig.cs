using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockBuilder.BlockManagement;
using System;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "MeshRuleConfig", menuName = "BlockBuilder/BuildingConfig")]
public class MeshRuleConfig : ScriptableObject
{
    [CustomHeader("Building Data", 15, "#F6DCAC")]
    [SerializeField] private List<BlockGenInfo> buildingDatas;
    [SerializeField] private const string PATH_BUILDING = "BDS001";
    [CustomHeader("Building Data", 15, "#FED8B1")]
    [SerializeField] private List<MaterialApplyInfo> materialDatas;
    [SerializeField] private const string PATH_MATERIAL = "Material";

    [Button]
    private void GenerateBlockData()
    {
        buildingDatas.Clear();
        foreach (BuildingType type in Enum.GetValues(typeof(BuildingType)))
        {
            BlockGenInfo newData = new BlockGenInfo
            {
                PathToBuilding = $"{PATH_BUILDING}/{type.ToString()}",
                Type = type,
            };
            buildingDatas.Add(newData);
        }
    }

    [Button]
    private void ClearBlockData()
    {
        buildingDatas.Clear();
    }

    public GameObject FindMatchBuilding(BuildingType buildingType)
    {
        // Kiểm tra buildingDatas có null hoặc trống không
        if (buildingDatas == null || buildingDatas.Count == 0)
        {
            Debug.LogError("buildingDatas is null or empty!");
            return null;
        }

        foreach (var item in buildingDatas)
        {
            if (item.Type == buildingType)
            {
                GameObject building = item.GetBuilding();

                // Kiểm tra GetBuilding trả về null
                if (building == null)
                {
                    Debug.LogError($"GetBuilding returned null for buildingType: {buildingType}");
                }

                return building;
            }
        }

        Debug.LogWarning($"No matching building found for buildingType: {buildingType}");
        return null;
    }

    public List<GameObject> GetBlockPrefab()
    {
        List<GameObject> blocks = new();
        foreach (var item in buildingDatas)
        {
            GameObject block = item.GetBuilding();
            blocks.Add(block);
        }
        return blocks;
    }
}
