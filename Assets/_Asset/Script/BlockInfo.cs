using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class BlockInfo
{
    public string PathToBlock;
    public BlockShape Shape;

}
[Serializable]
public enum BlockShape
{
    SINGLE,
    ISHAPE,
    LSHAPE,
    _SHAPE,
}
