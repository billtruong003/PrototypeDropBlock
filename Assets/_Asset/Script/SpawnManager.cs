using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform cubeContainer;
    [SerializeField] private List<GameObject> dropBrick;
    [SerializeField] private GameObject brick;
    [SerializeField, Expandable] private BlockConfig blockConfig;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitDropBrick();
        SpawnCube();
    }

    private void Update()
    {
        // Intentionally left blank
    }

    public void SpawnCube()
    {
        if (dropBrick == null || dropBrick.Count == 0)
        {
            Debug.LogError("DropBrick list is empty. Cannot spawn cube.");
            return;
        }

        GameObject selectedBrick = dropBrick[Random.Range(0, dropBrick.Count)];
        Instantiate(selectedBrick, Vector3.up * 10, Quaternion.identity, cubeContainer);
    }

    private void InitDropBrick()
    {
        if (blockConfig == null)
        {
            Debug.LogError("BlockConfig is null. Cannot initialize drop bricks.");
            return;
        }

        dropBrick = blockConfig.GetBlockPrefab();
    }
}
