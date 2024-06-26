using BillUtils.SerializeCustom;
using UnityEngine;
using BillUtils.HexUtils;
using Unity.VisualScripting;

public class CamLook : MonoBehaviour
{
    public float mouseSensitivity = 300;
    public Transform playerBody;
    private float xRotation = 0f;

    [BillHeader("RayCast", 15, HexColors.DarkGoldenrod)]
    public float rayCastDistance = 100f;
    public ChooseBlockSpawn chooseBlockSpawn;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        ShootRaycast();

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

        if (Input.GetMouseButtonDown(0))
        {
            HandleSpawnBlock();
        }
    }

    private void HandleSpawnBlock()
    {
        if (chooseBlockSpawn == null)
            return;

        chooseBlockSpawn.SpawnBlock();
    }

    void ShootRaycast()
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayCastDistance))
        {
            if (hit.collider != null)
            {
                ChooseBlockSpawn hitChooseBlockSpawn = hit.collider.gameObject.GetComponent<ChooseBlockSpawn>();
                if (hitChooseBlockSpawn != null)
                {
                    chooseBlockSpawn = hitChooseBlockSpawn;
                    Debug.Log("Hit: " + hit.collider.name);
                }
                else
                {
                    chooseBlockSpawn = null;
                }
            }
            else
            {
                chooseBlockSpawn = null;
            }
        }
        else
        {
            chooseBlockSpawn = null;
        }
    }
}
