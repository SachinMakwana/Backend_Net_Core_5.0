using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Department : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<DepartmentVM> deptVMList = new List<DepartmentVM>();
        RestClient client;

        public Department()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult DepartmentList()
        {
          
            var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
            deptVMList = user.data;
            return View(deptVMList);
        }
        public IActionResult DepartmentAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DepartmentAdd(IFormCollection collection)
        {
            try
            {
                AchievementVM achievementVM = new AchievementVM();
                await TryUpdateModelAsync<AchievementVM>(achievementVM);

                var request = new RestRequest("/AddDepartmentDetail", Method.Post);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(AchievementVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(achievementVM);
                    request.AddParameter(property.Name.ToString(), value==null?"":value.ToString());
                }
            
                var response = client.Execute(request);
                //use response.content --> this will directly give the parsed result.
                return Json(true);

            }
            catch (Exception ex)
            {
                return Json(false);
            }
            return Json(true);
        }

        public IActionResult DepartmentView()
        {
            return View();
        }
        public IActionResult DepartmentAllView()
        {
            return View();
        }
    }
}
