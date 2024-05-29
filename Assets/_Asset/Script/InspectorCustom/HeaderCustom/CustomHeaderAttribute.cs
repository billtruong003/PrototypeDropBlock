using System.Security.Cryptography;
using UnityEngine;

namespace BillUtils.SerializeCustom
{
    public class CustomHeaderAttribute : PropertyAttribute
    {
        public string Header { get; private set; }
        public int FontSize { get; private set; }
        public Color Color { get; private set; }

        public CustomHeaderAttribute(string header, int fontSize = 12, string hexColor = "#FFFFFF")
        {
            this.Header = header;
            this.FontSize = fontSize;
            this.Color = HexToColor(hexColor);
        }

        private Color HexToColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                return color;
            }
            else
            {
                Debug.LogWarning("Invalid hex color format. Defaulting to white.");
                return Color.white;
            }
        }
    }
}
