using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BillUtils.TimeUtilities;

public class ButtonController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private BlockController blockController;

    public void DestroyBuilding()
    {
        Debug.Log("Destroy");
    }

    public void Rotate()
    {
        ReconstructSystem.Instance.RotateBlock();
    }

    public void Clean()
    {
        Debug.Log("Clean");
    }

    public void CloseBtn()
    {
        Debug.Log("CloseUI button clicked");
        StartCoroutine(CloseBtnCoroutine());
    }

    private IEnumerator CloseBtnCoroutine()
    {
        anim.SetTrigger("Close");
        yield return TimeUtils.WaitHalfSec;
        Debug.Log("Triggering HideUI");
        ReconstructSystem.Instance.HideUI();
    }
}