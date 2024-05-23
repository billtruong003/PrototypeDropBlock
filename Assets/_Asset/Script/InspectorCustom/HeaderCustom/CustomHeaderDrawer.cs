using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CustomHeaderAttribute))]
public class CustomHeaderDrawer : DecoratorDrawer
{
    public override void OnGUI(Rect position)
    {
        CustomHeaderAttribute customHeader = (CustomHeaderAttribute)attribute;

        GUIStyle style = new GUIStyle(EditorStyles.label)
        {
            fontSize = customHeader.fontSize,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = customHeader.color } // Đặt màu sắc của văn bản
        };

        GUIContent content = new GUIContent(customHeader.header);

        EditorGUI.LabelField(position, content, style);
    }

    public override float GetHeight()
    {
        CustomHeaderAttribute customHeader = (CustomHeaderAttribute)attribute;
        return EditorGUIUtility.singleLineHeight * (customHeader.fontSize / 12.0f);
    }
}
