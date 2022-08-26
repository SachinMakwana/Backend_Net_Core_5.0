using BACKEND_HTML_DOT_NET.Models;
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
    public class LabWorkshops : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<LabWorkshopVM> labworkshopList = new List<LabWorkshopVM>();
        RestClient client;

        public LabWorkshops()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult LabWorkshopsList()
        {
            var restRequest = new RestRequest("/GetAllLabWorkshopDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<LabWorkshopVM>>>(content);
                labworkshopList = user.data;
                foreach (var data in labworkshopList)
                {
                    data.Image = "https://localhost:44374/" + data.Image;
                }
            }
            return View(labworkshopList);
        }
        
        public IActionResult LabWorkshopsAdd(long id = 0)
        {
            LabWorkshopVM labworkshopVM = new LabWorkshopVM();
            try
            {
                if (id > 0)
                {
                    labworkshopVM = labworkshopList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var dept = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                    labworkshopVM.DepartmentList = dept.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(labworkshopVM);
        }

        [HttpPost]
        public IActionResult LabWorkshopsAdd([FromForm] LabWorkshopVM labworkshopVM, [Optional]IFormCollection collection)
        {
            try
            {
                var request = new RestRequest("/AddLabWorkshopDetail", Method.Post);
                labworkshopVM.CreatedDate = DateTime.Now;
                labworkshopVM.UpdatedDate = DateTime.Now;
                if(collection.Files.Count() > 0)
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
                PropertyInfo[] properties = typeof(LabWorkshopVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "DepartmentList")
                    {
                        var value = property.GetValue(labworkshopVM);
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


        public IActionResult LabWorkshopsEdit(long id = 0)
        {
            LabWorkshopVM labworkshopVM = new LabWorkshopVM();
            try
            {

                labworkshopVM = labworkshopList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                    labworkshopVM.DepartmentList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(labworkshopVM);
        }

        [HttpPost]
        public IActionResult LabWorkshopsEdit([FromForm] LabWorkshopVM labworkshopVM, [Optional] IFormCollection collection)
        {
            try
            {
                labworkshopVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateLabWorkshopDetail", Method.Put);

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
                PropertyInfo[] properties = typeof(DepartmentVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "DepartmentList")
                    {
                        var value = property.GetValue(labworkshopVM);
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
        public async Task<IActionResult> LabWorkshopDelete(int id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = labworkshopList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteLabWorkshopDetail");
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

        public IActionResult LabWorkshopsView()
        {

            return View();
        }

        public IActionResult LabAllView()
        {
            return View();
        }
    }
}
