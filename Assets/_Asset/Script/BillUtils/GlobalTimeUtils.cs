using System;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace BillUtils.GlobalTimeUtils
{
    public static class GlobalTimeUtils
    {
        private static float lastRequestTime = 0f;
        private static readonly float requestCooldownSeconds = 300f;

        public static DateTime GetCurrentTimeAtTimeZone(string timeZoneId)
        {
            try
            {

                if (Time.time - lastRequestTime > requestCooldownSeconds)
                {
                    return FetchCurrentTimeFromApi(timeZoneId);
                }
                else
                {
                    Debug.Log("Request is within cooldown period.");
                    return DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"An error occurred: {ex.Message}");
                return DateTime.MinValue;
            }
        }

        private static DateTime FetchCurrentTimeFromApi(string timeZoneId)
        {
            DateTime currentTime = DateTime.UtcNow;

            lastRequestTime = Time.time;

            return currentTime;
        }
    }
}
