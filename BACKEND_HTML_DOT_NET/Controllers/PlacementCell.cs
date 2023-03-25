using BACKEND_HTML_DOT_NET.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class PlacementCell : Controller
    {
        private static List<PlacementDetailsVM> placementGraphList = new List<PlacementDetailsVM>();
        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient client;
        public PlacementCell(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }
        public IActionResult PlacementAdd()
        {
            return View();
        }
        public IActionResult PlacementView()
        {
            return View();
        }
        public IActionResult PlacementEdit()
        {
            return View();
        }
        public IActionResult PlacementDetailsForGraph()
        {
            var restRequest = new RestRequest("/GetPlacementDetailsForGraph", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<PlacementDetailsVM>>>(content);
                placementGraphList = user.data;
            }
            return View(placementGraphList);
        }

        public IActionResult PlacementDetailsForGraphAdd(long id = 0)
        {
            PlacementDetailsVM departmentForPlacement = new PlacementDetailsVM();
            try
            {
                if (id > 0)
                {
                    departmentForPlacement = placementGraphList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    departmentForPlacement.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(departmentForPlacement);

        }

        [HttpPost]
        public async Task<IActionResult> PlacementDetailsForGraphAdd(PlacementDetailsVM placementGraph)
        {
            placementGraph.CreatedDate = DateTime.Now;
            placementGraph.UpdatedDate = DateTime.Now;

            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddPlacementDetailForGraph");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(placementGraph), Encoding.UTF8, "application/json");

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

        public IActionResult PlacementDetailsForGraphEdit(long id)
        {
            PlacementDetailsVM departmentForPlacement = new PlacementDetailsVM();
            try
            {

                departmentForPlacement = placementGraphList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    departmentForPlacement.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(departmentForPlacement);
        }

        [HttpPost]
        public async Task<IActionResult> PlacementDetailsForGraphEdit(PlacementDetailsVM placementGraph)
        {

            try
            {
                var placementEditModel = placementGraphList.Where(m => m.Id == placementGraph.Id).FirstOrDefault();
                placementEditModel.UpdatedDate = DateTime.Now;
                placementEditModel.LowestPackage = placementGraph.LowestPackage;
                placementEditModel.DeptId = placementGraph.DeptId;
                placementEditModel.HigestPackage = placementGraph.HigestPackage;
                placementEditModel.NoOfCompany = placementGraph.NoOfCompany;
                placementEditModel.TotalStudent = placementGraph.TotalStudent;
                placementEditModel.PlacedStudent = placementGraph.PlacedStudent;
                placementEditModel.NumberofRegisterdStudent = placementGraph.NumberofRegisterdStudent;
                placementEditModel.PlacementYear = placementGraph.PlacementYear;

                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdatePlacementDetailForGraph");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(placementEditModel), Encoding.UTF8, "application/json");

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
        public async Task<IActionResult> PlacementDetailsForGraphDelete(long id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = placementGraphList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeletePlacementDetailForGraph");
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
        public IActionResult PlacementAttachmentsAdd()
        {
            return View();
        }
        public IActionResult PlacementAttachmentsView()
        {
            return View();
        }
        public IActionResult PlacementMemberAdd()
        {
            return View();
        }
        public IActionResult PlacementMemberView()
        {
            return View();
        }
    }
}
