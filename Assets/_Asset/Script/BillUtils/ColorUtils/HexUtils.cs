using UnityEngine;

namespace BillUtils.HexUtils
{
    public static class HexColors
    {
        public const string Red = "#FF0000";
        public const string Green = "#00FF00";
        public const string Blue = "#0000FF";
        public const string Yellow = "#FFFF00";
        public const string Cyan = "#00FFFF";
        public const string Magenta = "#FF00FF";
        public const string White = "#FFFFFF";
        public const string Black = "#000000";
        public const string Orange = "#FFA500";
        public const string Purple = "#800080";
        public const string Pink = "#FFC0CB";
        public const string Brown = "#A52A2A";
        public const string Grey = "#808080";
        public const string LightGrey = "#D3D3D3";
        public const string DarkGrey = "#A9A9A9";

        public const string DarkRed = "#8B0000";
        public const string DarkGreen = "#006400";
        public const string DarkBlue = "#00008B";
        public const string LightBlue = "#ADD8E6";
        public const string LightGreen = "#90EE90";
        public const string LightYellow = "#FFFFE0";
        public const string LightPink = "#FFB6C1";
        public const string SkyBlue = "#87CEEB";
        public const string Coral = "#FF7F50";
        public const string Tomato = "#FF6347";
        public const string Gold = "#FFD700";
        public const string Chocolate = "#D2691E";
        public const string Olive = "#808000";
        public const string Teal = "#008080";
        public const string Navy = "#000080";
        public const string Lavender = "#E6E6FA";
        public const string Beige = "#F5F5DC";
        public const string Silver = "#C0C0C0";
        public const string Maroon = "#800000";
        public const string Crimson = "#DC143C";
        public const string Indigo = "#4B0082";
        public const string Violet = "#EE82EE";
        public const string Plum = "#DDA0DD";
        public const string Orchid = "#DA70D6";
        public const string Salmon = "#FA8072";
        public const string SeaGreen = "#2E8B57";
        public const string SlateBlue = "#6A5ACD";
        public const string SteelBlue = "#4682B4";
        public const string Turquoise = "#40E0D0";
        public const string Tan = "#D2B48C";
        public const string Thistle = "#D8BFD8";
        public const string Wheat = "#F5DEB3";
        public const string Sienna = "#A0522D";
        public const string HotPink = "#FF69B4";
        public const string Chartreuse = "#7FFF00";
        public const string Aquamarine = "#7FFFD4";
        public const string DarkSlateGray = "#2F4F4F";
        public const string LightSlateGray = "#778899";
        public const string MintCream = "#F5FFFA";
        public const string RosyBrown = "#BC8F8F";
        public const string SandyBrown = "#F4A460";
        public const string SaddleBrown = "#8B4513";
        public const string LightCoral = "#F08080";
        public const string PaleVioletRed = "#DB7093";
        public const string DarkOrchid = "#9932CC";
        public const string DarkMagenta = "#8B008B";
        public const string DarkOliveGreen = "#556B2F";
        public const string LightGoldenrodYellow = "#FAFAD2";
        public const string PaleGreen = "#98FB98";
        public const string SpringGreen = "#00FF7F";
        public const string PaleTurquoise = "#AFEEEE";
        public const string RoyalBlue = "#4169E1";
        public const string DarkTurquoise = "#00CED1";
        public const string MediumTurquoise = "#48D1CC";
        public const string MediumAquamarine = "#66CDAA";
        public const string MediumSeaGreen = "#3CB371";
        public const string MediumSlateBlue = "#7B68EE";
        public const string MediumOrchid = "#BA55D3";
        public const string MediumPurple = "#9370DB";
        public const string MediumVioletRed = "#C71585";
        public const string MediumSpringGreen = "#00FA9A";
        public const string MidnightBlue = "#191970";
        public const string NavajoWhite = "#FFDEAD";
        public const string PeachPuff = "#FFDAB9";
        public const string PapayaWhip = "#FFEFD5";
        public const string MistyRose = "#FFE4E1";
        public const string Moccasin = "#FFE4B5";
        public const string MediumBlue = "#0000CD";
        public const string LightSeaGreen = "#20B2AA";
        public const string DarkSalmon = "#E9967A";
        public const string PaleGoldenrod = "#EEE8AA";
        public const string BlanchedAlmond = "#FFEBCD";
        public const string LightSteelBlue = "#B0C4DE";
        public const string PowderBlue = "#B0E0E6";
        public const string OldLace = "#FDF5E6";
        public const string AliceBlue = "#F0F8FF";
        public const string AntiqueWhite = "#FAEBD7";
        public const string BurlyWood = "#DEB887";
        public const string CornflowerBlue = "#6495ED";
        public const string DarkGoldenrod = "#B8860B";
        public const string DarkKhaki = "#BDB76B";
        public const string DarkSeaGreen = "#8FBC8F";
        public const string FireBrick = "#B22222";
        public const string FloralWhite = "#FFFAF0";
        public const string ForestGreen = "#228B22";
        public const string GhostWhite = "#F8F8FF";
        public const string Honeydew = "#F0FFF0";
        public const string IndianRed = "#CD5C5C";
        public const string Khaki = "#F0E68C";
        public const string LemonChiffon = "#FFFACD";
        public const string LightCyan = "#E0FFFF";
        public const string LightSalmon = "#FFA07A";
        public const string LightSkyBlue = "#87CEFA";
        public const string Linen = "#FAF0E6";
        public const string SeaShell = "#FFF5EE";
        public const string SlateGray = "#708090";
        public const string Snow = "#FFFAFA";
        public const string YellowGreen = "#9ACD32";
        public const string DarkCyan = "#008B8B";
        public const string DarkSlateBlue = "#483D8B";


        /// <summary>
        /// Converts a hex color string to a Color object.
        /// </summary>
        /// <param name="hex">Hex color string (e.g., "#FF0000" or "FF0000")</param>
        /// <returns>Color object</returns>
        public static Color ToColor(string hex)
        {
            if (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            if (hex.Length != 6 && hex.Length != 8)
            {
                throw new System.ArgumentException("Invalid hex color format. Hex color must be 6 or 8 characters long.");
            }

            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            byte a = hex.Length == 8 ? byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) : (byte)255;

            return new Color32(r, g, b, a);
        }

        /// <summary>
        /// Converts a Color object to a hex color string.
        /// </summary>
        /// <param name="color">Color object</param>
        /// <returns>Hex color string</returns>
        public static string ToHex(Color color)
        {
            Color32 color32 = color;
            return $"#{color32.r:X2}{color32.g:X2}{color32.b:X2}{color32.a:X2}";
        }
    }
}
