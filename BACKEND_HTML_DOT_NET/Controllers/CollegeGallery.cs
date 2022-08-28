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
    public class CollegeGallery : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private static List<GalleryVM> galleryVMList = new List<GalleryVM>();
        RestClient client;


        public CollegeGallery()
        {
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult GalleryList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllGalleryDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<GalleryVM>>>(content);
            galleryVMList = user.data;
            return View(galleryVMList);

        }
        public IActionResult GalleryView(int id=0)
        {
            return View();

        }

        public IActionResult GalleryAdd()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GalleryAdd(IFormCollection collection)
        {

            try
            {
                GalleryVM galleryVM = new GalleryVM();
                await TryUpdateModelAsync<GalleryVM>(galleryVM);
                galleryVM.CreatedDate = DateTime.Now;
                galleryVM.UpdatedDate = DateTime.Now;
                var request = new RestRequest("/AddGalleryDetail", Method.Post);
                //add files to request
                foreach (var file in collection.Files)
                {
                    var memorystream = new MemoryStream();
                    file.CopyTo(memorystream);
                    var bytes = memorystream.ToArray();
                    request.AddFile(file.Name.ToString(), bytes, file.FileName.ToString());
                }

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(GalleryVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    var value = property.GetValue(galleryVM);
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
        public async Task<IActionResult> GalleryDelete(long id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = galleryVMList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteGalleryDetail");
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
        public IActionResult DepartmentView()
        {
            return View();
        }

        public IActionResult DepartmentAllView()
        {
            return View();
        }
    }

}

