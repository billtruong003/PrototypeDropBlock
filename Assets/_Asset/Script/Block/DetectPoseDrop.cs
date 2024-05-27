using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BlockBuilder.BlockManagement;
using System.Runtime.CompilerServices;

public class DetectPoseDrop : MonoBehaviour
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
    public SingleBlock DoneDrop;
    private void Start()
    {
    }

    private void Update()
    {
        // if (!DoneDrop)
        CastRay(Vector3.down);
    }
    private void CastRay(Vector3 direction)
    {
        ray = new Ray(transform.parent.position, direction);
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

}