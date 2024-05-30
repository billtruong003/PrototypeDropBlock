using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
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
    private void Start()
    {
        alpha = 0.5f;
        roofRenderer = GetComponentInChildren<Renderer>();
    }

    [Button]
    private void Init()
    {
        brick = GetChildObjectByName("Brick");
        furniture = GetChildObjectByName("Furniture");
        roof = GetChildObjectByName("Roof");
        window = GetChildObjectByName("Window");
        door = GetChildObjectByName("Door");
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

    public void TurnOnFurniture()
    {
        furniture.SetActive(true);
    }

    public void TurnOffFurniture()
    {
        furniture.SetActive(false);
    }

    public Material SetMaterialAlpha(float alpha)
    {
        // Tạo một instance riêng của material
        Material material = TNS_Mat;

        // Kiểm tra nếu material có thuộc tính _Color
        if (material.HasProperty("_Color"))
        {
            // Lấy màu hiện tại
            Color color = material.color;

            // Thiết lập giá trị alpha mới
            color.a = alpha;

            // Gán lại màu mới cho material
            material.color = color;
        }
        else if (material.HasProperty("_BaseColor"))
        {
            // Nếu sử dụng HDRP hoặc URP với shader có thuộc tính _BaseColor
            Color color = material.GetColor("_BaseColor");
            color.a = alpha;
            material.SetColor("_BaseColor", color);
        }

        // Thiết lập chế độ blend để hỗ trợ alpha
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
        SetMaterialAlpha(alpha);
    }

    public void ResetMaterialAlpha()
    {
        SetMaterialAlpha(1.0f);
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

    public void TransparentRoof()
    {
        MeshRenderer mesh = roof.GetComponentInChildren<MeshRenderer>();
        Material tnsMat = SetMaterialAlpha(0.5f);
        if (mesh != null)
        {
            mesh.materials = new Material[0]; // Clear materials
        }
        ApplyMaterials(roof, mats[0]);
    }
    public void ResetMaterial()
    {
        MeshRenderer mesh = roof.GetComponentInChildren<MeshRenderer>();
        Material tnsMat = SetMaterialAlpha(0.5f);
        if (mesh != null)
        {
            mesh.materials = new Material[0]; // Clear materials
        }
        ApplyMaterials(roof, tnsMat);
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

        if (furniture != null)
        {
            MeshRenderer mesh = furniture.GetComponentInChildren<MeshRenderer>();
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

}
