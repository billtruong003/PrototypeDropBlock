using System.Security.Cryptography;
using UnityEngine;

public class CustomHeaderAttribute : PropertyAttribute
{
    public string header;
    public int fontSize;
    public Color color;

    public CustomHeaderAttribute(string header, int fontSize = 12, string hexColor = "#FFFFFF")
    {
        this.header = header;
        this.fontSize = fontSize;
        this.color = HexToColor(hexColor);
    }

    private Color HexToColor(string hex)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hex, out color))
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
