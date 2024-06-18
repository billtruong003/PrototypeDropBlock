using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using BillUtils.SerializeCustom;
using BlockBuilder.BlockManagement;
using BillUtils.GameObjectUtilities;
public class SpawnManager : Singleton<SpawnManager>
{
    [Header("Object Pooling")]
    public BlockController CurrentBlock;
    public List<PoolBlock> poolBlocks;
    public List<PoolMesh> poolMeshs;


    [Header("Container")]
    [SerializeField] private Transform cubeContainer;
    [SerializeField] private List<GameObject> dropBrick;
    [SerializeField] private GameObject brick;
    [SerializeField, Expandable] private BlockConfig blockConfig;

    [BillHeader("Cheat", 20, "#EE4E4E")]

    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private bool spawnCheat;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private GameObject cheatBlock;

    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private bool materialCheat;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private MaterialType cheatMaterial;

    private GameObject selectedBrick;

    public bool CheckMatCheat() => materialCheat;
    public MaterialType GetCheatMat() => cheatMaterial;

    public void SetModeCheatBlock(bool value) => spawnCheat = value;
    public void SetCheatBlock(GameObject blockType) => cheatBlock = blockType;
    public void SetModeCheatMaterial(bool value) => materialCheat = value;
    public void SetCheatMaterial(MaterialType matType) => cheatMaterial = matType;
    public bool GetStatusCheatBlock() => spawnCheat;
    public bool GetStatusCheatMaterial() => materialCheat;
    public GameObject GetDropBlock(int value) => dropBrick[value];

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        InitDropBrick();
        SpawnCube();
    }


    public void SpawnCube()
    {
        if (dropBrick == null || dropBrick.Count == 0)
        {
            Debug.LogError("DropBrick list is empty. Cannot spawn cube.");
            return;
        }

        if (spawnCheat)
        {
            selectedBrick = cheatBlock;
        }
        else
        {
            selectedBrick = dropBrick[Random.Range(0, dropBrick.Count)];
        }
        selectedBrick = Instantiate(selectedBrick, Vector3.up * 10, Quaternion.identity, cubeContainer);
        CurrentBlock = selectedBrick.GetComponent<BlockController>();
        UpdateUIInfo();
    }

    private void UpdateUIInfo()
    {
        UIManager.Instance.SetCurrentBlock(selectedBrick.GetComponent<BlockController>().getShape);
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

    public void AddPoolBlock(BlockController blockController)
    {
        PoolBlock poolBlock = new PoolBlock(blockController);
        poolBlocks.Add(poolBlock);
    }

    public void AddPoolMesh(CubeData cubeData)
    {
        PoolMesh poolMesh = new PoolMesh(cubeData);
        poolMeshs.Add(poolMesh);
    }
}
