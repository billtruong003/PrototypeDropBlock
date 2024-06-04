#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace BillUtils.SerializeCustom
{
    [CustomPropertyDrawer(typeof(BillHeaderAttribute))]
    public class CustomHeaderDrawer : DecoratorDrawer
    {
        public override void OnGUI(Rect position)
        {
            BillHeaderAttribute customHeader = (BillHeaderAttribute)attribute;

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
            BillHeaderAttribute customHeader = (BillHeaderAttribute)attribute;
            return EditorGUIUtility.singleLineHeight * (customHeader.FontSize / 12.0f);
        }
    }
}
#endif
