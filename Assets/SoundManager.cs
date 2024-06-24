using BillUtils.SerializeCustom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlockBuilder.BlockManagement;
public class SoundManager : Singleton<SoundManager>
{
    [BillHeader("Sound ID", 15, "#667BC6")]
    [SerializeField] private List<SoundID> soundIDs;
    [SerializeField] private string blockDestroy = "Play_sx_game_int_Block_Destroy";
    [SerializeField] private string blockGoingDown = "Play_sx_game_int_Block_GoingDown";
    [SerializeField] private string blockMove = "Play_sx_game_int_Block_Move";
    [SerializeField] private string blockRotate = "Play_sx_game_int_Block_Rotate";
    [SerializeField] private string blockTransform = "Play_sx_game_int_Block_Transform";
    [SerializeField] private string pressDownInput = "Play_sx_game_ui_Controller_PressDownInput";
    [SerializeField] private string menuNavigation = "Play_sx_uni_ui_Controller_MenuNavigation";

    private string idTrigger;
    protected override void Awake()
    {
        base.Awake();
    }

    public void PlaySound(SoundType soundType)
    {
        idTrigger = GetSoundID(soundType);
        AkSoundEngine.PostEvent(idTrigger, gameObject);
    }

    private string GetSoundID(SoundType soundType)
    {
        foreach (SoundID soundID in soundIDs)
        {
            if (soundID.soundType == soundType)
            {
                return soundID.soundID;
            }
        }
        return "";
    }

    public void PlaySoundControllerPressDownInput()
    {
        AkSoundEngine.PostEvent(pressDownInput, gameObject);
    }


}
