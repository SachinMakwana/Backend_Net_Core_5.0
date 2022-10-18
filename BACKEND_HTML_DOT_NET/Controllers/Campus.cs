using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
    public class Campus : Controller
    {

        HttpClient hc = new HttpClient();
        private static List<CampusVM> campusVMList = new List<CampusVM>();
        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public Campus(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult CampusAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CampusAdd(IFormCollection collection)
        {

            try
            {
                CampusVM campusVM = new CampusVM();
                await TryUpdateModelAsync<CampusVM>(campusVM);
                campusVM.CreatedDate = DateTime.Now;
                campusVM.UpdatedDate = DateTime.Now;
                var request = new RestRequest("/AddCampusDetails", Method.Post);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(CampusVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(campusVM);
                    request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
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


        public IActionResult CampusView()
        {
            var restRequest = new RestRequest("/GetAllCampusDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<CampusVM>>>(content);
                campusVMList = user.data;
                foreach (var data in campusVMList = user.data)
                {
                    data.Image = imageBaseUrl + data.Image;
                }
            }
            return View(campusVMList);
        }
        public IActionResult CampusEdit(long id = 0)
        {
            CampusVM campusVM = new CampusVM();
            try
            {

                campusVM = campusVMList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/UpdateCampusDetail", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<AchievementVM>>>(content);

                }
            }
            catch (Exception ex)
            {

            }
            return View(campusVM);
        }

        [HttpPost]
        public async Task<IActionResult> CampusEdit(IFormCollection collection)
        {
            try
            {
                CampusVM campusVM = new CampusVM();
                await TryUpdateModelAsync<CampusVM>(campusVM);
                campusVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateCampusDetail", Method.Post);

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
                PropertyInfo[] properties = typeof(CampusVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(campusVM);
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
        public async Task<IActionResult> CampusDelete(long id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = campusVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteCampusDetail");
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
