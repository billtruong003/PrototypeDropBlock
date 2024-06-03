using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.UI;
using TMPro;
using BillUtils.SerializeCustom;
using BlockBuilder.BlockManagement;
using BillUtils.EnumUtilities;

public class CheatUIManager : MonoBehaviour
{
    [BillHeader("Cheat", 20, "#EE4E4E")]
    [SerializeField] private TMP_Dropdown dropDownBlock;
    [SerializeField] private TMP_Dropdown dropDownMaterial;
    [SerializeField] private Image btnBlock;
    [SerializeField] private Image btnMaterial;
    [SerializeField] private Color inactiveCol;
    [SerializeField] private Color activeCol;
    private bool statusValue;
    private GameObject blockAssign;
    private MaterialType selectedMaterial;

    [Button]
    private void InitCheat()
    {
        AssignDropMaterial();
        AssignDropBlock();
    }
    public void SwapCheatBlock()
    {
        if (SpawnManager.Instance == null)
            return;

        blockAssign = SpawnManager.Instance.GetDropBlock(dropDownBlock.value);
        SpawnManager.Instance.SetCheatBlock(blockAssign);
    }

    public void SwapCheatMaterial()
    {
        if (SpawnManager.Instance == null)
            return;

        selectedMaterial = EnumUtility.GetEnumValueAtIndex<MaterialType>(dropDownMaterial.value);
        SpawnManager.Instance.SetCheatMaterial(selectedMaterial);
    }

    public void SetCheatButtonBlock() // BTN
    {
        statusValue = SpawnManager.Instance.GetStatusCheatBlock();
        statusValue = SwapMode(statusValue);
        SpawnManager.Instance.SetModeCheatBlock(statusValue);
        btnBlock.color = SwapColor(statusValue);
    }

    public void SetCheatButtonMaterial() // BTN
    {
        statusValue = SpawnManager.Instance.GetStatusCheatMaterial();
        statusValue = SwapMode(statusValue);
        SpawnManager.Instance.SetModeCheatMaterial(statusValue);
        btnMaterial.color = SwapColor(statusValue);
    }

    private Color SwapColor(bool value)
    {
        if (value)
            return activeCol;
        return inactiveCol;
    }

    private bool SwapMode(bool value)
    {
        if (!value)
            return true;
        return false;
    }

    private void AssignDropMaterial()
    {
        if (dropDownMaterial == null)
        {
            Debug.LogError("Material Dropdown is not assigned.");
            return;
        }

        dropDownMaterial.ClearOptions();

        string[] materialNames = Enum.GetNames(typeof(MaterialType));

        List<string> options = new List<string>(materialNames);

        dropDownMaterial.AddOptions(options);
    }

    private void AssignDropBlock()
    {
        if (dropDownBlock == null)
        {
            Debug.LogError("Material Dropdown is not assigned.");
            return;
        }

        // Clear existing options
        dropDownBlock.ClearOptions();

        // Get all enum names
        string[] materialNames = Enum.GetNames(typeof(BlockShape));

        // Convert string array to List<string>
        List<string> options = new List<string>(materialNames);

        // Add options to the dropdown
        dropDownBlock.AddOptions(options);
    }

}
