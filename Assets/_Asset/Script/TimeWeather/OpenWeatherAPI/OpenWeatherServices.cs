using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using BillUtils.SerializeCustom;
using NaughtyAttributes;
using BlockBuilder.BlockManagement;
public class OpenWeatherServices : MonoBehaviour
{
    [SerializeField] private string apiKey = "5f971192b5447c21d69a957edadef2c8"; // Sử dụng API key của bạn

    // FIXME: Test case
    // Tokyo, Nhật Bản
    // Latitude: 35.682839f
    // Longitude: 139.759455f 

    // New York, Mỹ
    // Latitude: 40.712776f
    // Longitude: -74.005974f

    // Hà Nội, Việt Nam
    // Latitude: 21.028511f
    // Longitude: 105.804817f
    [SerializeField] private float latitude = 35.682839f; // Vĩ độ (latitude)
    [SerializeField] private float longitude = 139.759455f; // Kinh độ (longitude)
    private string apiURL = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric";
    [SerializeField] private List<WeatherDisplay> weatherInfo;
    [SerializeField] private float SecondRepeat;

    [BillHeader("Cheat", 20, "#EE4E4E")]

    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private bool cheatWeather;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private WeatherType weatherType;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private bool cheatLocate;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private LocationData locationData;
    [BoxGroup("Cheat", "#8B322C", 0.5f)]
    [SerializeField] private Locate locate;
    private WeatherType currentWeather;

    [Button]
    private void ReTriggerWeather()
    {
        RefreshWeather();
    }
    void Start()
    {

        InvokeRepeating(nameof(RefreshWeather), 0, SecondRepeat); // 900 giây = 15 phút
    }

    void RefreshWeather()
    {
        StartCoroutine(GetWeather());
    }

    IEnumerator GetWeather()
    {
        if (cheatLocate)
        {
            Vector2 lonlat = locationData.GetLocation(locate);
            latitude = lonlat.x;
            longitude = lonlat.y;
        }
        string url = string.Format(apiURL, latitude, longitude, apiKey);
        Debug.Log("Request URL: " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Weather data: " + request.downloadHandler.text);
            ProcessWeatherData(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
            Debug.LogError("Status Code: " + request.responseCode);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
    }

    void ProcessWeatherData(string jsonData)
    {
        WeatherData weatherData = JsonUtility.FromJson<WeatherData>(jsonData);

        Debug.Log("City: " + weatherData.name);
        Debug.Log("Temperature: " + weatherData.main.temp + "°C");
        Debug.Log("Pressure: " + weatherData.main.pressure + " hPa");
        Debug.Log("Humidity: " + weatherData.main.humidity + "%");
        Debug.Log("Wind Speed: " + weatherData.wind.speed + " m/s");

        if (currentWeather != WeatherType.DEFAULT)
        {
            GetWeatherObj(currentWeather).DeactiveWeather();
            SoundManager.Instance.CheckSoundWeatherAmbient(currentWeather);
        }
        currentWeather = DetermineWeatherType(weatherData.weather);
        GetWeatherObj(currentWeather).TriggerWeather();
        Debug.Log("Current Weather: " + currentWeather);
    }

    WeatherType DetermineWeatherType(WeatherData.Weather[] weatherArray)
    {
        if (cheatWeather)
            return weatherType;
        foreach (var weather in weatherArray)
        {
            switch (weather.main.ToLower())
            {
                case "rain":
                case "drizzle":
                case "thunderstorm":
                    return WeatherType.RAIN;
                case "snow":
                case "sleet":
                    return WeatherType.SNOWY;
                case "clouds":
                case "clear":
                    return WeatherType.DEFAULT;
                case "mist":
                case "fog":
                case "haze":
                case "smoke":
                case "dust":
                case "sand":
                case "ash":
                case "squall":
                case "tornado":
                case "wind":
                    return WeatherType.WINDY;

                default:
                    return WeatherType.DEFAULT;
            }
        }
        return WeatherType.DEFAULT;
    }

    private WeatherDisplay GetWeatherObj(WeatherType weatherType)
    {
        foreach (var item in weatherInfo)
        {
            if (item.weatherType == weatherType)
                return item;
        }
        return weatherInfo[0];
    }
}