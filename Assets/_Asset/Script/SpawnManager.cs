using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform cubeContainer;
    [SerializeField] private List<GameObject> dropBrick;
    [SerializeField] private GameObject brick;

    [SerializeField]
    [Expandable] private BlockConfig blockConfig;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        InitDropBrick();
        SpawnCube();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnCube()
    {
        GameObject brick = dropBrick[Random.Range(0, dropBrick.Count)];
        Instantiate(brick, Vector3.up * 10, Quaternion.identity, cubeContainer);
    }

    public void InitDropBrick()
    {
        dropBrick = blockConfig.GetBlockPrefab();
    }

}
