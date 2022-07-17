using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BACKEND_HTML_DOT_NET.Models;
using Newtonsoft.Json;
using System.Text;
using RestSharp;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class News : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();

        public IActionResult NewsView()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllNewsDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<NewsVM>(content);

            return View(user);

        }
        public IActionResult NewsAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewsAdd(FromBodyAttribute fromBodyAttribute)
        {

            return View();
        }
    }
}
