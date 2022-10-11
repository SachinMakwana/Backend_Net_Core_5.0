using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Authorization;
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
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class AboutCollege : Controller
    {

        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<CollegeVM> collegeVM = new List<CollegeVM>();
        RestClient client;

        public AboutCollege()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult AboutCollegeView()
        {
            var restRequest = new RestRequest("/GetCollegeDetail", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<CollegeVM>>>(content);
                collegeVM = user.data;
                foreach (var data in collegeVM)
                {
                    data.Image = "https://localhost:44374/" + data.Image;
                }
                
            }
            return View(collegeVM);
        }

        public IActionResult AboutCollegeAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AboutCollegeAdd([FromForm] CollegeVM collegeVM, [Optional] IFormCollection collection)
        {
            try
            {
                RestRequest request = new RestRequest("/AddCollegeDetail", Method.Post);
                collegeVM.CreatedDate = DateTime.Now;
                collegeVM.UpdatedDate = DateTime.Now;

                if (collection.Files.Count() > 0)
                {
                    //add files to request
                    foreach (var file in collection.Files)
                    {
                        var memorystream = new MemoryStream();
                        file.CopyTo(memorystream);
                        var bytes = memorystream.ToArray();
                        request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                    }
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(CollegeVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "SelectList")
                    {
                        var value = property.GetValue(collegeVM);
                        request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                    }
                }

                var response = client.Execute(request);
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);

            }
            catch (Exception ex)
            {
                return Json(new { status_code = "000", message = ex.Message.ToString() });
            }
        }
    }
}
