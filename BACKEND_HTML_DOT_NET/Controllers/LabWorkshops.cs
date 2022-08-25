using BACKEND_HTML_DOT_NET.Models;
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
        
        public IActionResult LabWorkshopsAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LabWorkshopsAdd(IFormCollection collection)
        {
            try
            {
                LabWorkshopVM labworkshopVM = new LabWorkshopVM();
                await TryUpdateModelAsync<LabWorkshopVM>(labworkshopVM);
                labworkshopVM.CreatedDate = DateTime.Now;
                labworkshopVM.UpdatedDate = DateTime.Now;
                var request = new RestRequest("/AddLabWorkshopDetail", Method.Post);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(LabWorkshopVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(labworkshopVM);
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

        public IActionResult LabWorkshopsView()
        {

            return View();
        }

        public IActionResult LabWorkshopsEdit()
        {
            return View();
        }

        public IActionResult LabWorkshopsUpdate()
        {
            return View();
        }

        public IActionResult LabAllView()
        {
            return View();
        }
    }
}
