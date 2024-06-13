using UnityEngine;
using System;
using System.Globalization;
using NaughtyAttributes;
using BillUtils.SerializeCustom;
using BillUtils.TimeUtilities;
using BillUtils.GlobalTimeUtils;

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

    private float lat;
    private float lon;
    public void SetLat(float lat) => this.lat = lat;
    public void SetLon(float lon) => this.lon = lon;
    private SunriseSunsetService sunriseSunsetService;
    private DateTime sunriseTime = DateTime.MinValue;
    private DateTime sunsetTime = DateTime.MinValue;
    private TimeSpan utcOffset;
    private string timeZoneId;

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

    private TimeZoneInfo locationTimeZone;

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
        if (!cheatLocate)
        {
            GetSSTime(lat, lon);
        }
        else
        {
            Vector2 latlon = locationData.GetLocation(locate);
            lat = latlon.x;
            lon = latlon.y;
            GetSSTime(lat, lon);
        }
    }

    private void GetSSTime(float lat, float lon)
    {
        sunriseSunsetService.GetSunriseSunsetTime(lat, lon);
        locationTimeZone = GetTimeZoneByCoordinates(lat, lon);
    }

    private void OnSunriseSunsetDataReceived(SunriseSunsetResponse.Results results)
    {
        try
        {
            DateTime utcSunrise = ParseTime(results.sunrise);
            DateTime utcSunset = ParseTime(results.sunset);

            sunriseTime = utcSunrise;
            sunsetTime = utcSunset;
            utcOffset = locationTimeZone.GetUtcOffset(DateTime.UtcNow);
            timeZoneId = results.timezone;
            Debug.Log($"Sunrise at: {sunriseTime}, Sunset at: {sunsetTime}, Timezone: {locationTimeZone.DisplayName}");
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

    private DateTime ConvertToLocalTime(DateTime utcTime, TimeZoneInfo timeZone)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime, timeZone);
    }

    private TimeZoneInfo GetTimeZoneByCoordinates(float latitude, float longitude)
    {
        return TimeZoneInfo.Local;
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
        DateTime currentTime = GetLocationCurrentTime();
        SetTimeOfDay(currentTime);

        float dayProgress = CalculateDayProgress(currentTime);
        UpdateAmbientAndFog(dayProgress);
        UpdateDirectionalLight(dayProgress);
        UpdateSkyboxExposure();
        ToggleSunAndMoon();
    }

    private bool IsValidTimeZoneId(string timeZoneId)
    {
        try
        {
            TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            return true;
        }
        catch
        {
            return false;
        }
    }


    private DateTime GetLocationCurrentTime()
    {
        return GlobalTimeUtils.GetCurrentTimeAtTimeZone(timeZoneId);
    }


    private void SetTimeOfDay(DateTime currentTime)
    {
        if (!cheatProgress)
        {
            timeOfDay = GetRealTimeOfDay(currentTime);
        }
        else
        {
            timeOfDay = cheatTime;
        }
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

    private void UpdateSkyboxExposure()
    {
        DateTime currentTime = GetLocationCurrentTime();
        float targetExposure = GetTargetExposure(currentTime);
        currentExposure = Mathf.Lerp(currentExposure, targetExposure, transitionSpeed * Time.deltaTime);
        currentExposure = Mathf.Clamp(currentExposure, SkyBoxNight, SkyBoxMorning);

        SetSkyboxExposure(currentExposure);
        Debug.Log($"Current Time: {currentTime}, Target Exposure: {targetExposure}, Current Exposure {currentExposure}");
    }

    private float GetTargetExposure(DateTime currentTime)
    {
        TimeSpan transitionDuration = TimeSpan.FromHours(1);

        DateTime transitionStartMorning = sunriseTime - transitionDuration;
        DateTime transitionEndMorning = sunriseTime + transitionDuration;
        DateTime transitionStartEvening = sunsetTime - transitionDuration;
        DateTime transitionEndEvening = sunsetTime + transitionDuration;

        if (currentTime >= transitionStartMorning && currentTime <= transitionEndMorning)
        {
            float t = (float)(currentTime - transitionStartMorning).TotalMinutes / (float)transitionDuration.TotalMinutes;
            return Mathf.Lerp(SkyBoxNight, SkyBoxMorning, t);
        }
        else if (currentTime >= transitionStartEvening && currentTime <= transitionEndEvening)
        {
            float t = (float)(currentTime - transitionStartEvening).TotalMinutes / (float)transitionDuration.TotalMinutes;
            return Mathf.Lerp(SkyBoxMorning, SkyBoxNight, t);
        }
        else if (currentTime > sunriseTime && currentTime < sunsetTime)
        {
            return SkyBoxMorning;
        }
        else
        {
            return SkyBoxNight;
        }
    }

    private void SetSkyboxExposure(float exposure)
    {
        if (currentSkybox != null && currentSkybox.HasProperty("_Exposure"))
        {
            currentSkybox.SetFloat("_Exposure", exposure);
        }
    }

    private void ToggleSunAndMoon()
    {
        DateTime currentTime = GetLocationCurrentTime();
        TimeSpan transitionDuration = TimeSpan.FromMinutes(30);

        DateTime startDayTransition = sunriseTime - transitionDuration;
        DateTime endDayTransition = sunsetTime + transitionDuration;

        bool isNight = currentTime < startDayTransition || currentTime >= endDayTransition;

        sun.SetActive(!isNight);
        moon.SetActive(isNight);
        SetLighting(!isNight);
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
