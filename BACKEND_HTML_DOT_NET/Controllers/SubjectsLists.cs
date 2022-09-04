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
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class SubjectsLists : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<SubjectVM> attachmentVMList = new List<SubjectVM>();
        RestClient client;

        public SubjectsLists()
        {
            client = new RestClient(apiBaseUrl);

        }

        public IActionResult SubjectAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubjectAdd(SubjectVM subjectVM)
        {

            subjectVM.CreatedDate = DateTime.Now;
            subjectVM.UpdatedDate = DateTime.Now;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddSubjectDetails");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(subjectVM), Encoding.UTF8, "application/json");

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
        public IActionResult SubjectView()
        {

            var restRequest = new RestRequest("/GetSubjectDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<SubjectVM>>>(content);
                attachmentVMList = user.data;
                
            }
            return View(attachmentVMList);
        }
    }
}
