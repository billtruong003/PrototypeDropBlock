using System.Collections;
using System.Collections.Generic;
using BlockBuilder.BlockManagement;
using Unity.Mathematics;
using UnityEngine;
using AnimationController;
using AnimationController.WithTransform;
using UnityEngine.UIElements;

public class ProcessMesh : MonoBehaviour
{
    [SerializeField] private CombineRuleConfig combineRuleConfig;
    [SerializeField] private VFXManager vFXManager;
    [SerializeField] private Transform meshContainer;


    public void SpawnBuilding(CubeData cubeData)
    {
        if (cubeData.Mesh != null)
        {
            cubeData.ConditionalRoof();
            return;
        }

        BlockShape shape = cubeData.Shape;
        BlockAngle angle = cubeData.Angle;
        MaterialType matType = cubeData.MaterialType;
        Vector3 dropPosition = cubeData.CenterPoint.position;

        GameObject building = ProcessCubeCode(shape, angle, matType);
        GameObject spawnBuilding = Instantiate(building, meshContainer);

        AssignBuildingDataToCube(cubeData, spawnBuilding);

        Material mainMat = GetMainMat(cubeData.MaterialType);
        Material emissionLight = GetLight(cubeData.MaterialType);
        Material transMat = GetTNSMat(cubeData.MaterialType);
        cubeData.AssignMaterialToBuildingHandle(mainMat, emissionLight, transMat);

        spawnBuilding.transform.SetLocalPositionAndRotation(dropPosition, Quaternion.identity);
        spawnBuilding.transform.eulerAngles = new Vector3(0, -cubeData.RotateDrop.y, 0);

        Anim.JellyBounce(spawnBuilding.transform.GetChild(0));
        vFXManager.TriggerExplo(dropPosition);

        Debug.Log($"Position: {dropPosition}, Roof:");
        cubeData.ConditionalRoof();
        cubeData.DisplayFurnitureSideAndApplyMaterial();
        cubeData.SetPivot();
    }

    private void AssignBuildingDataToCube(CubeData cubeData, GameObject spawnBuilding)
    {
        BuildingHandle buildingHandle = spawnBuilding.GetComponent<BuildingHandle>();
        cubeData.SetBuildingHandle(buildingHandle);
        cubeData.AssignBuildingHandleToBlockController();
        cubeData.SetMesh(spawnBuilding);
    }

    public GameObject ProcessCubeCode(BlockShape blockShape, BlockAngle blockAngle, MaterialType materialType)
    {
        return combineRuleConfig.GetReturnBlock(blockShape, blockAngle, materialType);
    }

    public Material GetMainMat(MaterialType matType)
    {
        return combineRuleConfig.GetMainMat(matType);
    }
    public Material GetLight(MaterialType matType)
    {
        return combineRuleConfig.GetEmissionLight(matType);
    }

    public Material GetTNSMat(MaterialType matType)
    {
        return combineRuleConfig.GetTransparentMat(matType);
    }


}