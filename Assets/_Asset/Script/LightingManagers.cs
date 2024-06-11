using UnityEngine;
using System;
using System.Globalization;
using NaughtyAttributes;
using BillUtils.SerializeCustom;

public class LightingManager : MonoBehaviour
{
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject moon;
    [SerializeField] private Light directionalLight;
    [SerializeField] private Light nightLight;
    [SerializeField] private LightingPreset preset;
    [SerializeField, Range(0, 24)] private float timeOfDay;

    private Material currentSkybox;
    private const float SkyBoxMorning = 0.6f;
    private const float SkyBoxNight = 0.3f;
    private float currentExposure;
    private float exposureVelocity = 0.0f;
    [SerializeField, Range(0.01f, 1.0f)] private float transitionSpeed = 0.1f;

    private SunriseSunsetService sunriseSunsetService;
    private DateTime sunriseTime = DateTime.MinValue;
    private DateTime sunsetTime = DateTime.MinValue;
    private TimeSpan utcOffset;

    [BillHeader("Cheat", 20, "#EE4E4E")]
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private bool cheatProgress;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField, Range(0, 24)] private float cheatTime;

    [BillHeader("CheatLocate", 20, "#EE4E4E")]
    [BoxGroup("CheatLocate", "#8B322C", 0.5f)]
    [SerializeField] private bool cheatLocate;
    [BoxGroup("CheatLocate", "#8B322C", 0.5f)]
    [SerializeField] private LocationData locationData;
    [BoxGroup("CheatLocate", "#8B322C", 0.5f)]
    [SerializeField] private Locate locate;

    private void Awake()
    {
        Initialize();
        RequestSunriseSunsetData();
    }

    private void Initialize()
    {
        currentSkybox = RenderSettings.skybox;
        currentExposure = SkyBoxMorning;
        sunriseSunsetService = GetComponent<SunriseSunsetService>();
        sunriseSunsetService.OnSunriseSunsetDataReceived += OnSunriseSunsetDataReceived;
    }

    private void RequestSunriseSunsetData()
    {
        Vector2 latlon = locationData.GetLocation(locate);
        float lat = latlon.x;
        float lon = latlon.y;

        if (!cheatLocate)
            sunriseSunsetService.GetSunriseSunsetTime(21.028511f, 105.804817f);
        else
            sunriseSunsetService.GetSunriseSunsetTime(lat, lon);
    }

    private void OnSunriseSunsetDataReceived(SunriseSunsetResponse.Results results)
    {
        try
        {
            DateTime utcSunrise = ParseTime(results.sunrise);
            DateTime utcSunset = ParseTime(results.sunset);

            TimeZoneInfo localTimeZone = TimeZoneInfo.Local;

            sunriseTime = ConvertToLocalTime(utcSunrise, localTimeZone);
            sunsetTime = ConvertToLocalTime(utcSunset, localTimeZone);
            utcOffset = localTimeZone.GetUtcOffset(DateTime.UtcNow);

            Debug.Log($"Sunrise at: {sunriseTime}, Sunset at: {sunsetTime}, Local Timezone: {localTimeZone.DisplayName}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error parsing sunrise/sunset data: {ex.Message}");
        }
    }

    private DateTime ParseTime(string time)
    {
        return DateTime.Parse(time, null, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);
    }

    private DateTime ConvertToLocalTime(DateTime utcTime, TimeZoneInfo localTimeZone)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, localTimeZone);
    }

    private void Update()
    {
        if (IsPresetInvalid() || IsSunriseSunsetTimeInvalid())
            return;

        UpdateLighting();
    }

    private bool IsPresetInvalid()
    {
        return preset == null;
    }

    private bool IsSunriseSunsetTimeInvalid()
    {
        return sunriseTime == DateTime.MinValue || sunsetTime == DateTime.MinValue;
    }

    private void UpdateLighting()
    {
        DateTime currentTime = GetLocalCurrentTime();
        SetTimeOfDay(currentTime);

        float dayProgress = CalculateDayProgress(currentTime);

        UpdateAmbientAndFog(dayProgress);
        UpdateDirectionalLight(dayProgress);
        UpdateSkyboxExposure(dayProgress);
        ToggleSunAndMoon(dayProgress);
    }

    private DateTime GetLocalCurrentTime()
    {
        return DateTime.UtcNow + utcOffset;
    }

    private void SetTimeOfDay(DateTime currentTime)
    {
        timeOfDay = cheatProgress ? cheatTime : GetRealTimeOfDay(currentTime);
    }

    private float GetRealTimeOfDay(DateTime currentTime)
    {
        return currentTime.Hour + currentTime.Minute / 60f;
    }

    private void UpdateAmbientAndFog(float dayProgress)
    {
        RenderSettings.ambientLight = preset.AmbientColor.Evaluate(dayProgress);
        RenderSettings.fogColor = preset.FogColor.Evaluate(dayProgress);
    }

    private void UpdateDirectionalLight(float dayProgress)
    {
        if (directionalLight == null) return;

        directionalLight.color = preset.DirectionalColor.Evaluate(dayProgress);

        float sunAngle = dayProgress * 360f - 90f;
        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3(sunAngle, 170f, 0f));
    }

    private void UpdateSkyboxExposure(float dayProgress)
    {
        float targetExposure = GetTargetExposure(dayProgress);
        currentExposure = Mathf.Lerp(currentExposure, targetExposure, transitionSpeed * Time.deltaTime);
        currentExposure = Mathf.Clamp(currentExposure, SkyBoxNight, SkyBoxMorning);

        SetSkyboxExposure(currentExposure);
    }

    private float GetTargetExposure(float dayProgress)
    {
        return dayProgress >= 0f && dayProgress < 0.5f ? SkyBoxMorning : SkyBoxNight;
    }

    private void SetSkyboxExposure(float exposure)
    {
        if (currentSkybox != null && currentSkybox.HasProperty("_Exposure"))
        {
            currentSkybox.SetFloat("_Exposure", exposure);
        }
    }

    private void ToggleSunAndMoon(float dayProgress)
    {
        bool isDay = dayProgress >= 0f && dayProgress < 0.5f;

        sun.SetActive(isDay);
        moon.SetActive(!isDay);
        SetLighting(isDay);
    }

    private void SetLighting(bool isDay)
    {
        if (directionalLight != null)
        {
            directionalLight.enabled = isDay;
        }
        if (nightLight != null)
        {
            nightLight.enabled = !isDay;
        }
    }

    private float CalculateDayProgress(DateTime currentTime)
    {
        TimeSpan totalDayLength = sunsetTime - sunriseTime;
        TimeSpan currentTimeFromSunrise = currentTime - sunriseTime;

        if (currentTimeFromSunrise < TimeSpan.Zero)
        {
            currentTimeFromSunrise += TimeSpan.FromDays(1);
        }

        return (float)(currentTimeFromSunrise.TotalHours / totalDayLength.TotalHours);
    }

    private void OnValidate()
    {
        if (directionalLight != null)
            return;

        FindAndAssignDirectionalLight();
    }

    private void FindAndAssignDirectionalLight()
    {
        if (RenderSettings.sun != null)
        {
            directionalLight = RenderSettings.sun;
        }
        else
        {
            AssignFirstDirectionalLight();
        }
    }

    private void AssignFirstDirectionalLight()
    {
        Light[] lights = GameObject.FindObjectsOfType<Light>();
        foreach (Light light in lights)
        {
            if (light.type == LightType.Directional)
            {
                directionalLight = light;
                return;
            }
        }
    }
}
