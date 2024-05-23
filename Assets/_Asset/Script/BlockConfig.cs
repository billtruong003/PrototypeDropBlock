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
        blockDatas.Clear(); // Clear the existing data before generating new data
        foreach (BlockShape shape in Enum.GetValues(typeof(BlockShape)))
        {
            BlockInfo newData = new BlockInfo
            {
                PathToBlock = $"{path}/{shape.ToString()}",
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
