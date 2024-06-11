using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using NaughtyAttributes;

public class ToolGenPrefab : MonoBehaviour
{
    [SerializeField] private List<Vector3> Pose;
    [SerializeField] private List<Quaternion> Angle;
    [SerializeField] private string pathLoad = "Building/";
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
            // Create the main parent object
            GameObject mainPrefab = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, container);
            mainPrefab.name = $"BD_0{ProcessCodeMesh(i + 1)}";
            BuildingHandle bdHandle = mainPrefab.AddComponent<BuildingHandle>();

            GameObject pivot = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, mainPrefab.transform);
            pivot.name = "Pivot";

            // Create the container
            GameObject containAllSmall = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, mainPrefab.transform);
            containAllSmall.name = "Container";

            // Create Brick
            CreateChildObject(containAllSmall.transform, "Brick", body, i + 1);

            // Create Furniture with different sides
            GameObject furniture = Instantiate(instantiatePrefab, Vector3.zero, quaternion.identity, containAllSmall.transform);
            furniture.name = "Furniture";
            furniture.AddComponent<AppearSide>();

            CreateSideObject(furniture.transform, "Forward", 0, door, window, i + 1);
            CreateSideObject(furniture.transform, "Left", 90, door, window, i + 1);
            CreateSideObject(furniture.transform, "Backward", 180, door, window, i + 1);
            CreateSideObject(furniture.transform, "Right", -90, door, window, i + 1);

            // Create Roof
            CreateChildObject(containAllSmall.transform, "Roof", roof, i + 1);
            bdHandle.Init();
            containAllSmall.transform.SetLocalPositionAndRotation(Pose[i], Angle[i]);

        }
    }

    private void CreateSideObject(Transform parent, string sideName, float yRotation, string doorType, string windowType, int index)
    {
        GameObject side = new GameObject(sideName);
        side.transform.SetParent(parent);
        CreateChildObject(side.transform, "Door", doorType, index);
        CreateChildObject(side.transform, "Window", windowType, index);
        side.transform.localEulerAngles = new Vector3(0, yRotation, 0);
        side.SetActive(false);
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
