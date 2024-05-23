using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor.SceneManagement;

[CreateAssetMenu(fileName = "SceneToggleManager", menuName = "Tools/SceneToggleManager")]
public class SceneToggleManager : ScriptableObject
{
    public List<SceneField> scenes = new List<SceneField>();
    public List<SceneBookmark> bookmarks = new List<SceneBookmark>();

    [System.Serializable]
    public class SceneField
    {
        public string sceneName;
        public int buildIndex;
        public bool isEnabled;
    }

    [System.Serializable]
    public class SceneBookmark
    {
        public string sceneName;
        public int buildIndex;
    }

    [Button("Open Toggled Scenes")]
    public void OpenToggledScenes()
    {
        foreach (var scene in scenes)
        {
            if (scene.isEnabled)
            {
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[scene.buildIndex].path, OpenSceneMode.Additive);
            }
        }
    }

    [Button("Open Toggled Scenes Additive")]
    public void OpenToggledScenesAdditive()
    {
        foreach (var scene in scenes)
        {
            if (scene.isEnabled)
            {
                EditorSceneManager.OpenScene(EditorBuildSettings.scenes[scene.buildIndex].path, OpenSceneMode.Additive);
            }
        }
    }
}
