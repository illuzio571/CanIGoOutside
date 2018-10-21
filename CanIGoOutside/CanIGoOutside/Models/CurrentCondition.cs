using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanIGoOutside.Models
{
    public class CurrentCondition
    {
        public CurrentCondition(string json)
        {
            if (json != null)
            {
                JArray jsonArray = JArray.Parse(json);
                if (jsonArray != null)
                {
                    dynamic data = JObject.Parse(jsonArray[0].ToString());
                    WeatherText = data.WeatherText;
                    LocalTime = data.LocalObservationDateTime;
                }
            }
        }
        public string WeatherText { get; set; }
        public string LocalTime { get; set; }
    }
}