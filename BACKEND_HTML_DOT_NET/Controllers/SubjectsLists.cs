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
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class SubjectsLists : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<SubjectVM> attachmentVMList = new List<SubjectVM>();
        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public SubjectsLists(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
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

        public IActionResult SubjectEdit(long id = 0)
        {
            SubjectVM subjectVM = new SubjectVM();
            try
            {

                subjectVM = attachmentVMList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetSubjectDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<SubjectVM>>>(content);

                }
            }
            catch (Exception ex)
            {

            }
            return View(subjectVM);
        }

        [HttpPost]
        public async Task<IActionResult> SubjectEdit(IFormCollection collection)
        {
            try
            {
                SubjectVM subjectVM = new SubjectVM();
                await TryUpdateModelAsync<SubjectVM>(subjectVM);
                subjectVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateSubjectDetails", Method.Put);

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
                PropertyInfo[] properties = typeof(SubjectVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(subjectVM);
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
        public async Task<IActionResult> SubjectDelete(long id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = attachmentVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteSubjectDetails");
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

    }
}
