using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class Attachments : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<AttachmentVM> attachmentVMList = new List<AttachmentVM>();
        RestClient client;

        public Attachments()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult AttachmentsAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AttachmentsAdd(IFormCollection collection)
        {
            try
            {
                AttachmentVM attachmentVM = new AttachmentVM();
                await TryUpdateModelAsync<AttachmentVM>(attachmentVM);
                attachmentVM.CreatedDate = DateTime.Now;
                attachmentVM.UpdatedDate = DateTime.Now;
                var request = new RestRequest("/AddAttachmentDetail", Method.Post);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(AttachmentVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(attachmentVM);
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

        public IActionResult AttachmentsView()
        {
            var restRequest = new RestRequest("/GetAllAttachmentDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<AttachmentVM>>>(content);
                attachmentVMList = user.data;
                foreach (var data in attachmentVMList = user.data)
                {
                    data.Attachment1 = "https://localhost:44374/" + data.Attachment1;
                }
            }
            return View(attachmentVMList);

        }



        public IActionResult AttachmentsEdit(long id = 0)
        {
            AttachmentVM attachmentVM = new AttachmentVM();
            try
            {

                attachmentVM = attachmentVMList.Where(m => m.Id == id).FirstOrDefault();

                var restRequest = new RestRequest("/GetAllAttachmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<AttachmentVM>>>(content);

                }
            }
            catch (Exception ex)
            {

            }
            return View(attachmentVM);
        }

        [HttpPost]
        public async Task<IActionResult> AttachmentsEdit(IFormCollection collection)
        {
            try
            {
                AttachmentVM attachmentVM = new AttachmentVM();
                await TryUpdateModelAsync<AttachmentVM>(attachmentVM);
                attachmentVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateAttachmentDetail", Method.Put);

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
                PropertyInfo[] properties = typeof(AttachmentVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(attachmentVM);
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
        public async Task<IActionResult> AttachmentsDelete(long id)
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
                    var uri = new Uri(apiBaseUrl + "/DeleteAttachmentDetail");
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
