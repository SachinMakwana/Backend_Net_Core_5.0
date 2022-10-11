using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class FacultyDetails : Controller
    {
        private string apiBaseUrl = "https://api.gecpatan.ac.in/api";
        HttpClient hc = new HttpClient();
        private static List<FacultyDetailsVM> facultyDetailsList = new List<FacultyDetailsVM>();
        RestClient client;
        public FacultyDetails()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult FacultyList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                facultyDetailsList = user.data;
                foreach (var data in facultyDetailsList)
                {
                    data.Image = "https://api.gecpatan.ac.in/" + data.Image;
                }
            }

            return View(facultyDetailsList);
        }
        public IActionResult FacultyView(int id = 0)
        {

            FacultyDetailsVM newsVM = new FacultyDetailsVM();
            newsVM = facultyDetailsList.Where(m => m.Id == id).FirstOrDefault();
            return View(newsVM);
        }

        public IActionResult FacultyAdd(long id = 0)
        {
            FacultyDetailsVM facultyVM = new FacultyDetailsVM();
            try
            {
                if (id > 0)
                {
                    facultyVM = facultyDetailsList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                    facultyVM.DepartmentSelectList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(facultyVM);
        }

        [HttpPost]
        public async Task<IActionResult> FacultyAdd([FromForm] FacultyDetailsVM faculty, [Optional] IFormCollection collection)
        {
            faculty.CreatedDate = DateTime.Now;
            faculty.UpdatedDate = DateTime.Now;
            try
            {
                RestRequest request = new RestRequest("/AddFacultyDetail", Method.Post);
                faculty.CreatedDate = DateTime.Now;
                faculty.UpdatedDate = DateTime.Now;

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
                PropertyInfo[] properties = typeof(FacultyDetailsVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(faculty);
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


        public IActionResult FacultyEdit(long id = 0)
        {
            FacultyDetailsVM facultyVM = new FacultyDetailsVM();
            try
            {
                if (id > 0)
                {
                    facultyVM = facultyDetailsList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                    facultyVM.DepartmentSelectList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(facultyVM);
        }

        [HttpPost]
        public async Task<IActionResult> FacultyUpdate([FromForm] FacultyDetailsVM faculty, [Optional] IFormCollection collection)
        {
            try
            {
                faculty.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateFacultyDetail", Method.Put);

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
                else
                {
                    byte[] data = new byte[0];
                    request.AddFile("image", data, "noimage");
                }


                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(FacultyDetailsVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(faculty);
                        request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                    }
                }

                var response = client.Execute(request);
                //use response.content --> this will directly give the parsed result.
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);

            }
            catch (Exception ex)
            {
                return Json(new { status_code = "000", message = ex.Message.ToString() });
            }
        }

        [HttpPost]
        public async Task<IActionResult> FacultyDelete(int id)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = facultyDetailsList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteFacultyDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(updateItem), Encoding.UTF8, "application/json");

                    using (var response = client.PutAsync(uri, content))
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
        public IActionResult FacultyDetail()
        {
            return View();
        }
    }
}
