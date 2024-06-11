// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;

// namespace NaughtyAttributes.Editor
// {
//     [CustomPropertyDrawer(typeof(FoldableBoxGroupAttribute))]
//     public class FoldableBoxGroupDrawer : DecoratorDrawer
//     {
//         private static readonly Dictionary<string, bool> FoldoutStates = new Dictionary<string, bool>();
//         private const float HeaderHeight = 20.0f;
//         private const float SpaceHeight = 2.0f;
//         private static readonly GUIStyle FoldoutHeaderStyle = new GUIStyle(EditorStyles.foldout)
//         {
//             fontStyle = FontStyle.Bold,
//             fixedHeight = HeaderHeight
//         };

//         public override void OnGUI(Rect position)
//         {
//             FoldableBoxGroupAttribute foldableBoxGroup = attribute as FoldableBoxGroupAttribute;
//             if (foldableBoxGroup == null)
//             {
//                 Debug.LogError("FoldableBoxGroupAttribute is null");
//                 return;
//             }

//             if (!FoldoutStates.ContainsKey(foldableBoxGroup.Name))
//             {
//                 FoldoutStates[foldableBoxGroup.Name] = true;
//             }

//             Rect foldoutRect = new Rect(position.x, position.y, position.width, HeaderHeight);
//             FoldoutStates[foldableBoxGroup.Name] = EditorGUI.Foldout(foldoutRect, FoldoutStates[foldableBoxGroup.Name], foldableBoxGroup.Name, true, FoldoutHeaderStyle);

//             if (FoldoutStates[foldableBoxGroup.Name])
//             {
//                 position.y += HeaderHeight + SpaceHeight;
//                 EditorGUI.DrawRect(new Rect(position.x, position.y, position.width, position.height - (HeaderHeight + SpaceHeight)), new Color(foldableBoxGroup.Color.r, foldableBoxGroup.Color.g, foldableBoxGroup.Color.b, foldableBoxGroup.Opacity));
//             }
//         }

//         public override float GetHeight()
//         {
//             FoldableBoxGroupAttribute foldableBoxGroup = attribute as FoldableBoxGroupAttribute;
//             if (foldableBoxGroup == null)
//             {
//                 Debug.LogError("FoldableBoxGroupAttribute is null");
//                 return 0;
//             }

//             if (FoldoutStates.ContainsKey(foldableBoxGroup.Name) && FoldoutStates[foldableBoxGroup.Name])
//             {
//                 return base.GetHeight() + HeaderHeight + SpaceHeight;
//             }
//             else
//             {
//                 return HeaderHeight;
//             }
//         }

//     }
// }
