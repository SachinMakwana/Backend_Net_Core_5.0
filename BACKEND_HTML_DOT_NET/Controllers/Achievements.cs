using BACKEND_HTML_DOT_NET.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Achievements : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        RestClient client;

        public Achievements()
        {
            client = new RestClient(apiBaseUrl);
        }
        public IActionResult AchievementsAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AchievementsAdd(IFormCollection collection)
        {
            try
            {
                AchievementVM achievementVM = new AchievementVM();
                await TryUpdateModelAsync<AchievementVM>(achievementVM);
                var request = new RestRequest("/AddAchievementDetail", Method.Post)
                {
                    AlwaysMultipartFormData = true
                };
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.FileName, bytes, file.FileName);
                }
                request.AddBody(achievementVM);
                var response = await client.GetAsync<ServiceResponse<bool>>(request);
                return Json(true);

            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(true);
        }
        public IActionResult AchievementsDetails()
        {
            return View();
        }
        public IActionResult AchievementsListView()
        {

            return View();
        }
        public IActionResult AchievementsView()
        {
            return View();
        }
    }
}
