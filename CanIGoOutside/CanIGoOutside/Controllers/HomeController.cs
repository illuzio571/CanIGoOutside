using CanIGoOutside.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CanIGoOutside.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();
        string locationCode;
        CurrentCondition currentCondition;

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string zip)
        {
            GetLocationCode(zip);
            if (locationCode != null)
            {
                GetCurrentCondition();

                ViewBag.temp = currentCondition.Temperature;
                ViewBag.windSpeed = currentCondition.WindSpeed;
                ViewBag.humidity = currentCondition.RelativeHumidity;
                ViewBag.rainChance = currentCondition.PrecipitationProbability;

                if (CheckIfYouCanGoOutside())
                {
                    ViewBag.result = "Yes!";
                }
                else
                {
                    ViewBag.result = "No!";
                }
            }
            else
            {
                ViewBag.isValid = "false";
            }

            return View();
        }

        private void GetLocationCode(string zip)
        {
            var client = new RestClient("http://dataservice.accuweather.com/locations/v1/postalcodes/search?apikey=HackPSU2018&q=" + zip);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "23acb8fb-c16a-4064-b29c-7d55d782675e");
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);

            PostalCode postalCode = new PostalCode(response.Content.ToString());

            locationCode = postalCode.Key;
        }

        private void GetCurrentCondition()
        {
            var client = new RestClient("http://dataservice.accuweather.com/forecasts/v1/hourly/1hour/" + locationCode + "?apikey=HackPSU2018&details=true&metric=false");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "23acb8fb-c16a-4064-b29c-7d55d782675e");
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);

            currentCondition = new CurrentCondition(response.Content.ToString());
        }

        private bool CheckIfYouCanGoOutside()
        {
            if (currentCondition.Temperature < 50)
            {
                ViewBag.reason = "Too cold!";
                return true;
            }
            else if (currentCondition.Temperature >= 90)
            {
                ViewBag.reason = "Too hot!";
                return true;
            }
            else if (currentCondition.RainValue > 0)
            {
                ViewBag.reason = "It's raining!!!";
                return true;
            }
            else if (currentCondition.SnowValue > 0)
            {
                ViewBag.reason = "It's snowing!!!";
                return true;
            }
            else if (currentCondition.IceValue > 0)
            {
                ViewBag.reason = "There's ice!!!";
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}