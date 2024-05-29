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
    #endregion

    #region Orientation
    public enum Direction
    {
        TOP,
        DOWN,
        LEFT,
        RIGHT,
        ABOVE_DOWN,
        ABOVE_UP
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
        METAL,
        RARE_METAL,
        PEBBLE_BRICK,
        BRICK,
        ROCK,
        STONE_BRICK,
        CLAY,
        WOOD,
        GLASS
    }
    #endregion
}
