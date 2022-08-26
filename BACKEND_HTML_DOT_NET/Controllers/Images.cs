using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Images : Controller
    {

        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<GalleryVM> galleryList = new List<GalleryVM>();
        RestClient client;


        public Images()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult ImagesVikew()
        {
            return View();
        }

        public IActionResult ImagesView()
        {

            var restRequest = new RestRequest("/GetAllGalleryTag", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<GalleryVM>>>(content);
                galleryList = user.data;
                foreach (var data in galleryList)
                {
                    data.Image = "https://localhost:44374/" + data.Image;
                }
            }
            return View(galleryList);

        }
        public IActionResult ImagesAdd()
        {
            return View();
        }

    }
}
