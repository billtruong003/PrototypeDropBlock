using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using NaughtyAttributes;

public class TimeZoneMapping
{
    public string Iana { get; set; }
    public string Windows { get; set; }
}

public class TimeZoneGenerator : MonoBehaviour
{
    private string jsonFilePath = "Assets/Resources/timezone_mapping.json";
    private string timeZoneDBApiKey = "PI5M6ZSB0FR3"; // Thay bằng API key của bạn từ TimeZoneDB.
    private string timeZoneApiUrl = "https://timezoneapi.io/api/iana2windows?iana={0}&token=YOUR_TIMEZONEAPI_KEY"; // Thay bằng API key của bạn từ TimeZone API.



    private async Task<List<TimeZoneMapping>> FetchAndMapTimeZonesAsync()
    {
        List<string> ianaTimeZones = await FetchIanaTimeZonesAsync();
        var timeZoneMappings = await MapToWindowsTimeZonesAsync(ianaTimeZones);
        return timeZoneMappings;
    }

    private async Task<List<string>> FetchIanaTimeZonesAsync()
    {
        string url = $"https://api.timezonedb.com/v2.1/list-time-zone?key={timeZoneDBApiKey}&format=json";
        List<string> ianaTimeZones = new List<string>();

        using (HttpClient client = new HttpClient())
        {
            string jsonResponse = await client.GetStringAsync(url);
            var response = JsonConvert.DeserializeObject<TimeZoneDBResponse>(jsonResponse);

            foreach (var zone in response.zones)
            {
                ianaTimeZones.Add(zone.zoneName);
            }
        }

        return ianaTimeZones;
    }

    private async Task<List<TimeZoneMapping>> MapToWindowsTimeZonesAsync(List<string> ianaTimeZones)
    {
        var timeZoneMappings = new List<TimeZoneMapping>();

        using (HttpClient client = new HttpClient())
        {
            foreach (var ianaTimeZone in ianaTimeZones)
            {
                string windowsTimeZone = await GetWindowsTimeZoneAsync(client, ianaTimeZone);
                if (!string.IsNullOrEmpty(windowsTimeZone))
                {
                    timeZoneMappings.Add(new TimeZoneMapping
                    {
                        Iana = ianaTimeZone,
                        Windows = windowsTimeZone
                    });
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy ánh xạ cho IANA Time Zone: {ianaTimeZone}");
                }
            }
        }

        return timeZoneMappings;
    }

    private async Task<string> GetWindowsTimeZoneAsync(HttpClient client, string ianaTimeZone)
    {
        try
        {
            string url = string.Format(timeZoneApiUrl, ianaTimeZone);
            string jsonResponse = await client.GetStringAsync(url);
            var response = JsonConvert.DeserializeObject<TimeZoneApiResponse>(jsonResponse);

            if (response?.data?.windows != null)
            {
                return response.data.windows;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Lỗi khi chuyển đổi IANA sang Windows: {ex.Message}");
        }

        return null;
    }

    private void WriteToJson(List<TimeZoneMapping> mappings, string outputPath)
    {
        try
        {
            string jsonOutput = JsonConvert.SerializeObject(mappings, Formatting.Indented);
            string directory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(outputPath, jsonOutput);
            Debug.Log($"Dữ liệu đã được ghi vào tệp: {outputPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ghi dữ liệu vào tệp JSON thất bại: {ex.Message}");
        }
    }
}

public class TimeZoneDBResponse
{
    public List<TimeZoneDBZone> zones { get; set; }
}

public class TimeZoneDBZone
{
    public string zoneName { get; set; }
}

public class TimeZoneApiResponse
{
    public TimeZoneData data { get; set; }
}

public class TimeZoneData
{
    public string windows { get; set; }
}
