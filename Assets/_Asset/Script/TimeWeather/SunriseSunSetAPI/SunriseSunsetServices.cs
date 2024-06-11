using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class SunriseSunsetService : MonoBehaviour
{
    private string apiUrl = "https://api.sunrisesunset.io/json";
    public Action<SunriseSunsetResponse.Results> OnSunriseSunsetDataReceived;

    public void GetSunriseSunsetTime(float latitude, float longitude, string date = "today")
    {
        StartCoroutine(GetSunriseSunsetTimeCoroutine(latitude, longitude, date));
    }

    private IEnumerator GetSunriseSunsetTimeCoroutine(float latitude, float longitude, string date)
    {
        string url = $"{apiUrl}?lat={latitude}&lng={longitude}&date={date}&formatted=0";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            ProcessSunriseSunsetData(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error: " + request.error);
        }
    }

    private void ProcessSunriseSunsetData(string jsonData)
    {
        var json = JsonUtility.FromJson<SunriseSunsetResponse>(jsonData);
        OnSunriseSunsetDataReceived?.Invoke(json.results);

        Debug.Log("Date: " + json.results.date);
        Debug.Log("Sunrise: " + json.results.sunrise);
        Debug.Log("Sunset: " + json.results.sunset);
        Debug.Log("Timezone: " + json.results.timezone);
        Debug.Log("UTC Offset: " + json.results.utc_offset);
    }
}

