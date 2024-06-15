using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BillUtils.TimeUtilities;
using AnimationController.WithTransform;
public class ButtonController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private BlockController blockController;
    [SerializeField] private ReconstructVisual reconstructVisual;

    public void DestroyBuilding()
    {
        VFXManager.Instance.TriggerExplo(blockController.GetCenter());
        Anim.DOTriggerExplosion(blockController.GetTotalCube(), blockController.GetCenter());
        CloseBtn();
        Debug.Log("Destroy");
    }

    public void AddBlockController(BlockController blockController)
    {
        this.blockController = blockController;
    }

    public void Rotate()
    {
        ReconstructSystem.Instance.RotateBlock();
    }

    public void ChangeMaterial()
    {
        if (blockController == null)
        {
            Debug.LogError("No blockController assigned to change material.");
            return;
        }

        Debug.Log("ChangeMaterial");
        ReconstructVisual.Instance.ChangeMaterial(blockController);
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