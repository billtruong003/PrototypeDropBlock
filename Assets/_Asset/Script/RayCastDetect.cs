using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BlockBuilder.BlockManagement;

public class RayCastDetect : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material touchColor; // Màu khi chạm
    [SerializeField] private Material normalColor; // Màu bình thường
    [SerializeField] private Material blockNormCol;
    [SerializeField] private MeshRenderer lastMeshRenderer;
    [SerializeField] private AngleShoot angleShoot;

    private GameObject lastHitObject; // Đối tượng cuối cùng mà raycast đã chạm vào
    private Vector3 hitPosition;
    private Ray ray;
    private BlockController cubeController;

    void Start()
    {
        cubeController = gameObject.GetComponent<BlockController>();
        if (cubeController == null)
        {
            cubeController = transform.parent.GetComponent<BlockController>();
        }
    }

    void Update()
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
            case AngleShoot.SIDE:
                CastRay(Vector3.forward);
                CastRay(Vector3.back);
                CastRay(Vector3.left);
                CastRay(Vector3.right);
                break;
            case AngleShoot.TOP:
                CastRay(Vector3.up);
                break;
        }
    }

    private void CastRay(Vector3 direction)
    {
        ray = new Ray(transform.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20, groundLayer))
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
        else
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
}
