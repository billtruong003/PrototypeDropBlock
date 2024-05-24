using System;
using System.Collections;
using System.Collections.Generic;
using BlockBuilder.BlockManagement;
using UnityEngine;

[Serializable]
public class CubeData
{
    public BlockShape Shape;
    public BlockAngle Angle;
    public Vector3 positionDrop;
    public Quaternion rotateDrop;
    public GameObject cube;
    public Transform pivot;
    public GameObject mesh;

    private BuildingHandle buildingHandle;
    private BlockController blockController;

    public void InitShapeAndAngle(BlockShape blockShape, BlockAngle blockAngle)
    {
        Shape = blockShape;
        Angle = blockAngle;
    }

    public void InitPositionRotation(Vector3 pos, Quaternion rotation)
    {
        positionDrop = pos;
        rotateDrop = rotation;
    }

    public void InitGameObjectAndPivot(GameObject cube, Transform pivot)
    {
        this.cube = cube;
        this.pivot = pivot;
    }

    public void SaveCubeType()
    {
        positionDrop = pivot.transform.position;
        rotateDrop = pivot.transform.rotation;
        if (PositionManager.Instance != null)
        {
            PositionManager.Instance.SaveCubeType(this);
        }
    }
    public void ConditionalRoof()
    {
        if (CheckRoof())
        {
            buildingHandle.TurnOffRoof();
        }
        else
        {
            buildingHandle.TurnOnRoof();
        }
    }
    public bool CheckRoof()
    {
        return blockController.CheckRoof();
    }

    public void AddMesh(GameObject mesh)
    {
        this.mesh = mesh;
        AddBuildingHandle(mesh.GetComponent<BuildingHandle>());
    }
    public void AddBuildingHandle(BuildingHandle buildingHandle)
    {
        this.buildingHandle = buildingHandle;
    }
    public void AddBlockController(BlockController blockController)
    {
        this.blockController = blockController;
    }
}
