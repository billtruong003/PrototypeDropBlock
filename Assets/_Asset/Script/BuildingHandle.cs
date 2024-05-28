using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHandle : MonoBehaviour
{
    [SerializeField] private GameObject brick;
    [SerializeField] private GameObject funiture;
    [SerializeField] private GameObject roof;
    [SerializeField] private GameObject window;
    [SerializeField] private List<Transform> pivots;
    [SerializeField] private Renderer roofRenderer;
    [SerializeField, Range(0, 1)] private float alpha = 0.5f;

    private void Start()
    {
        alpha = 0.5f;
        roofRenderer = GetComponentInChildren<Renderer>();
    }
    public void TurnOnRoof()
    {
        roof.SetActive(true);
    }
    public void TurnOffRoof()
    {
        roof.SetActive(false);
    }
    public void TurnOnFurniture()
    {
        funiture.SetActive(true);
    }
    public void TurnOffFurniture()
    {
        funiture.SetActive(false);
    }

    public void SetMaterialAlpha(float alpha)
    {
        // Tạo một instance riêng của material
        Material material = roofRenderer.material;

        // Kiểm tra nếu material có thuộc tính _Color
        if (material.HasProperty("_Color"))
        {
            // Lấy màu hiện tại
            Color color = material.color;

            // Thiết lập giá trị alpha mới
            color.a = alpha;

            // Gán lại màu mới cho material
            material.color = color;
        }
        else if (material.HasProperty("_BaseColor"))
        {
            // Nếu sử dụng HDRP hoặc URP với shader có thuộc tính _BaseColor
            Color color = material.GetColor("_BaseColor");
            color.a = alpha;
            material.SetColor("_BaseColor", color);
        }

        // Thiết lập chế độ blend để hỗ trợ alpha
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
    public void SetRoofTransparent()
    {
        SetMaterialAlpha(alpha);
    }
    public void ResetMaterialAlpha()
    {
        SetMaterialAlpha(1.0f);
    }

}
