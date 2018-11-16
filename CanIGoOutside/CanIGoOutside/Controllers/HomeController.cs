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
        HourlyForecast hourlyForecast;
        CurrentCondition currentCondition;

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.image = "SunnyDay";
            return View();
        }

        [HttpPost]
        public ActionResult Index(string zip)
        {
            GetLocationCode(zip);
            if (locationCode != null)
            {
                GetHourlyForecast();
                GetCurrentCondition();

                ViewBag.temp = hourlyForecast.Temperature;
                ViewBag.windSpeed = hourlyForecast.WindSpeed;
                ViewBag.humidity = hourlyForecast.RelativeHumidity;
                ViewBag.rainChance = hourlyForecast.PrecipitationProbability;

                if (CheckIfYouCanGoOutside())
                {
                    ViewBag.result = "Yes!";
                }
                else
                {
                    ViewBag.result = "No.";
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
            var client = new RestClient("http://dataservice.accuweather.com/locations/v1/postalcodes/search?apikey=SyQbb4hIuNveRnj4NLzAWrRWBPMyv53q&q=" + zip);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "23acb8fb-c16a-4064-b29c-7d55d782675e");
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);

            PostalCode postalCode = new PostalCode(response.Content.ToString());

            locationCode = postalCode.Key;
        }

        private void GetHourlyForecast()
        {
            var client = new RestClient("http://dataservice.accuweather.com/forecasts/v1/hourly/1hour/" + locationCode + "?apikey=SyQbb4hIuNveRnj4NLzAWrRWBPMyv53q&details=true&metric=false");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "23acb8fb-c16a-4064-b29c-7d55d782675e");
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);

            hourlyForecast = new HourlyForecast(response.Content.ToString());
        }

        private void GetCurrentCondition()
        {
            var client = new RestClient("http://dataservice.accuweather.com/currentconditions/v1/" + locationCode + "?apikey=SyQbb4hIuNveRnj4NLzAWrRWBPMyv53q&details=true");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Postman-Token", "23acb8fb-c16a-4064-b29c-7d55d782675e");
            request.AddHeader("Cache-Control", "no-cache");
            IRestResponse response = client.Execute(request);

            currentCondition = new CurrentCondition(response.Content.ToString());
        }

        private bool CheckIfYouCanGoOutside()
        {
            if (currentCondition.WeatherText.ToString().ToLower().Contains("thunder"))
            {
                ViewBag.reason = "There's thunder out there!!!";
                ViewBag.image = "SunnyDay";
                return false;
            }
            else if (currentCondition.WeatherText.ToString().ToLower().Contains("snow"))
            {
                ViewBag.reason = "It's snowing!!!";
                ViewBag.image = "SunnyDay";
                return false;
            }
            else if (currentCondition.WeatherText.ToString().ToLower().Contains("ice"))
            {
                ViewBag.reason = "There's ice!!!";
                ViewBag.image = "SunnyDay";
                return false;
            }
            else if (currentCondition.WeatherText.ToString().ToLower().Contains("rain"))
            {
                ViewBag.reason = "It's raining!!!";
                ViewBag.image = "RainyDay";
                return false;
            }
            else if (hourlyForecast.Temperature < 50)
            {
                ViewBag.reason = "Too cold!";
                ViewBag.image = "SunnyDay";
                return false;
            }
            else if (hourlyForecast.Temperature >= 90)
            {
                ViewBag.reason = "Too hot!";
                ViewBag.image = "SunnyDay";
                return false;
            }
            else
            {
                ViewBag.reason = "It's " + currentCondition.WeatherText.ToString().ToLower() + ".";
                return true;
            }
        }
    }
}