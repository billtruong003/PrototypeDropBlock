using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UIElements;


namespace BlockBuilder.BlockManagement
{
    [Serializable]
    public class BlockInfo
    {
        public string PathToBlock;
        public BlockShape Shape;

        public GameObject GetBlock()
        {
            return Resources.Load<GameObject>(PathToBlock);
        }
    }
    [Serializable]
    public class BlockMeshInfo
    {
        public string PathToBuilding;
        public BuildingType Type;
        public MaterialType matType;
        public GameObject GetBuilding()
        {
            if (string.IsNullOrEmpty(PathToBuilding))
            {
                Debug.LogError("PathToBuilding is null or empty!");
                return null;
            }

            GameObject building = Resources.Load<GameObject>(PathToBuilding);
            if (building == null)
            {
                Debug.LogError($"Failed to load building at path: {PathToBuilding}");
            }

            return building;
        }

    }

    [Serializable]
    public class PathBlock
    {
        public MaterialType matType;
        public string PathToBuilding;
    }

    [Serializable]
    public class MaterialApplyInfo
    {
        public string PathToMaterial;
        public MaterialType MatType;
        public Material MainMat;
        public Material EmissionLight;
        public Material SpecialMat;
        public Material Transparent_Mat;

        public Material GetMat()
        {
            return Resources.Load<Material>(PathToMaterial);
        }
    }
    [Serializable]
    public class CombineBuildingAndMaterial
    {
        public BlockShape shape;
        public BlockAngle angle;
        public BuildingType returnBuilding;
        public BuildingType GetBuilding()
        {
            return returnBuilding;
        }
    }
}