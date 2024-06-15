using UnityEngine;
using System.Collections.Generic;

namespace BillUtils.GameObjectUtilities
{
    public static class GameObjectUtils
    {
        public static GameObject GetChildObjectByName(GameObject parent, string childName)
        {
            if (parent == null)
            {
                Debug.LogError("Parent object is null.");
                return null;
            }
            return GetChildObjectByName(parent.transform, childName);
        }

        public static GameObject GetChildObjectByName(Transform parent, string childName)
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

        public static void DisableAllMeshRenderers(GameObject target)
        {
            MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = false;
            }
        }

        public static void EnableAllMeshRenderers(GameObject target)
        {
            MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.enabled = true;
            }
        }

        public static void SetAllMaterials(GameObject target, Material mat)
        {
            MeshRenderer[] meshRenderers = target.GetComponentsInChildren<MeshRenderer>();
            foreach (var meshRenderer in meshRenderers)
            {
                meshRenderer.material = mat;
            }
        }
    }
}
