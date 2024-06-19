using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BillUtils.TimeUtilities;
using AnimationController.WithTransform;
using BillUtils.GameObjectUtilities;
public class ButtonController : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private BlockController blockController;
    [SerializeField] private ReconstructVisual reconstructVisual;

    public void DestroyBuilding()
    {
        VFXManager.Instance.TriggerExplo(blockController.GetCenter());
        // OPTIMIZE: Optimize later with object pool
        // Anim.DOTriggerExplosion(blockController.GetTotalCube(), blockController.GetCenter());
        GameObjectUtils.DestroyObject(blockController.gameObject);
        StartCoroutine(CloseBtnCoroutine());
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

    public void MoveBtn()
    {
        ReconstructSystem.Instance.MoveBlock();
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
        ReconstructSystem.Instance.SetBackPosition();
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