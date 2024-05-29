using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BlockBuilder.BlockManagement;

[CreateAssetMenu(fileName = "CombineRule", menuName = "BlockBuilder/CombineRule")]
public class CombineRuleConfig : ScriptableObject
{
    public List<CombineBuildingAndMaterial> combineMat;
    public MeshRuleConfig ruleConfig;
    public GameObject GetReturnBlock(BlockShape shape, BlockAngle angle)
    {
        if (ruleConfig == null)
        {
            Debug.LogError("ruleConfig is null!");
            return null;
        }

        BuildingType buildingType = GetBuildingType(shape, angle);
        GameObject building = ruleConfig.FindMatchBuilding(buildingType);

        // Kiểm tra xem building có bị null không
        if (building == null)
        {
            Debug.LogError("building is null!");
            return null;
        }

        BuildingHandle buildingHandle = building.GetComponent<BuildingHandle>();

        // Kiểm tra xem buildingHandle có bị null không
        if (buildingHandle == null)
        {
            Debug.LogError("buildingHandle is null!");
            return null;
        }
        return building;
    }
    public BuildingType GetBuildingType(BlockShape shape, BlockAngle angle)
    {
        foreach (var item in combineMat)
        {
            if (item.shape == shape && item.angle == angle)
                return item.GetBuilding();
        }
        return BuildingType.BD_001;
    }

    public Material GetMainMat(MaterialType matType)
    {
        return ruleConfig.GetMainMat(matType);
    }
    public Material GetEmissionLight(MaterialType matType)
    {
        return ruleConfig.GetEmissionLightMat(matType);
    }
}