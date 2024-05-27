using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SingleBlock : BlockBase, IBlock
{
    private float targetHeight;
    private Vector3 targetPosition;
    private GameObject detectedObject;
    private GameObject hitObject;
    public Transform GetPivot() => pivot;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (!DoneDrop)
        {
            HandleMovement();
            HandleRotate();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                DropToCenter();
            }
        }
    }

    public void DropToCenter()
    {
        DoneDrop = true;
        targetPosition = GetHighestPosition();
        hitObject = GetHitObject();

        // pivot.position = detectedObject.transform.position;
        SetAllCubeToParent();

        Vector3 targetPose = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        pivot.position = targetPose;

        Sequence mySequence = DOTween.Sequence();

        targetHeight = hitObject != null && hitObject.CompareTag("Block") ? targetPosition.y + 1 : targetPosition.y + 0.5f;
        mySequence.Append(pivot.DOMoveY(targetHeight, floatingSpeed).SetEase(Ease.InQuad));

        Debug.Log("Sequence setup completed");
        mySequence.OnComplete(OnDropComplete);
        mySequence.Play();
        Debug.Log("Sequence started");
    }

    private void OnDropComplete()
    {
        Debug.Log("OnDropComplete called");

        if (SpawnManager.Instance != null)
        {
            SpawnManager.Instance.SpawnCube();
            // SaveData(new Vector3(targetPosition.x, targetHeight, targetPosition.z), transform.rotation);
        }
        else
        {
            Debug.LogError("SpawnManager.Instance is null");
        }
    }

    private Vector3 GetHighestPosition()
    {
        float highestY = float.MinValue;
        Vector3 detectPoint = Vector3.zero;
        foreach (var cube in totalCube)
        {
            Transform DetectTransform = GroundDetect(cube.transform, Vector3.down);
            if (DetectTransform != null && DetectTransform.position.y > highestY)
            {
                highestY = DetectTransform.position.y;
                detectPoint = DetectTransform.position;
            }
        }
        return detectPoint;
    }
    private Vector3 GetObjectDetect()
    {
        float highestY = float.MinValue;
        Vector3 detectPoint = Vector3.zero;
        foreach (var cube in totalCube)
        {
            Transform DetectTransform = GroundDetect(cube.transform, Vector3.down);
            if (DetectTransform != null && DetectTransform.position.y > highestY)
            {
                highestY = DetectTransform.position.y;
                detectPoint = DetectTransform.position;
            }
        }
        return detectPoint;
    }
    private GameObject GetHitObject()
    {
        float highestY = float.MinValue;
        GameObject catchObject = null;
        foreach (var cube in totalCube)
        {
            Transform DetectTransform = GroundDetect(cube.transform, Vector3.down);
            if (DetectTransform != null && DetectTransform.position.y > highestY)
            {
                highestY = DetectTransform.position.y;
                catchObject = DetectTransform.gameObject;
            }
        }
        return catchObject;
    }



    public void SetAllCubeToParent()
    {
        foreach (var cube in totalCube)
        {
            cube.transform.SetParent(pivot, true);
        }
        centerPoint.transform.SetParent(pivot, true);

    }

    protected override void HandleMovement()
    {
        base.HandleMovement();
    }

    protected override void HandleRotate()
    {
        base.HandleRotate();
    }


}
