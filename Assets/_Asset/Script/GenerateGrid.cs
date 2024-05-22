using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public class GenerateGrid : MonoBehaviour
{
    [SerializeField] private GameObject plane;
    [SerializeField] private Transform gridContainer;
    [SerializeField] private float SPACE = 1f;
    float zPos;
    float xPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button]
    private void Generate()
    {
        for (int i = 0; i < 10; i++)
        {
            zPos = (5 - ((i + 0.5f) * SPACE));
            for (int j = 0; j < 10; j++)
            {
                xPos = (5 - ((j + 0.5f) * SPACE));
                GameObject planeSpawn = Instantiate(this.plane, gridContainer);
                planeSpawn.transform.SetLocalPositionAndRotation(new Vector3(xPos, 0, zPos), Quaternion.identity);
            }
        }
    }

    [Button]
    private void DestroyAllPlanes()
    {
        for (int i = gridContainer.childCount - 1; i >= 0; i--)
        {
            Transform child = gridContainer.GetChild(i);
            if (child.gameObject != null)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
