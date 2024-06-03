using System;
using BillUtils.GameObjectUtilities;
using BillUtils.SpaceUtilities;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEditor;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private List<GameObject> furnitures;
    [SerializeField] private GameObject furniture;
    [SerializeField] private GameObject roof;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject door;
    [SerializeField] private List<Transform> pivots;
    [SerializeField] private List<Material> mats;
    [SerializeField] private Material TNS_Mat;
    [SerializeField] private Renderer roofRenderer;
    [SerializeField, Range(0, 1)] private float alpha = 0.5f;

    public void AddMat(Material mat) => mats.Add(mat);
    public void AddTNSMat(Material mat) => TNS_Mat = mat;
    public GameObject GetFirstFur() => furnitures[0];

    private void Start()
    {
        alpha = 0.5f;
        roofRenderer = GetComponentInChildren<Renderer>();
    }

    [Button]
    private void Init()
    {
        brick = GetChildObjectByName("Brick");
        if (brick != null)
        {
            Debug.Log($"Brick object found: {brick.name}");
        }
        else
        {
            Debug.LogError("Brick object not found.");
        }
        furniture = GetChildObjectByName("Furniture");
        furnitures = InitFurs(furniture.transform);
        if (furnitures.Count > 0)
        {
            Debug.Log($"Found {furnitures.Count} furniture objects.");
        }
        else
        {
            Debug.LogError("No furniture objects found.");
        }
    }

    private List<GameObject> InitFurs(Transform furnitureContainer)
    {
        List<GameObject> childFur = new List<GameObject>();
        foreach (Transform child in furnitureContainer)
        {
            childFur.Add(child.gameObject);
        }
        return childFur;
    }

    private List<GameObject> GetChildObjectsByName(string name)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                children.Add(child.gameObject);
            }
        }
        return children;
    }

    private GameObject GetChildObjectByName(string childName)
    {
        return GetChildObjectByName(transform, childName);
    }

    private GameObject GetChildObjectByName(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
            {
                return child.gameObject;
            }
            GameObject result = GetChildObjectByName(child, childName);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    public void TurnOnRoof()
    {
        roof.SetActive(true);
    }

    public void TurnOffRoof()
    {
        roof.SetActive(false);
    }

    public void TurnOnFurniture(int indexFur)
    {
        furnitures[indexFur].SetActive(true);
    }

    public void TurnOffFurniture(int indexFur)
    {
        furnitures[indexFur].SetActive(false);
    }

    public Material SetMaterialAlpha(float alpha)
    {
        Material material = TNS_Mat;

        if (material.HasProperty("_Color"))
        {
            Color color = material.color;
            color.a = alpha;
            material.color = color;
        }
        else if (material.HasProperty("_BaseColor"))
        {
            Color color = material.GetColor("_BaseColor");
            color.a = alpha;
            material.SetColor("_BaseColor", color);
        }

        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
        return material;
    }

    public void SetRoofTransparent()
    {
        MeshRenderer mesh = roof.GetComponentInChildren<MeshRenderer>();
        Material tnsMat = SetMaterialAlpha(0.4f);
        if (mesh != null)
        {
            mesh.materials = new Material[0]; // Clear materials
        }
        ApplyMaterials(roof, tnsMat);
    }

    public void ResetRoof()
    {
        MeshRenderer mesh = roof.GetComponentInChildren<MeshRenderer>();
        if (mesh != null)
        {
            mesh.materials = new Material[0]; // Clear materials
        }
        ApplyMaterials(roof, mats[0]);
    }

    public void ChangeMaterials(Transform fur)
    {
        if (mats.Count < 2)
        {
            Debug.LogError("Not enough materials provided. Please ensure there are at least 4 materials.");
            return;
        }
        door = GetDoor(fur);
        window = GetWindow(fur);
        ClearMaterials(fur);
        if (brick != null)
        {
            ApplyMaterials(brick, mats[0]);
        }

        if (door != null)
        {
            ApplyMaterials(door, mats[0]);
        }

        if (window != null)
        {

            ApplyMaterials(window, mats[0], mats[1]);
        }

        if (roof != null)
        {
            ApplyMaterials(roof, mats[0]);
        }
    }

    private GameObject GetDoor(Transform fur)
    {
        return GetChildObjectByName(fur, "Door");
    }

    private GameObject GetWindow(Transform fur)
    {
        return GetChildObjectByName(fur, "Window");
    }

    private void ApplyMaterials(GameObject obj, Material mat1, Material mat2 = null)
    {
        MeshRenderer mesh = obj.GetComponentInChildren<MeshRenderer>();
        if (mesh != null)
        {
            if (mat2 != null)
            {
                mesh.materials = new Material[] { mat1, mat2 };
            }
            else
            {
                mesh.materials = new Material[] { mat1 };
            }
            Debug.Log($"Applied materials to {obj.name}");
        }
        else
        {
            Debug.LogError($"MeshRenderer not found in {obj.name}");
        }
    }

    [Button]
    private void ClearMaterials(Transform fur)
    {
        ClearMaterial(brick);
        ClearMaterial(GetDoor(fur));
        ClearMaterial(GetWindow(fur));
        ClearMaterial(door);
    }

    private void ClearMaterial(GameObject obj)
    {
        if (obj != null)
        {
            MeshRenderer mesh = obj.GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
            {
                mesh.materials = new Material[0]; // Clear materials
                Debug.Log($"Cleared materials from {obj.name}");
            }
            else
            {
                Debug.LogError($"MeshRenderer not found in {obj.name}");
            }
        }
    }

    // FIXME
    public GameObject CheckFurnituresSide(GameObject collideObj)
    {
        Vector3 colliderPoint = collideObj.transform.position;
        GameObject doorObject = GetChildObjectByName(furnitures[0].transform, "Door");
        if (!collideObj.transform.parent.parent.CompareTag("Block"))
        {
            return furnitures[0];
        }

        if (transform.position.x == colliderPoint.x && transform.position.z == colliderPoint.z)
        {
            furnitures[0].transform.GetChild(0).gameObject.SetActive(false);
            return furnitures[0];
        }

        GameObject closestFurniture = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject furnitureItem in furnitures)
        {
            doorObject = furnitureItem.transform.GetChild(0).gameObject;
            if (doorObject != null)
            {
                Vector3 doorPosition = SpaceUtils.GetMeshWorldPosition(doorObject);
                float distance = Vector3.Distance(doorPosition, colliderPoint);
                Debug.Log($"Distance: {distance}");
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestFurniture = furnitureItem;
                }
            }
        }
        return closestFurniture;
    }

    public Vector3 GetMeshWorldPosition(GameObject obj)
    {
        Vector3 worldPosition = SpaceUtils.GetMeshWorldPosition(obj);
        Debug.Log($"Mesh World Position of {obj.name}: {worldPosition}");
        return worldPosition;
    }
}
