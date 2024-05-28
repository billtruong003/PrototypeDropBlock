using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BlockBuilder.BlockManagement;

public class RayCastDetect : MonoBehaviour
{
    // Layer masks and materials
    [HideInInspector] public bool valid;

    [Header("Layer Masks and Materials")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material touchColor;
    [SerializeField] private Material normalColor;
    [SerializeField] private Material blockNormCol;
    [SerializeField] private MeshRenderer lastMeshRenderer;

    // Detection settings
    [Header("Detection Settings")]
    [SerializeField] private AngleShoot angleShoot;
    [SerializeField] private LayerMask blockLayer;
    [SerializeField] private float detectionRadius = 3f;

    [Header("Block Case")]
    [SerializeField] private List<Transform> cube;
    [SerializeField] private BlockShape blockShape;

    // Private variables
    private bool completeChangeMat;
    private GameObject lastHitObject;
    private Vector3 hitPosition;
    private Ray ray;
    private BlockController blockController;

    private void Start()
    {
        blockController = gameObject.GetComponent<BlockController>() ?? transform.parent.GetComponent<BlockController>();
        if (blockController == null)
        {
            Debug.LogError("BlockController not found!");
            enabled = false; // Disable this script if BlockController is not found
        }
    }

    private void Update()
    {
        if (blockController != null)
        {
            if (blockController.DoneDrop)
            {
                ResetLastHitObject();
            }
            else
            {
                ProcessRay();
            }
        }
    }

    public void ProcessRay()
    {
        Vector3[] directions;

        switch (angleShoot)
        {
            case AngleShoot.DOWN:
                directions = new Vector3[] { Vector3.down };
                break;
            case AngleShoot.SIDE_FORWARD:
                directions = new Vector3[] { Vector3.forward };
                break;
            case AngleShoot.SIDE_BACK:
                directions = new Vector3[] { Vector3.back };
                break;
            case AngleShoot.SIDE_RIGHT:
                directions = new Vector3[] { Vector3.right };
                break;
            case AngleShoot.SIDE_LEFT:
                directions = new Vector3[] { Vector3.left };
                break;
            case AngleShoot.SIDE:
                directions = new Vector3[] { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };
                break;
            case AngleShoot.TOP:
                directions = new Vector3[] { Vector3.up };
                break;
            default:
                directions = new Vector3[0];
                break;
        }

        foreach (var direction in directions)
        {
            CastRay(direction);
        }
    }

    private void CastRay(Vector3 direction)
    {
        ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 20, groundLayer))
        {
            if (hit.collider.gameObject.CompareTag("Block"))
            {
                BlockController blockController = FindBlockControllerInAncestors(hit.collider.gameObject);
                if (blockController != null)
                {
                    Debug.Log($"Done Drop: {blockController.DoneDrop}");
                    if (blockController.DoneDrop)
                    {
                        valid = true;
                        blockController.TransparentRoof();
                        HandleRaycastHit(hit);
                        return;
                    }
                    else
                    {
                        valid = false;
                        return;
                    }
                }
            }
            valid = true;
            HandleRaycastHit(hit);
        }
        else
        {
            ResetLastHitObject();
        }
    }

    private BlockController FindBlockControllerInAncestors(GameObject obj)
    {
        Transform currentTransform = obj.transform;

        while (currentTransform != null)
        {
            BlockController blockController = currentTransform.GetComponent<BlockController>();
            if (blockController != null)
            {
                return blockController;
            }
            currentTransform = currentTransform.parent;
        }

        return null;
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
                ResetLastHitObject();
            }

            meshRenderer.material = touchColor;
            lastHitObject = hitObject;
            lastMeshRenderer = meshRenderer;
        }
    }

    private void ResetLastHitObject()
    {
        if (lastHitObject != null)
        {
            if (lastMeshRenderer != null)
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
            lastHitObject = null;
            lastMeshRenderer = null;
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
