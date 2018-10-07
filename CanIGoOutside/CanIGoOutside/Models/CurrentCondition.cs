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
                    Temperature = data.Temperature.Value;
                    WindSpeed = data.Wind.Speed.Value;
                    RelativeHumidity = data.RelativeHumidity;
                    PrecipitationProbability = data.PrecipitationProbability;
                    RainValue = data.Rain.Value;
                    SnowValue = data.Snow.Value;
                    IceValue = data.Ice.Value;
                }
            }
        }
        public decimal Temperature { get; set; }
        public decimal WindSpeed { get; set; }
        public decimal RelativeHumidity { get; set; }
        public decimal PrecipitationProbability { get; set; }
        public decimal RainValue { get; set; }
        public decimal SnowValue { get; set; }
        public decimal IceValue { get; set; }
    }
}