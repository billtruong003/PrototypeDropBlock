using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BlockBuilder.BlockManagement;
using System.Runtime.CompilerServices;

public class RayCastDetect : MonoBehaviour
{
    // Layer masks and materials
    [Header("Layer Masks and Materials")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material touchColor;
    [SerializeField] private Material normalColor;
    [SerializeField] private Material blockNormCol;
    [SerializeField] private MeshRenderer lastMeshRenderer;

    // Detection settings
    [Header("Detection Settings")]
    [SerializeField] private AngleShoot angleShoot;
    [SerializeField] private LayerMask blockLayer = 8;
    [SerializeField] private float detectionRadius = 3f;

    // Private variables
    private GameObject lastHitObject;
    private Vector3 hitPosition;
    private Ray ray;
    private BlockController cubeController;

    private void Start()
    {
        cubeController = gameObject.GetComponent<BlockController>() ?? transform.parent.GetComponent<BlockController>();
        blockLayer = 8;
    }

    private void Update()
    {
        RayCastAngle();
    }

    private void RayCastAngle()
    {
        if (cubeController.DoneDrop)
            return;

        switch (angleShoot)
        {
            case AngleShoot.DOWN:
                CastRay(Vector3.down);
                break;
            case AngleShoot.SIDE_FORWARD:
                CastRay(Vector3.forward);
                break;
            case AngleShoot.SIDE_BACK:
                CastRay(Vector3.back);
                break;
            case AngleShoot.SIDE_RIGHT:
                CastRay(Vector3.right);
                break;
            case AngleShoot.SIDE_LEFT:
                CastRay(Vector3.left);
                break;
            case AngleShoot.TOP:
                CastRay(Vector3.up);
                break;
        }
    }

    private void CastRay(Vector3 direction)
    {
        ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 20, groundLayer))
        {
            HandleRaycastHit(hit);
        }
        else
        {
            ResetLastHitObject();
        }
    }

    private void HandleRaycastHit(RaycastHit hit)
    {
        Debug.Log("Hit ground: " + hit.collider.gameObject.name);
        Debug.DrawLine(transform.position, hit.point, Color.green);

        GameObject hitObject = hit.collider.gameObject;
        hitPosition = hitObject.transform.position;

        MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            if (lastHitObject != null && lastHitObject != hitObject)
            {
                lastMeshRenderer = lastHitObject.GetComponent<MeshRenderer>();
                if (lastMeshRenderer != null)
                {
                    SetBackBlockMat();
                }
            }

            meshRenderer.material = touchColor;
            lastHitObject = hitObject;
        }
    }

    private void ResetLastHitObject()
    {
        if (lastHitObject != null)
        {
            lastMeshRenderer = lastHitObject.GetComponent<MeshRenderer>();
            if (lastMeshRenderer != null)
            {
                SetBackBlockMat();
            }
            lastHitObject = null;
        }
    }

    public void SetDetectBlock(MeshRenderer mesh)
    {
        mesh.material = touchColor;
    }

    public void SetBackBlockMat()
    {
        if (lastMeshRenderer.gameObject.CompareTag("Ground"))
        {
            lastMeshRenderer.material = normalColor;
        }
        else
        {
            lastMeshRenderer.material = blockNormCol;
        }
    }

    public GameObject GetHitObject()
    {
        return lastHitObject;
    }

    public Vector3 GetHitPosition()
    {
        return hitPosition;
    }

    public float GetYHitPosition()
    {
        return hitPosition.y;
    }

    public bool CheckBlockOnTop()
    {
        bool roofOnTop = CheckBlock(transform.position, Vector3.up);
        Debug.Log($"GameObject {gameObject.name} roofOnTop {roofOnTop}");
        return roofOnTop;
    }

    private bool CheckBlock(Vector3 origin, Vector3 direction)
    {
        if (Physics.Raycast(origin, direction, out RaycastHit hit, detectionRadius, blockLayer))
        {
            Debug.Log($"Block detected in direction {direction} from position {origin}.");
            Debug.Log($"Detection Hit: {hit.collider.gameObject}");
            return true;
        }
        Debug.Log($"No block detected in direction {direction} from position {origin}.");
        return false;
    }
}
