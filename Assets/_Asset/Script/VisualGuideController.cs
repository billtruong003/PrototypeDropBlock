using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualGuideController : MonoBehaviour
{
    public void Display(Vector3 targetPosition)
    {
        targetPosition = new Vector3(targetPosition.x, targetPosition.y + 0.05f, targetPosition.z);
        transform.position = targetPosition;
    }
}
