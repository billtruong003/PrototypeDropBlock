using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private Transform cubeContainer;
    [SerializeField] private GameObject cubeDrop;

    protected override void Awake()
    {
        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnCube();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SpawnCube()
    {
        Instantiate(cubeDrop, Vector3.up * 10, Quaternion.identity, cubeContainer);
    }

}
