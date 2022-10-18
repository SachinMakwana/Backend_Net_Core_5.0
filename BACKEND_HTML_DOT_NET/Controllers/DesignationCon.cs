using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class DesignationCon : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<DesignationVM> designatioVMList = new List<DesignationVM>();
        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public DesignationCon(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult DesignationList()
        {

            var restRequest = new RestRequest("/GetAllDesignationDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<DesignationVM>>>(content);
                designatioVMList = user.data;
               
            }
            return View(designatioVMList);
        }
        public IActionResult DesignationAdd(long id = 0)
        {
            DesignationVM departmentVM = new DesignationVM();
            try
            {
                if (id > 0)
                {
                    departmentVM = designatioVMList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDesignationDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                
            }
            catch (Exception ex)
            {

            }
            return View(departmentVM);
        }


        [HttpPost]
        public IActionResult DesignationAdd([FromForm] DesignationVM designationVM, [Optional] IFormCollection collection)
        {
            try
            {
                RestRequest request = new RestRequest("/AddDesignationDetail", Method.Post);
                designationVM.CreatedDate = DateTime.Now;
                designationVM.UpdatedDate = DateTime.Now;

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
                PropertyInfo[] properties = typeof(DesignationVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(designationVM);
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

        public IActionResult DesignationEdit(long id = 0)
        {
            DesignationVM designationVM = new DesignationVM();
            try
            {

                designationVM = designatioVMList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetAllDesignationDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                
            }
            catch (Exception ex)
            {

            }
            return View(designationVM);
        }

        [HttpPost]
        public IActionResult DesignationEdit([FromForm] DesignationVM designationVM, [Optional] IFormCollection collection)
        {
            try
            {
                designationVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateDesignationDetail", Method.Post);

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
                PropertyInfo[] properties = typeof(DesignationVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(designationVM);
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
        public async Task<IActionResult> DesignationDelete(long id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = designatioVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteDesignationDetail");
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
        public IActionResult DesignationView()
        {
            return View();
        }

        public IActionResult DesignationAllView()
        {
            return View();
        }
    }
}
