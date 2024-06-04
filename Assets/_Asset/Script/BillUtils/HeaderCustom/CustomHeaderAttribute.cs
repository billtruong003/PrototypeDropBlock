using UnityEngine;

namespace BillUtils.SerializeCustom
{
    public class BillHeaderAttribute : PropertyAttribute
    {
        public string Header { get; private set; }
        public int FontSize { get; private set; }
        public Color Color { get; private set; }

        public BillHeaderAttribute(string header, int fontSize, string hexColor)
        {
            Header = header;
            FontSize = fontSize;
            Color color;
            if (ColorUtility.TryParseHtmlString(hexColor, out color))
            {
                Color = color;
            }
            else
            {
                Color = Color.white; // Default to white if parsing fails
            }
        }
    }
}
