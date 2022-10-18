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
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class News : Controller
    {
         HttpClient hc = new HttpClient();
        private static List<NewsVM> newsVMList = new List<NewsVM>();

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient restClient;
        public News(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            restClient = new RestClient(apiBaseUrl);
        }

        public IActionResult NewsList()
        {
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
        public async Task<IActionResult> NewsAdd(NewsVM news)
        {
            news.Date = DateTime.Now;
            news.CreatedDate = DateTime.Now;
            news.UpdatedDate = DateTime.Now;

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddNewsDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(news), Encoding.UTF8, "application/json");

                    using (var response = client.PostAsync(uri, content))
                    {
                        response.Wait();
                        var results = response.Result;
                        var jsonString = await results.Content.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<ServiceResponse<bool>>(jsonString);
                        if (results.IsSuccessStatusCode)
                        {
                            return Json(res);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.ToString() });
            }
            return Json(new { message = "something went wrong." });
        }

        public IActionResult NewsEdit(int id)
        {
            var item = newsVMList.Where(m => m.Id == id).FirstOrDefault();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> NewsUpdate(NewsVM news)
        {
            var updateItem = newsVMList.Where(m => m.Id == news.Id).FirstOrDefault();
            updateItem.Title = news.Title;
            updateItem.UpdatedDate = DateTime.Now;
            updateItem.Description = news.Description;
            updateItem.Date = DateTime.Now;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateNewsDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateItem), Encoding.UTF8, "application/json");

                    using (var response = client.PostAsync(uri, content))
                    {
                        response.Wait();
                        var results = response.Result;
                        var jsonString = await results.Content.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<ServiceResponse<bool>>(jsonString);
                        if (results.IsSuccessStatusCode)
                        {
                            return Json(res);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.ToString() });
            }
            return Json(new { message = "something went wrong." });
        }

        [HttpPost]
        public async Task<IActionResult> NewsDelete(int id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = newsVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteNewsDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateItem), Encoding.UTF8, "application/json");

                    using (var response = client.PostAsync(uri, content))
                    {
                        response.Wait();
                        var results = response.Result;
                        var jsonString = await results.Content.ReadAsStringAsync();

                        var res = JsonConvert.DeserializeObject<ServiceResponse<bool>>(jsonString);
                        if (results.IsSuccessStatusCode)
                        {
                            return Json(res);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { message = ex.Message.ToString() });
            }
            return Json(new { message = "something went wrong." });
        }


    }
}
