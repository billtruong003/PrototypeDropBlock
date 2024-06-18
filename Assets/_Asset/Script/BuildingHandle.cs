using System;
using BillUtils.GameObjectUtilities;
using BillUtils.SpaceUtils;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEditor;
using BlockBuilder.BlockManagement;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private List<GameObject> furnitures;
    [SerializeField] private GameObject furniture;
    [SerializeField] private GameObject roof;
    [SerializeField] private GameObject window;
    [SerializeField] private GameObject door;
    [SerializeField] private Transform pivot;
    [SerializeField] private List<Material> mats;
    [SerializeField] private Material TNS_Mat;
    [SerializeField] private Renderer roofRenderer;
    [SerializeField, Range(0, 1)] private float alpha = 0.5f;

    public void AddMat(Material mat) => mats.Add(mat);
    public void AddTNSMat(Material mat) => TNS_Mat = mat;
    public GameObject GetFirstFur() => furnitures[0];

    public void AddMaterial(Material mat) => mats.Add(mat);

    public void SetTNSMaterial(Material mat) => TNS_Mat = mat;

    public GameObject GetFirstFurniture() => furnitures.Count > 0 ? furnitures[0] : null;

    public void TurnOnRoof() => SetActiveState(roof, true);

    public void TurnOffRoof() => SetActiveState(roof, false);

    public void TurnOnFurniture(int index) => SetActiveState(furnitures, index, true);

    public void TurnOffFurniture(int index) => SetActiveState(furnitures, index, false);

    private void Start()
    {
        alpha = 0.5f;
        roofRenderer = GetComponentInChildren<Renderer>();
    }

    [Button]
    public void Init()
    {
        brick = GetChildObjectByName(transform, "Brick");
        Debug.Log(brick != null ? $"Brick object found: {brick.name}" : "Brick object not found.");

        furniture = GetChildObjectByName(transform, "Furniture");
        furnitures = InitializeFurnitureList(furniture?.transform);

        Debug.Log(furnitures.Count > 0 ? $"Found {furnitures.Count} furniture objects." : "No furniture objects found.");

        roof = GetChildObjectByName(transform, "Roof");
        pivot = GetChildObjectByName(transform, "Pivot")?.transform;
    }

    private List<GameObject> InitializeFurnitureList(Transform furnitureContainer)
    {
        var childFur = new List<GameObject>();
        if (furnitureContainer != null)
        {
            foreach (Transform child in furnitureContainer)
            {
                childFur.Add(child.gameObject);
            }
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

    public void SetPivotParent(Vector3 pos)
    {
        pivot.position = pos;
        GetChildObjectByName("Container").transform.SetParent(pivot, true);
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
            mesh.materials = new Material[0];
        }
        ApplyMaterials(roof, tnsMat);
    }

    public void ResetRoof()
    {
        MeshRenderer mesh = roof.GetComponentInChildren<MeshRenderer>();
        if (mesh != null)
        {
            mesh.materials = new Material[0];
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
        var colliderPoint = collideObj.transform.position;
        var doorObject = GetChildObjectByName(furnitures[0].transform, "Door");

        if (collideObj.transform.parent?.parent == null || !collideObj.transform.parent.parent.CompareTag("Block") || RoofIsFull(collideObj))
        {
            SetActiveStateChild(furnitures[0], 0, false);
            return furnitures[0];
        }

        if (IsPositionMatch(transform.position, colliderPoint))
        {
            SetActiveStateChild(furnitures[0], 0, false);
            return furnitures[0];
        }

        return FindClosestFurniture(colliderPoint);
    }

    private GameObject FindClosestFurniture(Vector3 colliderPoint)
    {
        GameObject closestFurniture = null;
        float closestDistance = float.MaxValue;

        foreach (var furnitureItem in furnitures)
        {
            var doorObject = furnitureItem.transform.GetChild(0).gameObject;
            if (doorObject != null)
            {
                var doorPosition = SpaceUtilities.GetMeshWorldPosition(doorObject);
                var distance = Vector3.Distance(doorPosition, colliderPoint);
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

    public bool RoofIsFull(GameObject colliderObj)
    {
        Transform parentCollide = colliderObj.transform.parent.parent;
        BlockController blockController = parentCollide.GetComponent<BlockController>();
        return blockController.CheckRoofIsFull();
    }

    public Vector3 GetMeshWorldPosition(GameObject obj)
    {
        Vector3 worldPosition = SpaceUtilities.GetMeshWorldPosition(obj);
        Debug.Log($"Mesh World Position of {obj.name}: {worldPosition}");
        return worldPosition;
    }
    public void Rotate()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        Transform targetTransform = this.transform.GetChild(0);
        if (targetTransform == null)
        {
            Debug.LogWarning("No selected object to rotate.");
            yield break;
        }

        Quaternion targetRotation = targetTransform.rotation * Quaternion.Euler(0, 90, 0);

        float duration = 0.5f;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            targetTransform.rotation = Quaternion.Slerp(targetTransform.rotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetTransform.rotation = targetRotation;

        ReconstructSystem.Instance.RotateDone();
    }

    private void SetActiveState(GameObject obj, bool state)
    {
        if (obj != null)
        {
            obj.SetActive(state);
        }
    }
    private void SetActiveStateChild(GameObject obj, int index, bool state)
    {
        if (index >= 0 && index < obj.transform.childCount)
        {
            obj.transform.GetChild(index).gameObject.SetActive(state);
        }
    }
    private void SetActiveState(List<GameObject> objects, int index, bool state)
    {
        if (index >= 0 && index < objects.Count)
        {
            objects[index].SetActive(state);
        }
    }

    private bool IsPositionMatch(Vector3 pos1, Vector3 pos2)
    {
        return pos1.x == pos2.x && pos1.z == pos2.z;
    }
}
