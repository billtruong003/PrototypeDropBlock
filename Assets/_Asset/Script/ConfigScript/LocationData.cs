using UnityEngine;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NewLocationData", menuName = "Weather/Location Data", order = 1)]
public class LocationData : ScriptableObject
{
    public List<Location> locations;

    public Vector2 GetLocation(Locate locate)
    {
        foreach (Location item in locations)
        {
            if (item.locate == locate)
            {
                return new Vector2(item.latitude, item.longitude);
            }
        }
        Location location = locations[0];
        return new Vector2(location.latitude, location.longitude);
    }
    [Button]
    void InitializeLocations()
    {
        locations = new();
        // Japan
        locations.Add(new Location("Tokyo", Locate.Tokyo_Japan, 35.682839f, 139.759455f));
        locations.Add(new Location("Kyoto", Locate.Kyoto_Japan, 35.011564f, 135.768149f));
        locations.Add(new Location("Osaka", Locate.Osaka_Japan, 34.693738f, 135.502165f));

        // USA
        locations.Add(new Location("New York", Locate.NewYork_USA, 40.712776f, -74.005974f));
        locations.Add(new Location("Los Angeles", Locate.LosAngeles_USA, 34.052235f, -118.243683f));
        locations.Add(new Location("Chicago", Locate.Chicago_USA, 41.878113f, -87.629799f));
        locations.Add(new Location("Barrow (Utqiagvik)", Locate.Barrow_USA, 71.290556f, -156.788611f));

        // Vietnam
        locations.Add(new Location("Hanoi", Locate.Hanoi_Vietnam, 21.028511f, 105.804817f));
        locations.Add(new Location("Ho Chi Minh City", Locate.HoChiMinhCity_Vietnam, 10.762622f, 106.660172f));
        locations.Add(new Location("Da Nang", Locate.DaNang_Vietnam, 16.047079f, 108.206230f));

        // Norway
        locations.Add(new Location("Oslo", Locate.Oslo_Norway, 59.913869f, 10.752245f));
        locations.Add(new Location("Bergen", Locate.Bergen_Norway, 60.391263f, 5.322054f));
        locations.Add(new Location("Stavanger", Locate.Stavanger_Norway, 58.970056f, 5.733945f));
        locations.Add(new Location("Tromsø", Locate.Troms_Norway, 69.6492f, 18.9560f));
        locations.Add(new Location("Svalbard", Locate.Svalbard_Norway, 78.2232f, 15.6469f));

        // India
        locations.Add(new Location("New Delhi", Locate.NewDelhi_India, 28.613939f, 77.209023f));
        locations.Add(new Location("Mumbai", Locate.Mumbai_India, 19.076090f, 72.877426f));
        locations.Add(new Location("Bangalore", Locate.Bangalore_India, 12.971599f, 77.594566f));

        // New Zealand
        locations.Add(new Location("Wellington", Locate.Wellington_NewZealand, -41.286461f, 174.776230f));
        locations.Add(new Location("Auckland", Locate.Auckland_NewZealand, -36.848460f, 174.763332f));
        locations.Add(new Location("Christchurch", Locate.Christchurch_NewZealand, -43.532054f, 172.636225f));

        // Brazil
        locations.Add(new Location("São Paulo", Locate.SaoPaulo_Brazil, -23.550520f, -46.633308f));
        locations.Add(new Location("Rio de Janeiro", Locate.RioDeJaneiro_Brazil, -22.906847f, -43.172896f));
        locations.Add(new Location("Brasilia", Locate.Brasilia_Brazil, -15.826691f, -47.921820f));

        // Egypt
        locations.Add(new Location("Cairo", Locate.Cairo_Egypt, 30.044420f, 31.235712f));
        locations.Add(new Location("Alexandria", Locate.Alexandria_Egypt, 31.200092f, 29.918739f));
        locations.Add(new Location("Giza", Locate.Giza_Egypt, 30.013056f, 31.208853f));

        // Russia
        locations.Add(new Location("Moscow", Locate.Moscow_Russia, 55.755825f, 37.617298f));
        locations.Add(new Location("Saint Petersburg", Locate.SaintPetersburg_Russia, 59.934280f, 30.335098f));
        locations.Add(new Location("Novosibirsk", Locate.Novosibirsk_Russia, 55.008353f, 82.935733f));

        // South Africa
        locations.Add(new Location("Johannesburg", Locate.Johannesburg_SouthAfrica, -26.204103f, 28.047304f));
        locations.Add(new Location("Cape Town", Locate.CapeTown_SouthAfrica, -33.924869f, 18.424055f));
        locations.Add(new Location("Durban", Locate.Durban_SouthAfrica, -29.858680f, 31.021840f));

        // Australia
        locations.Add(new Location("Sydney", Locate.Sydney_Australia, -33.868820f, 151.209296f));
        locations.Add(new Location("Melbourne", Locate.Melbourne_Australia, -37.813629f, 144.963058f));
        locations.Add(new Location("Brisbane", Locate.Brisbane_Australia, -27.469770f, 153.025124f));

        // UK
        locations.Add(new Location("London", Locate.London_UK, 51.507351f, -0.127758f));
        locations.Add(new Location("Manchester", Locate.Manchester_UK, 53.480759f, -2.242631f));
        locations.Add(new Location("Birmingham", Locate.Birmingham_UK, 52.486243f, -1.890401f));

        // France
        locations.Add(new Location("Paris", Locate.Paris_France, 48.856613f, 2.352222f));
        locations.Add(new Location("Marseille", Locate.Marseille_France, 43.296482f, 5.369780f));
        locations.Add(new Location("Lyon", Locate.Lyon_France, 45.764043f, 4.835659f));

        // Germany
        locations.Add(new Location("Berlin", Locate.Berlin_Germany, 52.520008f, 13.404954f));
        locations.Add(new Location("Munich", Locate.Munich_Germany, 48.135125f, 11.581981f));
        locations.Add(new Location("Hamburg", Locate.Hamburg_Germany, 53.551086f, 9.993682f));

        // Argentina
        locations.Add(new Location("Buenos Aires", Locate.BuenosAires_Argentina, -34.603722f, -58.381592f));
        locations.Add(new Location("Cordoba", Locate.Cordoba_Argentina, -31.420083f, -64.188776f));
        locations.Add(new Location("Rosario", Locate.Rosario_Argentina, -32.944243f, -60.650538f));

        // Canada
        locations.Add(new Location("Toronto", Locate.Toronto_Canada, 43.651070f, -79.347015f));
        locations.Add(new Location("Vancouver", Locate.Vancouver_Canada, 49.282729f, -123.120738f));
        locations.Add(new Location("Montreal", Locate.Montreal_Canada, 45.501689f, -73.567256f));

        // China
        locations.Add(new Location("Beijing", Locate.Beijing_China, 39.904202f, 116.407394f));
        locations.Add(new Location("Shanghai", Locate.Shanghai_China, 31.230391f, 121.473701f));
        locations.Add(new Location("Guangzhou", Locate.Guangzhou_China, 23.129110f, 113.264385f));

        // Antarctica
        locations.Add(new Location("McMurdo Station", Locate.McMurdo_Antarctica, -77.8419f, 166.6863f));
        locations.Add(new Location("Rothera Research Station", Locate.Rothera_Antarctica, -67.5683f, -68.1236f));
    }
}
