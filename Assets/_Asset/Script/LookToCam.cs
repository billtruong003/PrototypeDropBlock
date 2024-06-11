using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using AnimationController.WithTransform;

public class LookTocam : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask UILayer;
    void Start()
    {
        // Tìm camera chính
        mainCamera = Camera.main;
    }
    void Update()
    {
        UIClickHandle();
    }
    private void UIClickHandle()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Tạo một ray từ vị trí chuột
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Kiểm tra xem ray có đâm trúng bất kỳ collider nào không
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, UILayer))
            {
                Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red, 2.0f);
                // Kiểm tra nếu đối tượng trúng là UI
                if (((1 << hit.collider.gameObject.layer) & UILayer) != 0)
                {

                    Button btn = hit.collider.GetComponent<Button>();
                    Debug.Log($"Clicked on UI {btn.gameObject.name}");
                    if (btn == null)
                        return;
                    Anim.PressButton(btn);
                    btn.onClick.Invoke();
                }
            }
        }
    }


    void LateUpdate()
    {
        if (!mainCamera) return;

        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);

        transform.rotation = rotationToCamera;
    }
}