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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class WomenCell : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<CommitteeVM> committeList = new List<CommitteeVM>();
        RestClient client;

        public WomenCell()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult WomenCellEdit()
        {
            CommitteeVM committeeVM = new CommitteeVM();
            try
            {
                committeeVM = committeList.Where(m => m.Id == 1).FirstOrDefault();

            }catch(Exception ex)
            {

            }
            return View(committeeVM);
        }

        [HttpPost]
        public IActionResult WomenCellEdit([FromForm] CommitteeVM womencellVM, [Optional] IFormCollection collection)
        {
            try
            {
                womencellVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("UpdateCommitteeDetail", Method.Put);

                if(collection.Files.Count() > 0)
                {
                    foreach (var file in collection.Files)
                    {
                        var memorystream = new MemoryStream();
                        file.CopyTo(memorystream);
                        var bytes = memorystream.ToArray();
                        request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                    }
                }
                else {
                    byte[] data = new byte[0];
                    request.AddFile("image", data, "NoImage");
                }

                PropertyInfo[] properties = typeof(CommitteeVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(womencellVM);
                        request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                    }
                }

                var response = client.Execute(request);
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);


            }
            catch(Exception ex)
            {
                return Json(new { status_code = "000", message = ex.Message.ToString() });
            }
        }

       // public IActionResult WomenCellView()
       // {
       //     return View();
        //}

        
        public IActionResult WomenCellView()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllCommitteeDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<CommitteeVM>>>(content);
            committeList = user.data;

            var committee = committeList.Where(m => m.CommitteeId == 1).FirstOrDefault();

            return View(committee);
        }

        public IActionResult WomenCellMembersAdd()
        {
            return View();
        }
        public IActionResult WomenCellMembersDetails()
        {
            return View();
        }
    }
}
