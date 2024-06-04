using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using BillUtils.SpaceUtils;

public class CalculateMeshTest : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [Button]
    private void CountMeshPose()
    {
        Vector3 meshPose = SpaceUtilities.GetMeshWorldPosition(door);
        Debug.Log($"Calculated Mesh Position: {meshPose}");
    }
}
