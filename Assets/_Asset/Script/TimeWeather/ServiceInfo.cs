
using System;
using UnityEngine;
using System.Globalization;
using BillUtils.GlobalTimeUtils;
using BlockBuilder.BlockManagement;

[System.Serializable]
public class SunriseSunsetResponse
{
    public Results results;

    [System.Serializable]
    public class Results
    {
        public string date;
        public string sunrise;
        public string sunset;
        public string timezone;
        public DateTime currentTime; // Thêm thông tin về giờ hiện tại
        public int utc_offset;

        public void GetCorrectSunriseSunsetTimeAtTimeZone()
        {
            GlobalTimeUtils.GetCurrentTimeAtTimeZone(timezone);
        }


    }
}


[System.Serializable]
public class WeatherData
{
    public string name;
    public Main main;
    public Wind wind;
    public Weather[] weather;

    [System.Serializable]
    public class Main
    {
        public float temp;
        public float pressure;
        public float humidity;
    }

    [System.Serializable]
    public class Wind
    {
        public float speed;
    }

    [System.Serializable]
    public class Weather
    {
        public string main;
    }
}

public enum WeatherType
{
    DEFAULT, // Không có gì cả chỉ bình thường thôi
    RAIN,    // Trời mưa
    WINDY,   // Trời gió nhẹ
    SNOWY    // Trời có tuyết
}
[Serializable]
public class WeatherDisplay
{
    public WeatherType weatherType;
    public GameObject weatherObject;
    public SoundType soundType;

    public void TriggerWeather()
    {
        if (weatherObject == null)
            return;
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySound(soundType);
        weatherObject.SetActive(true);
    }
    public void DeactiveWeather()
    {
        if (weatherObject == null)
            return;
        weatherObject.SetActive(false);
    }
}
[Serializable]
public class Location
{
    public string locationName;
    public Locate locate;
    public float latitude;
    public float longitude;

    public Location(string name, Locate country, float lat, float lon)
    {
        locationName = name;
        this.locate = country;
        latitude = lat;
        longitude = lon;
    }
}



public enum Locate
{
    // Japan
    Tokyo_Japan,
    Kyoto_Japan,
    Osaka_Japan,

    // USA
    NewYork_USA,
    LosAngeles_USA,
    Chicago_USA,
    Barrow_USA,

    // Vietnam
    Hanoi_Vietnam,
    HoChiMinhCity_Vietnam,
    DaNang_Vietnam,

    // Norway
    Oslo_Norway,
    Bergen_Norway,
    Stavanger_Norway,
    Troms_Norway,
    Svalbard_Norway,

    // India
    NewDelhi_India,
    Mumbai_India,
    Bangalore_India,

    // New Zealand
    Wellington_NewZealand,
    Auckland_NewZealand,
    Christchurch_NewZealand,

    // Brazil
    SaoPaulo_Brazil,
    RioDeJaneiro_Brazil,
    Brasilia_Brazil,

    // Egypt
    Cairo_Egypt,
    Alexandria_Egypt,
    Giza_Egypt,

    // Russia
    Moscow_Russia,
    SaintPetersburg_Russia,
    Novosibirsk_Russia,

    // South Africa
    Johannesburg_SouthAfrica,
    CapeTown_SouthAfrica,
    Durban_SouthAfrica,

    // Australia
    Sydney_Australia,
    Melbourne_Australia,
    Brisbane_Australia,

    // UK
    London_UK,
    Manchester_UK,
    Birmingham_UK,

    // France
    Paris_France,
    Marseille_France,
    Lyon_France,

    // Germany
    Berlin_Germany,
    Munich_Germany,
    Hamburg_Germany,

    // Argentina
    BuenosAires_Argentina,
    Cordoba_Argentina,
    Rosario_Argentina,

    // Canada
    Toronto_Canada,
    Vancouver_Canada,
    Montreal_Canada,

    // China
    Beijing_China,
    Shanghai_China,
    Guangzhou_China,

    // Antartica
    McMurdo_Antarctica,
    Rothera_Antarctica,
}
