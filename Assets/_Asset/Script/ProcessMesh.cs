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
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float detectionRadius = 1.0f;
    public void SpawnBuilding(CubeData cubeData)
    {
        if (cubeData.mesh != null)
        {
            cubeData.ConditionalRoof();
            return;
        }
        BlockShape shape = cubeData.Shape;
        BlockAngle angle = cubeData.Angle;
        Vector3 dropPosition = cubeData.centerPoint.position;

        GameObject building = ProcessCubeCode(shape, angle);
        GameObject spawnBuilding = Instantiate(building, meshContainer);
        cubeData.AddMesh(spawnBuilding);
        spawnBuilding.transform.SetLocalPositionAndRotation(dropPosition, quaternion.identity);
        spawnBuilding.transform.eulerAngles = new Vector3(0, -cubeData.rotateDrop.y, 0);
        Anim.JellyBounce(spawnBuilding.transform);
        vFXManager.TriggerExplo(dropPosition);
        Debug.Log($"Position: {dropPosition}, Roof:");
        cubeData.ConditionalRoof();
    }

    public GameObject ProcessCubeCode(BlockShape blockShape, BlockAngle blockAngle)
    {
        return combineRuleConfig.GetReturnBlock(blockShape, blockAngle);
    }


}