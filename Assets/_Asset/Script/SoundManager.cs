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
    [SerializeField] private string bubblePop = "Play_sx_game_int_Bubble_Pop";
    [SerializeField] private string menuNavigation = "Play_sx_uni_ui_Controller_MenuNavigation";
    [SerializeField] private string weatherSnowStart = "Play_sx_game_amb_Weather_Snow";
    [SerializeField] private string weatherSnowStop = "Stop_sx_game_amb_Weather_Snow";
    [SerializeField] private string weatherRainStart = "Play_sx_game_amb_Weather_Rain";
    [SerializeField] private string weatherRainStop = "Stop_sx_game_amb_Weather_Rain";


    private string idTrigger;
    private string idStop;
    protected override void Awake()
    {
        base.Awake();
    }

    // Example usage: SoundManager.Instance.PlaySound(SoundType.S_MOVE);

    public void PlaySound(SoundType soundType)
    {
        idTrigger = GetSoundID(soundType);
        AkSoundEngine.PostEvent(idTrigger, gameObject);
    }

    public void StopSound(SoundType soundType)
    {
        idStop = GetSoundID(soundType);
        uint evenId = AkSoundEngine.GetIDFromString(idStop);
        AkSoundEngine.ExecuteActionOnEvent(evenId, AkActionOnEventType.AkActionOnEventType_Stop, gameObject);
    }

    public void TransitionStopSound(SoundType soundType)
    {
        idStop = GetSoundID(soundType);
        AkSoundEngine.PostEvent(idStop, gameObject);
    }

    //public void SwitchState()
    //{
    //    AkSoundEngine.SetState()
    //    AkSoundEngine.SetSwitch();
    //}

    public void CheckSoundWeatherAmbient(WeatherType weatherType)
    {
        if (weatherType == WeatherType.SNOWY)
        {
            TransitionStopSound(SoundType.S_SNOW_STOP);
        }
        else if (weatherType == WeatherType.RAIN)
        {
            TransitionStopSound(SoundType.S_RAIN_STOP);
        }
        else
        {
            return;
        }
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
