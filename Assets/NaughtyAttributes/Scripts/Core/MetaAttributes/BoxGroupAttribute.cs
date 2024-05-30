using System;
using UnityEngine;
namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class BoxGroupAttribute : MetaAttribute, IGroupAttribute
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public float Opacity { get; private set; }
        public bool Foldable { get; private set; }

        public BoxGroupAttribute(string name = "", string colorHex = "#333333", float opacity = 1f, bool foldable = true)
        {
            Name = name;
            ColorUtility.TryParseHtmlString(colorHex, out Color color);
            Color = color;
            Opacity = Mathf.Clamp01(opacity);
            Foldable = foldable;
        }
    }
    public class FoldableBoxGroupAttribute : PropertyAttribute
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public float Opacity { get; private set; }

        public FoldableBoxGroupAttribute(string name = "", string colorHex = "#FFFFFF", float opacity = 1f)
        {
            Name = name;
            ColorUtility.TryParseHtmlString(colorHex, out Color color);
            Color = color;
            Opacity = Mathf.Clamp01(opacity);
        }
    }
}
