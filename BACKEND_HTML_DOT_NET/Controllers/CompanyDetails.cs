using BACKEND_HTML_DOT_NET.Models;
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
    public class CompanyDetails : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<CompanyVM> companyList = new List<CompanyVM>();
        RestClient client;

        public CompanyDetails()
        {
            client = new RestClient(apiBaseUrl);
        }
        public IActionResult CompanyList()
        {
            var restRequest = new RestRequest("/GetAllCompanyDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<CompanyVM>>>(content);
                companyList = user.data;
                foreach (var data in companyList)
                {
                    data.Logo = "https://localhost:44374/" + data.Logo;
                    data.Image = "https://localhost:44374/" + data.Image;
                }
            }
            return View(companyList);
        }
        public IActionResult CompanyAdd(int id = 0)
        {
            CompanyVM companyVM = new CompanyVM();
            try
            {
                if (id > 0)
                {
                    companyVM = companyList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    companyVM.DepartmentList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(companyVM);
        }
        [HttpPost]
        public IActionResult CompanyAdd([FromForm] CompanyVM companyVM, [Optional] IFormCollection collection)
        {
            try
            {
                RestRequest request = new RestRequest("/AddCompanyDetail", Method.Post);
                companyVM.CreatedDate = DateTime.Now;
                companyVM.UpdatedDate = DateTime.Now;

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
                PropertyInfo[] properties = typeof(CompanyVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "DepartmentList")
                    {
                        var value = property.GetValue(companyVM);
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
        public IActionResult CompanyEdit(int id = 0)
        {
            CompanyVM companyVM = new CompanyVM();
            try
            {
                companyVM = companyList.Where(m => m.Id == id).FirstOrDefault();
                
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    companyVM.DepartmentList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(companyVM);
        }

        [HttpPost]
        public IActionResult CompanyEdit([FromForm] CompanyVM companyVM, [Optional] IFormCollection collection)
        {
            try
            {
                RestRequest request = new RestRequest("/UpdateCompanyDetail", Method.Post);
                companyVM.CreatedDate = DateTime.Now;
                companyVM.UpdatedDate = DateTime.Now;

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
                    request.AddFile("logo", data, "noimage");
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(CompanyVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "DepartmentList")
                    {
                        var value = property.GetValue(companyVM);
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
        [HttpPost]
        public async Task<IActionResult> CompanyDelete(long id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = companyList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteCompanyDetail");
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
            public IActionResult CompanyView()
        {
            return View();
        }
    }
}
