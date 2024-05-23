using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : Singleton<PositionManager>
{

    [SerializeField] private List<Vector3> PositionSave;
    private ProcessMesh processMesh;
    protected override void Awake()
    {
        base.Awake();
        processMesh = gameObject.GetComponent<ProcessMesh>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PositionSave.Clear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SavePosition(Vector3 pos)
    {
        PositionSave.Add(pos);
    }
    // public void 
}
