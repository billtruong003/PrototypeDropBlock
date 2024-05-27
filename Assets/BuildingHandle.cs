using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private GameObject funiture;
    [SerializeField] private GameObject roof;
    [SerializeField] private List<Transform> pivots;


    public void TurnOnRoof()
    {
        roof.SetActive(true);
    }
    public void TurnOffRoof()
    {
        roof.SetActive(false);
    }
    public void TurnOnFurniture()
    {
        funiture.SetActive(true);
    }
    public void TurnOffFurniture()
    {
        funiture.SetActive(false);
    }
}
