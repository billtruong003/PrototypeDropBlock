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

                // Recursively search for the child in the child's children
                GameObject result = GetChildObjectByName(child, childName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
