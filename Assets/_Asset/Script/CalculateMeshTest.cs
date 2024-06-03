using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using BillUtils.SpaceUtilities;

public class CalculateMeshTest : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [Button]
    private void CountMeshPose()
    {
        Vector3 meshPose = SpaceUtils.GetMeshWorldPosition(door);
        Debug.Log($"Calculated Mesh Position: {meshPose}");
    }
}
