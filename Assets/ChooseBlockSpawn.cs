using System.Collections;
using System.Collections.Generic;
using BlockBuilder.BlockManagement;
using UnityEngine;

public class ChooseBlockSpawn : MonoBehaviour
{
    [SerializeField] private BlockShape blockShape;

    public void SpawnBlock()
    {
        if (SpawnManager.Instance != null)
        {
            if (SpawnManager.Instance.CurrentBlock == null || SpawnManager.Instance.CurrentBlock.DoneDrop == true)
            {
                if (SpawnManager.Instance.ReadySpawn)
                {
                    SpawnManager.Instance.SpawnBlock(blockShape);
                    SpawnManager.Instance.ReadySpawn = false;
                }
            }
        }
    }
}
