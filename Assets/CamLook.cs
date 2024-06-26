using BillUtils.SerializeCustom;
using UnityEngine;
using BillUtils.HexUtils;

public class CamLook : MonoBehaviour
{
    public float mouseSensitivity = 300;
    public Transform playerBody;
    private float xRotation = 0f;

    [BillHeader("RayCast", 15, HexColors.DarkGoldenrod)]
    public float rayCastDistance = 100f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        ShootRaycast();
    }

    void ShootRaycast()
    {
        Vector3 rayOrigin = playerBody.position + playerBody.forward * transform.localPosition.z;
        if (Physics.Raycast(rayOrigin, playerBody.forward, out RaycastHit hit, rayCastDistance))
        {
            Debug.Log("Hit: " + hit.collider.name);
            // Do something with the hit object
        }
    }
}
