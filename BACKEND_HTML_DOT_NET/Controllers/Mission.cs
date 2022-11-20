using BACKEND_HTML_DOT_NET.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class Mission : Controller
    {
       HttpClient hc = new HttpClient();
        private static List<MissionVM> missionVMList = new List<MissionVM>();
        private static List<DepartmentVM> deptList = new List<DepartmentVM>();

        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public Mission(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult MissionList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllMissionDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;

            var restRequest2 = new RestRequest("/GetAllDepartmentDetails", Method.Get);
            restRequest2.AddHeader("Accept", "application/json");
            restRequest2.RequestFormat = DataFormat.Json;

            RestResponse response2 = restClient.Execute(restRequest2);

            var content2 = response2.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<MissionVM>>>(content);
            var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content2);
            deptList = dept.data;
            missionVMList = user.data;
            foreach (var data in missionVMList)
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
            return View(missionVMList);
        }
        public IActionResult MissionAdd(int id = 0)
        {
            MissionVM missionVM = new MissionVM();
            try
            {
                if (id > 0)
                {
                    missionVM = missionVMList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    missionVM.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(missionVM);
        }
        [HttpPost]
        public async Task<IActionResult> MissionAdd(MissionVM missionDetail)
        {
            missionDetail.CreatedDate = DateTime.Now;
            missionDetail.UpdatedDate = DateTime.Now;

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddMissionDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(missionDetail), Encoding.UTF8, "application/json");

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

        public IActionResult MissionEdit(int id)
        {
            MissionVM missionVM = new MissionVM();
            try
            {

                missionVM = missionVMList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    missionVM.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(missionVM);
        }

        [HttpPost]
        public async Task<IActionResult> MissionEdit(MissionVM mission)
        {
            var visionDetail = missionVMList.Where(m => m.Id == mission.Id).FirstOrDefault();
            visionDetail.UpdatedDate = DateTime.Now;
            visionDetail.Description = mission.Description;
            visionDetail.DeptId = mission.DeptId;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateMissionDetail");
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
        public async Task<IActionResult> MissionDelete(int id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = missionVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteMissionDetail");
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
