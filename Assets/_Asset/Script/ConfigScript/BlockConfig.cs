using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NaughtyAttributes;
using UnityEngine;
using BlockBuilder.BlockManagement;
[CreateAssetMenu(fileName = "BlockConfig", menuName = "BlockBuilder/BlockCheck")]
public class BlockConfig : ScriptableObject
{
    [CustomHeader("Normal Block Data", 15, "#F6DCAC")]
    [SerializeField] private List<BlockInfo> blockDatas;
    private string pathBlock = "Block";



    [Button]
    private void GenerateBlockData()
    {
        blockDatas.Clear();
        foreach (BlockShape shape in Enum.GetValues(typeof(BlockShape)))
        {
            BlockInfo newData = new BlockInfo
            {
                PathToBlock = $"{pathBlock}/{shape.ToString()}",
                Shape = shape
            };
            blockDatas.Add(newData);
        }
    }

    [Button]
    private void ClearBlockData()
    {
        blockDatas.Clear();
    }

    public List<GameObject> GetBlockPrefab()
    {
        List<GameObject> blocks = new();
        foreach (var item in blockDatas)
        {
            GameObject block = item.GetBlock();
            blocks.Add(block);
        }
        return blocks;
    }
}
