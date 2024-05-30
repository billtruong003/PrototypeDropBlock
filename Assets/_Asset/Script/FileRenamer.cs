using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;

public class FileRenamer : MonoBehaviour
{
    [SerializeField] private string prefixName = "TNSMAT";
    [SerializeField] private List<Material> mats;

    [ContextMenu("Rename Files")]
    [Button]
    private void RenameFiles()
    {
        int totalCount = mats.Count;
        int digits = totalCount.ToString().Length; // Determine the number of digits needed
        int index = 1;

        foreach (var mat in mats)
        {
            // Get the path of the material's source file
            string oldFilePath = AssetDatabase.GetAssetPath(mat);
            if (string.IsNullOrEmpty(oldFilePath))
            {
                Debug.LogError($"Material {mat.name} does not have a valid asset path.");
                continue;
            }

            // Determine the new file name
            string directory = Path.GetDirectoryName(oldFilePath);
            string newFileName = $"{prefixName}_0{ProcessCodeMesh(index)}"; // Example new file name
            string newFilePath = Path.Combine(directory, newFileName);

            // Rename the file
            if (File.Exists(oldFilePath))
            {
                AssetDatabase.RenameAsset(oldFilePath, Path.GetFileNameWithoutExtension(newFileName));
                Debug.Log($"File renamed from {oldFilePath} to {newFilePath}");
                index++;
            }
            else
            {
                Debug.LogError($"File not found: {oldFilePath}");
            }
        }

        // Refresh the AssetDatabase to reflect changes in the Unity Editor
        AssetDatabase.Refresh();
    }
    private string ProcessCodeMesh(int num)
    {
        return num < 10 ? $"0{num}" : num.ToString();
    }
}
