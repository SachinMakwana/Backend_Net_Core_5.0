using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class CollegeGallery : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<GalleryVM> galleryVMList = new List<GalleryVM>();

        public IActionResult GalleryList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllGalleryDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<GalleryVM>>>(content);
            galleryVMList = user.data;
            return View(galleryVMList);

        }
        public IActionResult GalleryView(int id=0)
        {
            return View();

        }

        public IActionResult GalleryAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GalleryAdd(GalleryVM galleryVM)
        {
            galleryVM.CreatedDate = DateTime.Now;
            galleryVM.UpdatedDate = DateTime.Now;
            TryUpdateModelAsync<GalleryVM>(galleryVM);

            using (var client = new HttpClient())
            {
                var uri = new Uri(apiBaseUrl + "/AddGalleryDetail");
                IFormCollection data = (IFormCollection)galleryVM;
                StringContent content = new StringContent(JsonConvert.SerializeObject(galleryVM), Encoding.UTF8, "application/json");
                using (var response = client.PostAsync(uri, content))
                {
                    response.Wait();
                    var results = response.Result;
                    if (results.IsSuccessStatusCode)
                    {
                        return Json("Submited 👌");
                    }
                }
            }

            return Json("Error 😒");
        }
    }
}
