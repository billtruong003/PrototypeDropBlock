using System;
using BlockBuilder.BlockManagement;
using UnityEngine;

[Serializable]
public class CubeData
{
    // Public fields
    public BlockShape Shape { get; private set; }
    public BlockAngle Angle { get; private set; }
    public MaterialType MaterialType { get; set; }
    public Vector3 PositionDrop { get; private set; }
    public Vector3 RotateDrop { get; private set; }
    public GameObject Cube { get; private set; }
    public Transform Pivot { get; private set; }
    public Transform CenterPoint { get; set; }
    public GameObject Mesh { get; private set; }

    // Private fields
    private BuildingHandle _buildingHandle;
    private BlockController _blockController;


    // Set Properties
    public void SetShape(BlockShape shape) => Shape = shape;
    public void SetAngle(BlockAngle angle) => Angle = angle;
    public void SetMaterialType(MaterialType materialType) => MaterialType = materialType;
    public void SetPositionDrop(Vector3 positionDrop) => PositionDrop = positionDrop;
    public void SetRotateDrop(Vector3 rotateDrop) => RotateDrop = rotateDrop;
    public void SetCube(GameObject cube) => Cube = cube;
    public void SetPivot(Transform pivot) => Pivot = pivot;
    public void SetCenterPoint(Transform centerPoint) => CenterPoint = centerPoint;
    public void SetMesh(GameObject mesh) => Mesh = mesh;
    public void SetBuildingHandle(BuildingHandle buildingHandle) => _buildingHandle = buildingHandle;
    public void SetBlockController(BlockController blockController) => _blockController = blockController;

    public void ConditionalRoof()
    {
        if (CheckRoof())
        {
            _buildingHandle.TurnOffRoof();
        }
        else
        {
            _buildingHandle.TurnOnRoof();
        }
    }

    public bool CheckRoof()
    {
        return _blockController.CheckRoof();
    }

    public void AssignBuildingHandleToBlockController()
    {
        _blockController.BuildingHandle = this._buildingHandle;
    }
    public void AssignMaterialToBuildingHandle(Material mainMat, Material EmissionLight)
    {
        _buildingHandle.AddMat(mainMat);
        _buildingHandle.AddMat(EmissionLight);
        _buildingHandle.ChangeMaterials();
    }
}
