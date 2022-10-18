using BACKEND_HTML_DOT_NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class DepartmentAmenty : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<DepartmentAmentyVM> departmentAmentyList = new List<DepartmentAmentyVM>();
        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public DepartmentAmenty(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult DepartmentAmentyList()
        {
            var restRequest = new RestRequest("/GetDepartmentAmentyDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentAmentyVM>>>(content);
                departmentAmentyList = user.data;
            }
            return View(departmentAmentyList);
        }

        public IActionResult DepartmentAmentyAdd(long id = 0)
        {
            DepartmentAmentyVM departmentAmenty = new DepartmentAmentyVM();
            try
            {
                if (id > 0)
                {
                    departmentAmenty = departmentAmentyList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    departmentAmenty.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(departmentAmenty);
        }
        [HttpPost]
        public async Task<IActionResult> DepartmentAmentyAddAsync(DepartmentAmentyVM departmentAmenty)
        {
            departmentAmenty.CreatedDate = DateTime.Now;
            departmentAmenty.UpdatedDate = DateTime.Now;

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddDepartmentAmentyDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(departmentAmenty), Encoding.UTF8, "application/json");

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
        public IActionResult DepartmentAmentyEdit(long id)
        {
            DepartmentAmentyVM departmentAmenty = new DepartmentAmentyVM();
            try
            {

                departmentAmenty = departmentAmentyList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    departmentAmenty.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(departmentAmenty);
        }
        [HttpPost]
        public async Task<IActionResult> DepartmentAmentyEditAsync(DepartmentAmentyVM departmentAmentyVM)
        {

            try
            {
                var departmentAmenty = departmentAmentyList.Where(m => m.Id == departmentAmentyVM.Id).FirstOrDefault();
                departmentAmenty.UpdatedDate = DateTime.Now;
                departmentAmenty.Subjects = departmentAmentyVM.Subjects;
                departmentAmenty.DeptId = departmentAmentyVM.DeptId;
                departmentAmenty.Classroom = departmentAmentyVM.Classroom;
                departmentAmenty.Intake = departmentAmentyVM.Intake;
                departmentAmenty.Labs = departmentAmentyVM.Labs;
                departmentAmenty.Workshop = departmentAmentyVM.Workshop;
                departmentAmenty.Seminar = departmentAmentyVM.Seminar;

                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateDepartmentAmentyDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(departmentAmenty), Encoding.UTF8, "application/json");

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
        public async Task<IActionResult> DepartmentAmentyDeleteAsync(long id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = departmentAmentyList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteDepartmentAmentyDetail");
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
