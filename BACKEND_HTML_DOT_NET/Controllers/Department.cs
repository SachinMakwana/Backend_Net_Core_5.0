using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class Department : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<DepartmentVM> deptList = new List<DepartmentVM>();
        RestClient client;

        public Department()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult DepartmentList()
        {

            var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                deptList = user.data;
                foreach(var data in deptList)
                {
                    data.Image = "https://localhost:44374/" + data.Image;
                }
            }
            return View(deptList);
        }
        public IActionResult DepartmentAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DepartmentAdd(IFormCollection collection)
        {
            try
            {
                DepartmentVM departmentVM = new DepartmentVM();
                await TryUpdateModelAsync<DepartmentVM>(departmentVM);
                departmentVM.CreatedDate = DateTime.Now;
                departmentVM.UpdatedDate = DateTime.Now;
                var request = new RestRequest("/AddDepartmentDetail", Method.Post);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(DepartmentVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(departmentVM);
                    request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                }

                var response = client.Execute(request);
                //use response.content --> this will directly give the parsed result.
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);

            }
            catch (Exception ex)
            {
                return Json(new { status_code = "000", message=ex.Message.ToString()});
            }

        }

        public IActionResult DepartmentEdit(int id)
        {
            var item = deptList.Where(m => m.Id == id).FirstOrDefault();
            return View(item);
        }

        public async Task<IActionResult> DepartmentUpdate(IFormCollection collection)
        {

            try
            {
                DepartmentVM departmentVM = new DepartmentVM();
                DepartmentVM updatedVM = new DepartmentVM();
                await TryUpdateModelAsync<DepartmentVM>(departmentVM);
                updatedVM.Id = departmentVM.Id;
                updatedVM.Name = departmentVM.Name;
                updatedVM.Message = departmentVM.Message;
                updatedVM.Description = departmentVM.Description;
                updatedVM.Head = departmentVM.Head;
                updatedVM.Slogan = departmentVM.Slogan;
                updatedVM.UpdatedDate = DateTime.Now;
                
                var request = new RestRequest("/UpdateDepartmentDetail", Method.Put);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(DepartmentVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(updatedVM);
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

        [HttpPost]
        public async Task<IActionResult> DepartmentDelete(int id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = deptList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteDepartmentDetail");
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

        public IActionResult DepartmentAllView()
        {
            return View();
        }
    }
}
