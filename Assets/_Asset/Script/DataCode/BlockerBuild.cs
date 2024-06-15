using System;
using UnityEngine;
namespace BlockBuilder.BlockManagement
{
    #region Block Configuration
    public enum BlockShape
    {
        SINGLE,
        DOUBLE,
        BIGPLANE,
        ISHAPE,
    }


    public enum BlockAngle
    {
        FLAT,
        STAND,
    }

    [Serializable]
    public class BlockMaterial
    {
        public MaterialType materialType;
        public Material material;

        public Material GetMat(MaterialType materialType)
        {
            if (this.materialType == materialType)
            {
                return material;
            }
            return null;
        }
    }

    #endregion

    #region Building Configuration
    public enum BuildingType
    {
        BD_001,
        BD_002,
        BD_003,
        BD_004,
        BD_005,
        BD_006,
        BD_007,
    }

    public enum BuildingStyleType
    {
        BDS_001,
        BDS_002,
        BDS_003,
        BDS_004,
    }

    public enum BuildingAesthetic
    {
        FIRST_FLOOR,
        SECOND_FLOOR,
        TERRACE,
        ROOFTOP,
    }
    #endregion

    #region Orientation
    public enum Direction
    {
        FORWARD,
        BACKWARD,
        LEFT,
        RIGHT,
    }

    public enum AngleShoot
    {
        DOWN,
        SIDE_LEFT,
        SIDE_RIGHT,
        SIDE_FORWARD,
        SIDE_BACK,
        SIDE,
        TOP,
    }
    #endregion

    #region Material Configuration
    public enum MaterialMixType
    {
        MATMIX_001,
        MATMIX_002,
        MATMIX_003,
    }

    public enum MaterialType
    {
        // METAL,
        // RARE_METAL,
        // PEBBLE_BRICK,
        BRICK,
        // ROCK,
        STONE,
        // STONE_BRICK,
        CLAY,
        WOOD,
        // GLASS
    }
    #endregion
}
