using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class CubeController : MonoBehaviour
{
    public bool doneDrop = false;
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<RayCastDetect> rayCastDetect;
    [SerializeField] private float floatingSpeed;
    private bool isDropping = false;
    private Vector3 targetPosition;
    private GameObject hitObject;

    void Start()
    {
        rayCastDetect = GetAllComponents(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        BlockMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropToCenter();
        }
    }

    private void BlockMovement()
    {
        if (doneDrop)
            return;

        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, 0, moveZ));
    }
    private void DropToCenter()
    {
        if (doneDrop)
            return;
        doneDrop = true;
        targetPosition = GetHighestPose();
        hitObject = GetHitObject();


        Vector3 targetPose = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = targetPose;
        Sequence mySequence = DOTween.Sequence();

        // mySequence.Append(transform.DOMove(targetPose, floatingSpeed).SetEase(Ease.Flash));
        if (hitObject.CompareTag("Block"))
        {
            mySequence.Append(transform.DOMoveY(targetPosition.y + 1, floatingSpeed).SetEase(Ease.InQuad));
        }
        else
        {
            mySequence.Append(transform.DOMoveY(targetPosition.y + 0.5f, floatingSpeed).SetEase(Ease.InQuad));
        }
        mySequence.OnComplete(() =>
        {
            doneDrop = true;

            if (SpawnManager.Instance != null)
            {
                SpawnManager.Instance.SpawnCube();
            }
        });
    }
    private List<RayCastDetect> GetAllComponents(GameObject gameObject)
    {
        List<RayCastDetect> allComponents = new List<RayCastDetect>();

        gameObject.GetComponentsInChildren<RayCastDetect>(true, allComponents);

        return allComponents;
    }
    private Vector3 GetHighestPose()
    {
        float y = -1;
        Vector3 highestRay = Vector3.zero;
        foreach (var item in rayCastDetect)
        {
            if (item.GetYHitPosition() > y)
            {
                highestRay = item.GetHitPosition();
            }
        }
        return highestRay;
    }
    private GameObject GetHitObject()
    {
        float y = -1;
        GameObject hitObject = null;
        foreach (var item in rayCastDetect)
        {
            if (item.GetYHitPosition() > y)
            {
                hitObject = item.GetHitObject();
            }
        }
        return hitObject;
    }


}

