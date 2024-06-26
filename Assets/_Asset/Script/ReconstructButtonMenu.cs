using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BillUtils.EnumUtilities;
using BillUtils.TimeUtilities;
using BlockBuilder.BlockManagement;
using UnityEngine;
using UnityEngine.UI;

public class ReconstructButtonMenu : MonoBehaviour
{
    [SerializeField] private Options currentOption;
    [SerializeField] private OptionStates currentOptionState;
    [SerializeField] private MaterialType saveMat;

    // UI Elements
    [SerializeField] private bool TurnUIOn;
    [SerializeField] private ButtonController btnController;
    [SerializeField] private Button currentButton;
    [SerializeField] private GameObject materialDefault;
    [SerializeField] private GameObject materialChangeMode;
    [SerializeField] private Image matChangeIMG;
    [SerializeField] private List<Button> buttons;
    [SerializeField] private List<MaterialInfoUI> MaterialUI;

    // Timing Parameters
    private int lengthOptions = 0;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        DisplayUISelected();
        if (Input.GetKeyDown(KeyCode.I))
        {
            SoundManager.Instance.PlaySound(SoundType.S_NAVIGATION);
            PrevButton();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            SoundManager.Instance.PlaySound(SoundType.S_NAVIGATION);
            NextButton();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {

            Choose();
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            Done();
        }

        if (currentOptionState == OptionStates.ChangeOn)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                SoundManager.Instance.PlaySound(SoundType.S_NAVIGATION);
                NextMat();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                SoundManager.Instance.PlaySound(SoundType.S_NAVIGATION);
                PrevMat();
            }
        }
    }
    public void Init()
    {
        lengthOptions = EnumUtils.GetEnumCount<Options>();
        currentOption = Options.ROTATE;
        UpdateSelectedBtn();
        InitMat();
        saveMat = btnController.GetCurrentMatType();
    }

    private void UpdateSelectedBtn(int buttonIndex = 0)
    {
        if (currentOptionState != OptionStates.DEFAULT)
            return;

        int index = EnumUtils.GetEnumIndex<Options>(currentOption);
        index = index + buttonIndex;
        if (index >= lengthOptions)
        {
            currentButton = buttons[0];
            currentOption = EnumUtils.GetEnumValueAtIndex<Options>(0);
        }
        else if (index < 0)
        {
            currentButton = buttons[lengthOptions - 1];
            currentOption = EnumUtils.GetEnumValueAtIndex<Options>(lengthOptions - 1);
        }
        else
        {
            currentButton = buttons[index];
            currentOption = EnumUtils.GetEnumValueAtIndex<Options>(index);
        }

    }
    public void DisplayUISelected()
    {
        currentButton.Select();
    }

    public void NextButton()
    {
        UpdateSelectedBtn(1);
    }

    public void PrevButton()
    {
        UpdateSelectedBtn(-1);
    }

    public void SelectBtn()
    {
        currentButton.Select();
    }

    public void Choose()
    {
        switch (currentOption)
        {
            case Options.ROTATE:
                if (currentOptionState != OptionStates.DEFAULT)
                    return;

                Rotate();
                break;

            case Options.MOVE:
                SoundManager.Instance.PlaySound(SoundType.S_PRESSDOWN);
                if (currentOptionState != OptionStates.DEFAULT)
                {
                    if (currentOptionState == OptionStates.MoveOn)
                        currentOptionState = OptionStates.DEFAULT;
                    return;
                }

                if (ReconstructSystem.Instance.NotPossibleToControl)
                {
                    MoveDialogueDisplay();
                    return;
                }
                Move();
                break;

            case Options.CHANGE:
                //### OPTIMIZE: Tristan
                if (currentOptionState != OptionStates.DEFAULT)
                {
                    if (currentOptionState == OptionStates.ChangeOn)
                    {
                        SoundManager.Instance.PlaySound(SoundType.S_PRESSDOWN);
                        SwitchChangeMatMode();
                    }
                    return;
                }
                SoundManager.Instance.PlaySound(SoundType.S_PRESSDOWN);
                SwitchChangeMatMode();
                break;

            case Options.DESTROY:
                if (currentOptionState != OptionStates.DEFAULT)
                {
                    if (currentOptionState == OptionStates.DestroyOn)
                    {
                        currentOptionState = OptionStates.DEFAULT;
                        Destroy();
                    }
                    return;
                }
                if (ReconstructSystem.Instance.NotPossibleToControl)
                {
                    DestroyDialogueDisplay();
                    return;
                }
                Destroy();
                break;
        }
    }


    #region FunctionControl
    public void Rotate()
    {
        StartCoroutine(RotateCoroutine());
    }

    private IEnumerator RotateCoroutine()
    {
        currentOptionState = OptionStates.RotateOn;

        btnController.Rotate();
        yield return TimeUtils.WaitHalfSec;

        currentOptionState = OptionStates.DEFAULT;
    }

    private void Move()
    {
        currentOptionState = OptionStates.MoveOn;
        btnController.MoveBtn();
    }

    public void NextMat()
    {
        btnController.ChangeMaterial(1);
        UpdateMat();
    }

    public void InitMat()
    {
        UpdateMat();
    }
    public void PrevMat()
    {
        btnController.ChangeMaterial(-1);
        UpdateMat();
    }

    public void UpdateMat()
    {
        foreach (MaterialInfoUI mat in MaterialUI)
        {
            MaterialType matType = btnController.GetCurrentMatType();
            if (matType == mat.MatType)
            {
                matChangeIMG.sprite = mat.MatUI;
                return;
            }
        }
    }
    private void SwitchChangeMatMode()
    {
        switch (currentOptionState)
        {
            case OptionStates.DEFAULT:
                currentOptionState = OptionStates.ChangeOn;
                materialChangeMode.SetActive(true);
                materialDefault.SetActive(false);
                break;
            case OptionStates.ChangeOn:
                currentOptionState = OptionStates.DEFAULT;
                materialChangeMode.SetActive(false);
                materialDefault.SetActive(true);
                break;
        }
    }
    public void Destroy()
    {
        btnController.DestroyBuilding();
    }

    public void Done()
    {
        CheckStatusWhenDone();
        btnController.Done();
    }

    public void CheckStatusWhenDone()
    {
        if (currentOptionState == OptionStates.DEFAULT)
        {
            currentOptionState = OptionStates.DEFAULT;
        }
        else if (currentOptionState == OptionStates.RotateOn)
        {
            currentOptionState = OptionStates.DEFAULT;
        }
        else if (currentOptionState == OptionStates.MoveOn)
        {
            currentOptionState = OptionStates.DEFAULT;
        }
        else if (currentOptionState == OptionStates.ChangeOn)
        {
            SwitchChangeMatMode();
            currentOptionState = OptionStates.DEFAULT;
            btnController.SetSaveBlockMat(saveMat);
        }
    }

    public void DestroyDialogueDisplay()
    {
        StopAllCoroutines();
        ReconstructVisual.Instance.DestroyDialogueDisplay(false);
        StartCoroutine(Cor_DestroyDialogueDisplay());
    }

    public IEnumerator Cor_DestroyDialogueDisplay()
    {
        ReconstructVisual.Instance.DestroyDialogueDisplay(true);
        yield return TimeUtils.WaitThreeSec;
        ReconstructVisual.Instance.DestroyDialogueDisplay(false);
    }

    public void MoveDialogueDisplay()
    {
        StopAllCoroutines();
        ReconstructVisual.Instance.MoveDialogueDisplay(false);
        StartCoroutine(Cor_MoveDialogDisplay());
    }

    public IEnumerator Cor_MoveDialogDisplay()
    {
        ReconstructVisual.Instance.MoveDialogueDisplay(true);
        yield return TimeUtils.WaitThreeSec;
        ReconstructVisual.Instance.MoveDialogueDisplay(false);
    }
    #endregion


    #region Option_State
    public enum OptionStates
    {
        DEFAULT,
        RotateOn,
        MoveOn,
        ChangeOn,
        DestroyOn,
    }
    public enum Options
    {
        ROTATE,
        MOVE,
        CHANGE,
        DESTROY,
    }
    #endregion
}
