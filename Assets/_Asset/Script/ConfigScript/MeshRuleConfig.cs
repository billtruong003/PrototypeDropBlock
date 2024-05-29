using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockBuilder.BlockManagement;
using System;
using NaughtyAttributes;
using BillUtils.SerializeCustom;

[CreateAssetMenu(fileName = "MeshRuleConfig", menuName = "BlockBuilder/BuildingConfig")]
public class MeshRuleConfig : ScriptableObject
{

    [SerializeField] private List<BlockGenInfo> buildingDatas;
    [SerializeField] private const string PATH_BUILDING = "BDS001";


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

    [CustomHeader("Building Data", 15, "#FED8B1")]
    [SerializeField] private List<MaterialApplyInfo> materialDatas;
    [SerializeField] private const string PATH_MATERIAL = "Material";

    [Button]
    private void GenMats()
    {
        materialDatas.Clear();
        foreach (MaterialType type in Enum.GetValues(typeof(MaterialType)))
        {
            MaterialApplyInfo materialData = new MaterialApplyInfo
            {
                MatType = type,
                PathToMaterial = $"{PATH_MATERIAL}/{type.ToString()}"
            };

            materialData.MainMat = Resources.Load<Material>(materialData.PathToMaterial);
            materialData.EmissionLight = Resources.Load<Material>($"{PATH_MATERIAL}/EMISSION_{type.ToString()}");
            if (materialData.MainMat == null)
            {
                Debug.LogError($"Material not found for type {type} at path {materialData.PathToMaterial}");
                materialData.MainMat = Resources.Load<Material>($"{PATH_MATERIAL}/MAT_014");
            }
            if (materialData.EmissionLight == null)
            {
                Debug.LogError($"Material not found for type {type} at path {materialData.PathToMaterial}");
                materialData.EmissionLight = Resources.Load<Material>($"{PATH_MATERIAL}/EMISSION_CLAY");
            }

            materialDatas.Add(materialData);
        }
    }
    [Button]
    private void ClearMatData()
    {
        materialDatas.Clear();
    }

    public Material GetMainMat(MaterialType matType)
    {
        foreach (var materialData in materialDatas)
        {
            if (materialData.MatType == matType)
            {
                return materialData.MainMat;
            }
        }
        Debug.LogWarning($"No main material found for type {matType}");
        return null;
    }
    public Material GetEmissionLightMat(MaterialType matType)
    {
        foreach (var materialData in materialDatas)
        {
            if (materialData.MatType == matType)
            {
                return materialData.EmissionLight;
            }
        }
        Debug.LogWarning($"No main material found for type {matType}");
        return null;
    }

}