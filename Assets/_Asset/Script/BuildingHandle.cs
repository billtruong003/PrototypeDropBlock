using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private List<GameObject> furniture;
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
    private void Start()
    {
        alpha = 0.5f;
        roofRenderer = GetComponentInChildren<Renderer>();
    }

    [Button]
    private void Init()
    {
        brick = GetChildObjectByName("Brick");
        furniture = GetChildObjectsByName("Furniture");
        roof = GetChildObjectByName("Roof");
        window = GetChildObjectByName("Window");
        door = GetChildObjectByName("Door");
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
        furniture[indexFur].SetActive(true);
    }

    public void TurnOffFurniture(int indexFur)
    {
        furniture[indexFur].SetActive(false);
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

    public void ChangeMaterials()
    {
        // Ensure there are enough materials
        if (mats.Count < 2)
        {
            Debug.LogError("Not enough materials provided. Please ensure there are at least 4 materials.");
            return;
        }

        ClearMaterials();

        // Apply materials to brick
        if (brick != null)
        {
            ApplyMaterials(brick, mats[0]);
        }

        // Apply materials to door
        if (door != null)
        {
            ApplyMaterials(door, mats[0]);
        }

        // Apply materials to window
        if (window != null)
        {
            ApplyMaterials(window, mats[0], mats[1]);
        }

        // Apply materials to roof
        if (roof != null)
        {
            ApplyMaterials(roof, mats[0]);
        }
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
    private void ClearMaterials()
    {
        if (brick != null)
        {
            MeshRenderer mesh = brick.GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
            {
                mesh.materials = new Material[0]; // Clear materials
            }
        }

        if (roof != null)
        {
            MeshRenderer mesh = roof.GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
            {
                mesh.materials = new Material[0]; // Clear materials
            }
        }

        if (window != null)
        {
            MeshRenderer mesh = window.GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
            {
                mesh.materials = new Material[0]; // Clear materials
            }
        }

        if (door != null)
        {
            MeshRenderer mesh = door.GetComponentInChildren<MeshRenderer>();
            if (mesh != null)
            {
                mesh.materials = new Material[0]; // Clear materials
            }
        }
    }

    public GameObject CheckFurnituresSide(Vector3 collideNeedCheck)
    {
        if (transform.position.x == collideNeedCheck.x && transform.position.z == collideNeedCheck.z)
        {
            furniture[0].transform.GetChild(2).gameObject.SetActive(false);
            return furniture[0];
        }

        GameObject closestFurniture = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject furnitureItem in furniture)
        {
            float distance = Vector3.Distance(furnitureItem.transform.position, collideNeedCheck);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestFurniture = furnitureItem;
            }
        }

        return closestFurniture;
    }

}
