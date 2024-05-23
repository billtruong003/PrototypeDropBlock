using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;



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
public enum BlockShape
{
    SINGLE,
    DOUBLE,
    BIGPLANE,
    ISHAPE,
}
