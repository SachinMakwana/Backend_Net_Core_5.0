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
using Microsoft.AspNetCore.Http;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class News : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<NewsVM> newsVMList = new List<NewsVM>();

        public IActionResult NewsList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllNewsDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<NewsVM>>>(content);
            newsVMList = user.data;
            return View(newsVMList);

        }
        
        public IActionResult NewsView(int id=0)
        {
            NewsVM newsVM = new NewsVM();
            newsVM = newsVMList.Where(m => m.Id == id).FirstOrDefault();

            return View(newsVM);
        }


        public IActionResult NewsAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewsAdd(NewsVM data)
        {
            data.Date = DateTime.Now;
            data.CreatedDate = DateTime.Now;
            data.UpdatedDate = DateTime.Now;

            if (data.Id == 0)
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddNewsDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
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
            }
            else
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateNewsDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    using (var response = client.PutAsync(uri, content))
                    {
                        response.Wait();
                        var results = response.Result;
                        if (results.IsSuccessStatusCode)
                        {
                            return Json("Submited 👌");
                        }
                    }
                }
            }
            return Json("Error 😒");
        }

      

    }
}
