using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BACKEND_HTML_DOT_NET.Models;
using Newtonsoft.Json;
using System.Text;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class News : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api/";

        public IActionResult NewsView()
        {
            

            IList<NewsVM> news = new List<NewsVM>();

             using (HttpClient client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(news), Encoding.UTF8, "application/json");
                string endpoint = apiBaseUrl + "GetAllNewsDetails";
                using (var Response = await client.GetAsync(endpoint))
                {
                    if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["Profile"] = JsonConvert.SerializeObject(news);
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        ModelState.Clear();
                        ModelState.AddModelError(string.Empty, "Username or Password is Incorrect");
                        return View();
                    }
                }
            }

            return View(news);
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
