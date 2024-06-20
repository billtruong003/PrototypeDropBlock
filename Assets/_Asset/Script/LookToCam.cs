using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using AnimationController.WithTransform;

public class LookTocam : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask uILayer;
    [SerializeField] private Transform positionToStay;
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // UIClickHandle();
    }

    // private void UIClickHandle()
    // {
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;

    //         if (Physics.Raycast(ray, out hit, Mathf.Infinity, UILayer))
    //         {
    //             if (hit.collider == null)
    //             {
    //                 Debug.LogError("hit.collider is null");
    //                 return;
    //             }

    //             Button btn = hit.collider.GetComponent<Button>();
    //             if (btn == null)
    //             {
    //                 Debug.LogError("Button component is not found");
    //                 return;
    //             }

    //             Debug.Log($"Clicked on UI {btn.gameObject.name}");
    //             Anim.PressButton(btn);
    //             btn.onClick.Invoke();
    //         }
    //     }
    // }



    void LateUpdate()
    {
        if (!mainCamera) return;
        transform.position = positionToStay.position;
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        Quaternion rotationToCamera = Quaternion.LookRotation(directionToCamera);

        transform.rotation = rotationToCamera;
    }
}