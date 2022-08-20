using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
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
    public class FacultyDetails : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<FacultyDetailsVM> facultyDetailsList = new List<FacultyDetailsVM>();

        public IActionResult FacultyList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
            facultyDetailsList = user.data;
            return View(facultyDetailsList);
        }
        public IActionResult FacultyView(int id = 0)
        {

            FacultyDetailsVM newsVM = new FacultyDetailsVM();
            newsVM = facultyDetailsList.Where(m => m.Id == id).FirstOrDefault();
            return View(newsVM);
        }

        public IActionResult FacultyAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FacultyAdd(FacultyDetailsVM faculty)
        {
            faculty.CreatedDate = DateTime.Now;
            faculty.UpdatedDate = DateTime.Now;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddFacultyDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(faculty), Encoding.UTF8, "application/json");

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


        public IActionResult FacultyEdit(int id)
        {
            var item = facultyDetailsList.Where(m => m.Id == id).FirstOrDefault();
            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> FacultyUpdate(FacultyDetailsVM faculty)
        {
            var updateItem = facultyDetailsList.Where(m => m.Id == faculty.Id).FirstOrDefault();
            updateItem.Name = faculty.Name;
            updateItem.DeptId = faculty.DeptId;
            updateItem.DesignationId = faculty.DesignationId;
            updateItem.UpdatedDate = DateTime.Now;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateFacultyDetail");
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
