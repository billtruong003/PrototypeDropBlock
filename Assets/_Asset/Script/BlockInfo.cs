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
    public class BlockGenInfo
    {
        public string PathToBuilding;
        public BuildingType Type;
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
    public class MaterialApplyInfo
    {
        public string PathToMaterial;
        public MaterialType MatType;
        public MaterialMixType MatMixType;

        public Material GetBuilding()
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