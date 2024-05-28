using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

public class ToolGenPrefab : MonoBehaviour
{
    [SerializeField]
    private string pathLoad = "Building/";
    [SerializeField] private string prefix = "SM_Clay_";
    [SerializeField] private string body = "Body_";
    [SerializeField] private string door = "Door_";
    [SerializeField] private string window = "Window_";
    [SerializeField] private string roof = "Roof_";
    [SerializeField] private int count = 7;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject instantiatePrefab;

    [Button]
    private void GenObject()
    {
        ClearAllObjectsInContainer();

        for (int i = 0; i < count; i++)
        {
            GameObject mainPrefab = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, container);
            mainPrefab.name = $"BD_0{ProcessCodeMesh(i + 1)}";
            GameObject containAllSmall = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, mainPrefab.transform);
            mainPrefab.name = $"Container";
            CreateChildObject(containAllSmall.transform, "Brick", body, i + 1);
            GameObject subPrefab = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, mainPrefab.transform);
            mainPrefab.name = $"BD_{ProcessCodeMesh(i + 1)}";
            CreateChildObject(subPrefab.transform, "Door", door, i + 1);
            CreateChildObject(subPrefab.transform, "Window", window, i + 1);
            CreateChildObject(containAllSmall.transform, "Roof", roof, i + 1);
        }
    }

    [Button]
    private void ClearAllObjectsInContainer()
    {
        if (container == null)
        {
            Debug.LogError("Container is not set.");
            return;
        }

        for (int i = container.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(container.GetChild(i).gameObject);
        }
    }

    private void CreateChildObject(Transform parent, string childName, string objectType, int index)
    {
        GameObject child = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, parent);
        child.name = childName;
        Vector3 spawnPose = childName == "Roof" ? Vector3.up * 1 : Vector3.zero;
        GameObject childPrefab = Instantiate(GetPrefab(pathLoad, prefix, objectType, index), Vector3.zero, quaternion.identity, child.transform);
    }

    private GameObject GetPrefab(string path, string prefix, string objectType, int index)
    {
        string fullPath = path + prefix + objectType + ProcessCodeMesh(index);
        Debug.Log($"Loading prefab from path: {fullPath}"); // Debug log for the full path
        GameObject prefabLoad = Resources.Load<GameObject>(fullPath);

        if (prefabLoad == null)
        {
            Debug.LogError($"Prefab not found at path: {fullPath}");
        }
        else
        {
            Debug.Log($"Prefab loaded successfully from path: {fullPath}");
        }

        return prefabLoad;
    }

    private string ProcessCodeMesh(int num)
    {
        return num < 10 ? $"0{num}" : num.ToString();
    }
}
