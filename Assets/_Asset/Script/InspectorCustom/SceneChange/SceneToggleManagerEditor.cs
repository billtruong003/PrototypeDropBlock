using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(SceneToggleManager))]
public class SceneToggleManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SceneToggleManager manager = (SceneToggleManager)target;

        if (GUILayout.Button("Refresh Scenes"))
        {
            RefreshScenes(manager);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Scenes", EditorStyles.boldLabel);
        foreach (var scene in manager.scenes)
        {
            EditorGUILayout.BeginHorizontal();
            scene.isEnabled = EditorGUILayout.Toggle(scene.isEnabled, GUILayout.Width(20));
            EditorGUILayout.LabelField(scene.sceneName, GUILayout.Width(200));
            EditorGUILayout.LabelField($"Build Index: {scene.buildIndex}", GUILayout.Width(100));

            if (GUILayout.Button("Bookmark", GUILayout.Width(80)))
            {
                BookmarkScene(manager, scene);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Bookmarks", EditorStyles.boldLabel);
        foreach (var bookmark in manager.bookmarks)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(bookmark.sceneName, GUILayout.Width(200));
            EditorGUILayout.LabelField($"Build Index: {bookmark.buildIndex}", GUILayout.Width(100));

            if (GUILayout.Button("Remove", GUILayout.Width(80)))
            {
                RemoveBookmark(manager, bookmark);
                break; // Avoid modifying the list while iterating
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUI.changed)
        {
            UpdateBuildSettings(manager);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Open Scene"))
        {
            manager.OpenToggledScenes();
        }

        if (GUILayout.Button("Open Scene Additive"))
        {
            manager.OpenToggledScenesAdditive();
        }
    }

    private void RefreshScenes(SceneToggleManager manager)
    {
        manager.scenes.Clear();
        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            var scenePath = EditorBuildSettings.scenes[i].path;
            var sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            manager.scenes.Add(new SceneToggleManager.SceneField
            {
                sceneName = sceneName,
                buildIndex = i,
                isEnabled = EditorBuildSettings.scenes[i].enabled
            });
        }
    }

    private void BookmarkScene(SceneToggleManager manager, SceneToggleManager.SceneField scene)
    {
        if (!manager.bookmarks.Exists(b => b.buildIndex == scene.buildIndex))
        {
            manager.bookmarks.Add(new SceneToggleManager.SceneBookmark
            {
                sceneName = scene.sceneName,
                buildIndex = scene.buildIndex
            });
        }
    }

    private void RemoveBookmark(SceneToggleManager manager, SceneToggleManager.SceneBookmark bookmark)
    {
        manager.bookmarks.Remove(bookmark);
    }

    private void UpdateBuildSettings(SceneToggleManager manager)
    {
        var scenes = new List<EditorBuildSettingsScene>();
        foreach (var scene in manager.scenes)
        {
            scenes.Add(new EditorBuildSettingsScene(EditorBuildSettings.scenes[scene.buildIndex].path, scene.isEnabled));
        }
        EditorBuildSettings.scenes = scenes.ToArray();
    }

}
