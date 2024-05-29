using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using AnimationController.WithTransform;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using BillUtils.SerializeCustom;

public abstract class BlockBase : MonoBehaviour
{
    // Movement settings
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float floatingSpeed = 2f;
    [SerializeField] protected bool canRotate;

    // Transforms
    [Header("Transforms")]
    [SerializeField] protected Transform pivot;
    [SerializeField] protected Transform centerPoint;

    // Raycast and Cube data
    [Header("Raycast and Cube Data")]
    [SerializeField] protected LayerMask GroundLayer;
    [SerializeField] protected List<GameObject> totalCube = new();

    // Block data
    [CustomHeader("Data", 15, "#B0EBB4")]
    [BoxGroup("CubeData")]
    [SerializeField] protected BlockShape blockShape;
    [BoxGroup("CubeData")]
    [SerializeField] protected BlockAngle blockAngle;


    protected bool DoneDrop { get; set; } = false;

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        if (!DoneDrop)
        {
            HandleMovement();
            HandleRotate();
        }
    }
    protected virtual void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        transform.Translate(new Vector3(moveX, 0, moveZ), Space.World);
    }

    protected virtual void HandleRotate()
    {
        if (!canRotate)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(Vector3.up, 90, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -90, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(Vector3.right, 90, Space.World);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            transform.Rotate(Vector3.right, -90, Space.World);
        }
    }
    protected virtual bool BlockAroundDetect(Transform origin, Vector3 dir, float length = 1f)
    {
        Debug.DrawRay(origin.position, dir * length, Color.red, length);
        if (Physics.Raycast(origin.position, dir, out RaycastHit hit, length, GroundLayer))
        {

            return true;
        }
        return false;
    }
    protected virtual Transform GroundDetect(Transform origin, Vector3 dir, float length = 20f)
    {
        Debug.DrawRay(origin.position, dir * length, Color.blue, length);
        if (Physics.Raycast(origin.position, dir, out RaycastHit hit, length, GroundLayer))
        {
            return hit.collider.transform;
        }
        return null;
    }
}
