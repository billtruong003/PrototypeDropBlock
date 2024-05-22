using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastDetect : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material touchColor; // Màu khi chạm
    [SerializeField] private Material normalColor; // Màu bình thường
    [SerializeField] private Material blockNormCol;
    private GameObject lastHitObject; // Đối tượng cuối cùng mà raycast đã chạm vào
    private Vector3 hitPosition;
    private Ray ray;
    private CubeController cubeController;
    void Start()
    {
        cubeController = gameObject.GetComponent<CubeController>();
    }

    void Update()
    {
        RayCastDown();
    }

    private void RayCastDown()
    {
        if (cubeController.doneDrop)
            return;

        ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20, groundLayer))
        {
            Debug.Log("Hit ground: " + hit.collider.gameObject.name);
            Debug.DrawLine(transform.position, hit.point, Color.green);

            GameObject hitObject = hit.collider.gameObject;
            Debug.Log(hitObject.tag);
            hitPosition = hitObject.transform.position; // Cập nhật vị trí chạm của raycast

            MeshRenderer meshRenderer = hitObject.GetComponent<MeshRenderer>();

            if (meshRenderer != null)
            {
                // Reset the material of the last hit object if it's different from the current hit object
                if (lastHitObject != null && lastHitObject != hitObject)
                {
                    MeshRenderer lastMeshRenderer = lastHitObject.GetComponent<MeshRenderer>();
                    if (lastMeshRenderer != null)
                    {
                        if (lastHitObject.CompareTag("Ground"))
                        {
                            lastMeshRenderer.material = normalColor;
                        }
                        else if (lastHitObject.CompareTag("Block"))
                        {
                            lastMeshRenderer.material = blockNormCol;
                        }
                    }
                }

                // Set the material of the current hit object to touchColor
                meshRenderer.material = touchColor;
                lastHitObject = hitObject;
            }
        }
        else
        {
            // Reset the material of the last hit object if no object is hit
            if (lastHitObject != null)
            {
                MeshRenderer lastMeshRenderer = lastHitObject.GetComponent<MeshRenderer>();
                if (lastMeshRenderer != null && lastHitObject.CompareTag("Ground"))
                {
                    if (lastHitObject.CompareTag("Ground"))
                    {
                        lastMeshRenderer.material = normalColor;
                    }
                    else if (lastHitObject.CompareTag("Block"))
                    {
                        lastMeshRenderer.material = blockNormCol;
                    }
                }
                lastHitObject = null;
            }
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
