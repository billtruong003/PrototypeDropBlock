using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private GameObject funiture;
    [SerializeField] private GameObject roof;
    [SerializeField] private Transform pivot;

    public void TurnOnRoof()
    {
        roof.SetActive(true);
    }
    public void TurnOnFurniture()
    {
        funiture.SetActive(true);
    }
}
