using BACKEND_HTML_DOT_NET.Models;
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
    public class Committee : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<CommitteeVM> committeList = new List<CommitteeVM>();
        private static List<CommitteeMembersVM> committeeMembersList = new List<CommitteeMembersVM>();
        RestClient client;

        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        public Committee(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public IActionResult CommitteeList()
        {
            var restRequest = new RestRequest("/GetAllCommitteeDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<CommitteeVM>>>(content);
                committeList = user.data;
                foreach (var data in committeList)
                {
                    data.Image = imageBaseUrl + data.Image;
                }
            }
            return View(committeList);
        }

        public IActionResult CommitteeAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CommitteeAdd([FromForm] CommitteeVM committteeVM, [Optional] IFormCollection collection)
        {
            try
            {
                RestRequest request = new RestRequest("/AddCommitteeDetail", Method.Post);
                committteeVM.CreatedDate = DateTime.Now;
                committteeVM.UpdatedDate = DateTime.Now;

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
                PropertyInfo[] properties = typeof(CommitteeVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "null")
                    {
                        var value = property.GetValue(committteeVM);
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


        public IActionResult CommitteeEdit(long id = 0)
        {
            CommitteeVM committeeVM = new CommitteeVM();
            try
            {
                committeeVM = committeList.Where(m => m.Id == id).FirstOrDefault();

            }
            catch (Exception ex)
            {

            }
            return View(committeeVM);
        }

        [HttpPost]
        public IActionResult CommitteeEdit([FromForm] CommitteeVM committteeVM, [Optional] IFormCollection collection)
        {
            try
            {
                committteeVM.UpdatedDate = DateTime.Now;
                RestRequest request = new RestRequest("/UpdateCommitteeDetail", Method.Post);

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
                PropertyInfo[] properties = typeof(CommitteeVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "null")
                    {
                        var value = property.GetValue(committteeVM);
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
        public async Task<IActionResult> CommitteeDelete(long id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = committeList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteCommitteeDetail");
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

        public IActionResult CommitteeView()
        {
            return View();
        }

        public IActionResult CommitteeMembersList()
        {
            var restRequest = new RestRequest("/GetAllCommitteeMembersDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = client.Execute(restRequest);

            var content = response.Content;
            if (content != null)
            {
                var user = JsonConvert.DeserializeObject<ServiceResponse<List<CommitteeMembersVM>>>(content);
                committeeMembersList = user.data;
            }
            return View(committeeMembersList);
        }

        public IActionResult CommitteeMembersAdd(long id = 0)
        {
            CommitteeMembersVM member = new CommitteeMembersVM();
            try
            {
                if (id > 0)
                {
                    member = committeeMembersList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllCommitteeDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<CommitteeVM>>>(content);
                    member.CommitteeList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.CommitteeId.ToString()
                    }).ToList();
                }

                var restRequestFaculty = new RestRequest("/GetAllFacultyDetails", Method.Get);
                restRequestFaculty.AddHeader("Accept", "application/json");
                restRequestFaculty.RequestFormat = DataFormat.Json;
                RestResponse responseFaculty = client.Execute(restRequestFaculty);

                var facultys = responseFaculty.Content;

                if (facultys != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(facultys);
                    member.FacultyList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> CommitteeMembersAdd(CommitteeMembersVM member)
        {
            member.CreatedDate = DateTime.Now;
            member.UpdatedDate = DateTime.Now;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/AddCommitteeMemberDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(member), Encoding.UTF8, "application/json");

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

        public IActionResult CommitteeMembersEdit(int id)
        {
            CommitteeMembersVM member = new CommitteeMembersVM();
            try
            {
                
                    member = committeeMembersList.Where(m => m.Id == id).FirstOrDefault();
                
                var restRequest = new RestRequest("/GetAllCommitteeDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<CommitteeVM>>>(content);
                    member.CommitteeList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.CommitteeId.ToString()
                    }).ToList();
                }

                var restRequestFaculty = new RestRequest("/GetAllFacultyDetails", Method.Get);
                restRequestFaculty.AddHeader("Accept", "application/json");
                restRequestFaculty.RequestFormat = DataFormat.Json;
                RestResponse responseFaculty = client.Execute(restRequestFaculty);

                var facultys = responseFaculty.Content;

                if (facultys != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(facultys);
                    member.FacultyList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }

            }
            catch (Exception ex)
            {

            }
            return View(member);
        }

        [HttpPost]
        public async Task<IActionResult> CommitteeMembersEdit(CommitteeMembersVM member)
        {
            var updateItem = committeeMembersList.Where(m => m.Id == member.Id).FirstOrDefault();
            updateItem.CommitteeId = member.CommitteeId;
            updateItem.FacultyId = member.FacultyId;
            updateItem.Role = member.Role;
            updateItem.UpdatedDate = DateTime.Now;
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/UpdateCommitteeMemberDetail");
                    StringContent content = new StringContent(JsonConvert.SerializeObject(member), Encoding.UTF8, "application/json");

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

        [HttpPost]
        public async Task<IActionResult> CommitteeMembersDelete(int id)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(id.ToString()))
                {
                    return Json(new { message = "Invalid Record." });
                }
                var updateItem = committeeMembersList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;
                using (var client = new HttpClient())
                {
                    var uri = new Uri(apiBaseUrl + "/DeleteCommitteeMemberDetail");
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
