using System.Collections;
using System.Collections.Generic;
using BillUtils.GameObjectUtilities;
using BlockBuilder.BlockManagement;
using UnityEngine;

public class ReconstructVisual : Singleton<ReconstructVisual>
{
    public Material currentMaterial;
    public MaterialType[] materialTypes;
    [SerializeField] private Material matVisual;
    [SerializeField] private BlockConfig blockConfig;
    private float indexMat;
    protected override void Awake()
    {
        base.Awake();
        InitMaterial();
    }

    public void ChangeMaterial(BlockController blockController)
    {
        if (blockController == null || materialTypes == null || materialTypes.Length == 0)
        {
            Debug.LogError("blockController or materialTypes are invalid.");
            return;
        }

        MaterialType currentMatType = blockController.GetMatType();
        int nextIndex = -1;

        for (int i = 0; i < materialTypes.Length; i++)
        {
            if (materialTypes[i] == currentMatType)
            {
                nextIndex = (i + 1) % materialTypes.Length;
                if (nextIndex == materialTypes.Length - 1) nextIndex = 0;
                break;
            }
        }

        if (nextIndex != -1)
        {
            blockController.SetMatType(materialTypes[nextIndex]);
            VFXManager.Instance.TriggerExplo(blockController.GetCenter());

            SetUpMaterialBlock(blockController.gameObject, materialTypes[nextIndex]);
        }
        else
        {
            Debug.LogWarning($"Material type {currentMatType} not found in materialTypes array.");
        }
    }


    public void InitMaterial()
    {
        materialTypes = (MaterialType[])System.Enum.GetValues(typeof(MaterialType));
    }

    public void SetUpMaterialBlock(GameObject block, MaterialType mat)
    {
        currentMaterial = blockConfig.GetMaterial(mat);
        GameObjectUtils.SetAllMaterials(block, currentMaterial);
    }


}
