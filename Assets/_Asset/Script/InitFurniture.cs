using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class InitFurniture : MonoBehaviour
{
    [SerializeField] private GameObject sample;
    List<string> side = new() { "Forward", "Left", "Backward", "Right" };
    private int Angle = 0;
    [Button]
    private void GenFourSide()
    {
        Angle = 0;
        for (int i = 0; i < 4; i++)
        {
            GameObject sideFur = Instantiate(sample, transform);
            sideFur.transform.eulerAngles = new Vector3(0, Angle, 0);
            sideFur.name = side[i];
            Angle += 90;
        }
    }
}
