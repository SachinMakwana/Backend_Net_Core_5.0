﻿using BACKEND_HTML_DOT_NET.Models;
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
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class Vision : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<VisionVM> visionVMList = new List<VisionVM>();
        private static List<DepartmentVM> deptList = new List<DepartmentVM>();

        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public Vision(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult VisionList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllVisionDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;

            var restRequest2 = new RestRequest("/GetAllDepartmentDetails", Method.Get);
            restRequest2.AddHeader("Accept", "application/json");
            restRequest2.RequestFormat = DataFormat.Json;

            RestResponse response2 = restClient.Execute(restRequest2);

            var content2 = response2.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<VisionVM>>>(content);
            var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content2);
            deptList = dept.data;
            visionVMList = user.data;
            foreach (var data in visionVMList)
            {
                foreach (var deptment in deptList)
                {
                    if (data.DeptId == deptment.Id)
                    {
                        data.DeptName = deptment.Name;
                        break;
                    }
                }
            }
            return View(visionVMList);
        }
        public IActionResult VisionAdd(int id = 0)
        {
            VisionVM visionVM = new VisionVM();
            try
            {
                if (id > 0)
                {
                    visionVM = visionVMList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    visionVM.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(visionVM);
        }
        [HttpPost]
        public async Task<IActionResult> VisionAdd(VisionVM visionDetail)
        {
            visionDetail.CreatedDate = DateTime.Now;
            visionDetail.UpdatedDate = DateTime.Now;

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddVisionDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(visionDetail), Encoding.UTF8, "application/json");

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

        public IActionResult VisionEdit(int id)
        {
            VisionVM visionVM = new VisionVM();
            try
            {
                
                visionVM = visionVMList.Where(m => m.Id == id).FirstOrDefault();
                
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    visionVM.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(visionVM);
        }

        [HttpPost]
        public async Task<IActionResult> VisionEdit(VisionVM vision)
        {
            var visionDetail = visionVMList.Where(m => m.Id == vision.Id).FirstOrDefault();
            visionDetail.UpdatedDate = DateTime.Now;
            visionDetail.Description = vision.Description;
            visionDetail.DeptId = vision.DeptId;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateVisionDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(visionDetail), Encoding.UTF8, "application/json");

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
        public async Task<IActionResult> VisionDelete(int id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = visionVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteVisionDetail");
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
