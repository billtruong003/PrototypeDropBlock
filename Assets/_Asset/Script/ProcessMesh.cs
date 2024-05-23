using System.Collections;
using System.Collections.Generic;
using BlockBuilder.BlockManagement;
using Unity.Mathematics;
using UnityEngine;
using AnimationController;
using AnimationController.WithTransform;
public class ProcessMesh : MonoBehaviour
{
    [SerializeField] private CombineRuleConfig combineRuleConfig;
    [SerializeField] private VFXManager vFXManager;
    [SerializeField] private Transform meshContainer;

    public void SpawnBuilding(CubeData cubeData)
    {
        SpawnBuilding(cubeData.Shape, cubeData.Angle, cubeData.positionDrop);
        vFXManager.TriggerExplo(cubeData.positionDrop);
        Debug.Log($"{cubeData.positionDrop}");

    }

    public GameObject ProcessCubeCode(BlockShape blockShape, BlockAngle blockAngle)
    {
        return combineRuleConfig.GetReturnBlock(blockShape);
    }
    public void ProcessItemOnCube(GameObject building, bool triggerRoof = false, bool triggerFurniture = true)
    {
        BuildingHandle buildingHandle = building.GetComponent<BuildingHandle>();
        if (triggerRoof)
        {
            buildingHandle.TurnOnRoof();
        }
        if (triggerFurniture)
        {
            buildingHandle.TurnOnFurniture();
        }
    }
    public void SpawnBuilding(BlockShape shape, BlockAngle angle, Vector3 dropPosition)
    {
        GameObject building = ProcessCubeCode(shape, angle);
        GameObject spawnBuilding = Instantiate(building, meshContainer);
        spawnBuilding.transform.SetLocalPositionAndRotation(dropPosition, quaternion.identity);
        Anim.JellyBounce(spawnBuilding.transform);
    }
}

