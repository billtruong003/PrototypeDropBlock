using BillUtils.SerializeCustom;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [BillHeader("Sound ID", 15, "#667BC6")]
    [SerializeField] private List<SoundID> soundID;
    [SerializeField] private string blockDestroy = "Play_sx_game_int_Block_Destroy";
    [SerializeField] private string blockGoingDown = "Play_sx_game_int_Block_GoingDown";
    [SerializeField] private string blockRotate = "Play_sx_game_int_Block_Rotate";
    [SerializeField] private string blockTransform = "Play_sx_game_int_Block_Transform";
    [SerializeField] private string pressDownInput = "Play_sx_game_ui_Controller_PressDownInput";
    [SerializeField] private string menuNavigation = "Play_sx_uni_ui_Controller_MenuNavigation";
    //public void PlaySound(SoundID soundInfo)
    //{
    //}
    protected override void Awake()
    {
        base.Awake();
    }
    public void PlaySound(string soundId)
    {
        AkSoundEngine.PostEvent(soundId, gameObject);
    }
    
    public void PlaySoundControllerPressDownInput()
    {
        AkSoundEngine.PostEvent(pressDownInput, gameObject);
    }
    [Serializable]
    public class SoundID
    {
        public string soundID;
        public SoundType soundType;
    }
    public enum SoundType
    {
        DEFAULT,
        MOVE,
        ROTATE
    }
}
