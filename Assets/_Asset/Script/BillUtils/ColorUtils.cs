using UnityEngine;

namespace BillUtils.ColorUtilities
{
    public static class ColorUtils
    {
        /// <summary>
        /// Converts a hex string to a Unity Color object.
        /// </summary>
        /// <param name="hex">The hex string representing the color.</param>
        /// <returns>A Color object representing the hex string.</returns>
        public static Color HexToColor(string hex)
        {
            if (ColorUtility.TryParseHtmlString(hex, out Color color))
            {
                return color;
            }
            return Color.white;
        }
    }
}
