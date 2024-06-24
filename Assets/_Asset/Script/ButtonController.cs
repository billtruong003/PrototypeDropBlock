using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BillUtils.TimeUtilities;
using AnimationController.WithTransform;
using BillUtils.GameObjectUtilities;
using BlockBuilder.BlockManagement;
public class ButtonController : MonoBehaviour
{
    // [SerializeField] private Animator anim;
    [SerializeField] private BlockController blockController;

    public void DestroyBuilding()
    {
        AkSoundEngine.PostEvent("Play_sx_game_int_Block_Destroy", gameObject); //### OPTIMIZE: Tristan
        VFXManager.Instance.TriggerExplo(blockController.GetCenter());
        // OPTIMIZE: Optimize later with object pool
        // Anim.DOTriggerExplosion(blockController.GetTotalCube(), blockController.GetCenter());
        GameObjectUtils.DestroyObject(blockController.gameObject);
        StartCoroutine(Cor_Done());
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

    public void ChangeMaterial(int indexMat = 0)
    {
        if (blockController == null)
        {
            Debug.LogError("No blockController assigned to change material.");
            return;
        }

        Debug.Log("ChangeMaterial");
        ReconstructVisual.Instance.ChangeMaterial(blockController, indexMat);
    }

    public void SetSaveBlockMat(MaterialType matType)
    {
        ReconstructVisual.Instance.SetSaveMaterial(matType);
    }

    public MaterialType GetCurrentMatType()
    {
        return blockController.GetMatType();
    }

    public void Done()
    {
        Debug.Log("CloseUI button clicked");
        ReconstructSystem.Instance.SetBackPosition();
        StartCoroutine(Cor_Done());
    }

    private IEnumerator Cor_Done()
    {
        // anim.SetTrigger("Close");
        yield return TimeUtils.WaitHalfSec;
        Debug.Log("Triggering HideUI");
        AkSoundEngine.PostEvent("Play_sx_game_ui_Controller_PressDownInput", gameObject); // OPTIMIZE
        ReconstructSystem.Instance.HideUI();
    }
}