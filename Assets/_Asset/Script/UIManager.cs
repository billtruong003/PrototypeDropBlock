using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BillUtils.SerializeCustom;
using BlockBuilder.BlockManagement;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager>
{
    [BillHeader("Cam Angle", 20, "#E88D67")]
    [SerializeField] private GameObject currentCamAngle;
    [SerializeField] private List<GameObject> camAngles;
    [SerializeField] private int angle = 0;
    private GameObject camOpen;

    [BillHeader("Info Block", 20, "#E88D67")]
    [SerializeField] private TextMeshProUGUI TMP_CurrentBlock;
    [SerializeField] private TextMeshProUGUI TMP_CurrentMaterial;

    protected override void Awake()
    {
        base.Awake();
        InitFirstCam();
    }
    private void Update()
    {
        HandleKeyInput();
    }
    public void HandleKeyInput()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeAngleLeft();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeAngleRight();
        }
    }
    public void ChangeAngleRight()
    {
        angle += 1;
        if (angle > camAngles.Count - 1)
            angle = 0;
        ChangeCam(angle);
    }

    public void ChangeAngleLeft()
    {
        angle -= 1;
        if (angle < 0)
            angle = camAngles.Count - 1;
        ChangeCam(angle);
    }

    private void ChangeCam(int side)
    {
        camOpen = camAngles[side];
        camOpen.SetActive(true);
        currentCamAngle.SetActive(false);
        currentCamAngle = camOpen;
    }

    private void InitFirstCam()
    {
        currentCamAngle = camAngles[angle];
        currentCamAngle.SetActive(true);
    }

    public void SetCurrentBlock(BlockShape blockType)
    {
        TMP_CurrentBlock.text = "Block Type: " + "\n" + blockType.ToString();
    }

    public void SetCurrentMat(MaterialType matType)
    {
        TMP_CurrentMaterial.text = "Mat Type: " + "\n" + matType.ToString();
    }
}
