using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class Images : Controller
    {

        HttpClient hc = new HttpClient();
        private static List<GalleryVM> galleryList = new List<GalleryVM>();
        RestClient client;


        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public Images(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
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
                    data.Image = imageBaseUrl + data.Image;
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
