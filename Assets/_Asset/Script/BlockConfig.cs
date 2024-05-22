using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BlockConfig", menuName = "BlockCheck")]
public class BlockConfig : ScriptableObject
{
    [SerializeField] private List<BlockInfo> blockDatas;
    [SerializeField] private string path = "Block";

    [Button]
    private void GenerateBlockData()
    {
        int count = 0;
        foreach (BlockShape shape in Enum.GetValues(typeof(BlockShape)))
        {
            blockDatas[count].PathToBlock = $"{path}/{shape}";
        }
    }
}
