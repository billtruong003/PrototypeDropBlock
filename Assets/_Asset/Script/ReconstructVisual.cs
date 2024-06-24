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
    [SerializeField] private GameObject rotateDialogue;
    [SerializeField] private GameObject moveDialogue;
    [SerializeField] private GameObject destroyDialogue;
    private float indexMat;
    protected override void Awake()
    {
        base.Awake();
        InitMaterial();
    }

    public void ChangeMaterial(BlockController blockController, int indexMat = 0)
    {
        if (blockController == null || materialTypes == null || materialTypes.Length == 0)
        {
            return;
        }

        MaterialType currentMatType = blockController.GetMatType();
        int nextIndex = -1;

        for (int i = 0; i < materialTypes.Length; i++)
        {
            if (materialTypes[i] == currentMatType)
            {
                nextIndex = i + indexMat;
                if (nextIndex == materialTypes.Length)
                    nextIndex = 0;
                else if (nextIndex < 0)
                    nextIndex = materialTypes.Length - 1;
                break;
            }
        }

        if (nextIndex != -1)
        {
            Debug.Log($"Index: {nextIndex}");
            blockController.SetMatType(materialTypes[nextIndex]);
            // VFXManager.Instance.TriggerExplo(blockController.GetCenter());

            SetUpMaterialBlock(blockController.gameObject, materialTypes[nextIndex]);
        }
        else
        {
            Debug.LogWarning($"Material type {currentMatType} not found in materialTypes array.");
        }
    }

    public void SetSaveMaterial(MaterialType materialType)
    {
        BlockController blockController = ReconstructSystem.Instance.GetBlockPick();
        blockController.SetMatType(materialType);
        SetUpMaterialBlock(blockController.gameObject, materialType);
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

    public void MoveDialogueDisplay(bool status)
    {
        moveDialogue.SetActive(status);
    }
    public void RotateDialogueDisplay(bool status)
    {
        rotateDialogue.SetActive(status);
    }
    public void DestroyDialogueDisplay(bool status)
    {
        destroyDialogue.SetActive(status);
    }
}
