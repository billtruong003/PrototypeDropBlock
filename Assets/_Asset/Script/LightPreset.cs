using NaughtyAttributes;
using UnityEngine;
using BillUtils.ColorUtilities;
using BillUtils.SerializeCustom;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "Scriptables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;

    [BillHeader("AmbientColor", 12, "#")]
    [SerializeField] private string ambientHexStart = "#D8BFD8";
    [SerializeField] private string ambientHexMiddle = "#FFD700";
    [SerializeField] private string ambientHexEnd = "#FFE4B5";

    [SerializeField] private string directionalHexStart = "#9370DB";
    [SerializeField] private string directionalHexMiddle = "#FF8C00";
    [SerializeField] private string directionalHexEnd = "#FFA07A";

    [SerializeField] private string fogHexStart = "#E6E6FA";
    [SerializeField] private string fogHexMiddle = "#B0E0E6";
    [SerializeField] private string fogHexEnd = "#87CEEB";

    [Button("Set Ambient Color")]
    public void SetAmbientColor()
    {
        AmbientColor = CreateGradient(ambientHexStart, ambientHexMiddle, ambientHexEnd);
    }

    [Button("Set Directional Color")]
    public void SetDirectionalColor()
    {
        DirectionalColor = CreateGradient(directionalHexStart, directionalHexMiddle, directionalHexEnd);
    }

    [Button("Set Fog Color")]
    public void SetFogColor()
    {
        FogColor = CreateGradient(fogHexStart, fogHexMiddle, fogHexEnd);
    }

    private Gradient CreateGradient(string hexStart, string hexMiddle, string hexEnd)
    {
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(ColorUtils.HexToColor(hexStart), 0.0f),
                new GradientColorKey(ColorUtils.HexToColor(hexMiddle), 0.5f),
                new GradientColorKey(ColorUtils.HexToColor(hexEnd), 1.0f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 0.5f),
                new GradientAlphaKey(1.0f, 1.0f)
            }
        );
        return gradient;
    }
}
