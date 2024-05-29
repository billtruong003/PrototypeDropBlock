using UnityEditor;
using UnityEngine;

namespace BillUtils.SerializeCustom
{
    [CustomPropertyDrawer(typeof(CustomHeaderAttribute))]
    public class CustomHeaderDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            CustomHeaderAttribute customHeader = (CustomHeaderAttribute)attribute;

            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                fontSize = customHeader.FontSize,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.MiddleLeft,
                normal = { textColor = customHeader.Color } // Đặt màu sắc của văn bản
            };

            GUIContent content = new GUIContent(customHeader.Header);

            EditorGUI.LabelField(position, content, style);
        }

        public override float GetHeight()
        {
            CustomHeaderAttribute customHeader = (CustomHeaderAttribute)attribute;
            return EditorGUIUtility.singleLineHeight * (customHeader.FontSize / 12.0f);
        }
    }
}