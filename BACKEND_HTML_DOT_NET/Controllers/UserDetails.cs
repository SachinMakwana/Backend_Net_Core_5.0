using BACKEND_HTML_DOT_NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Web.Helpers;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class UserDetails : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<PersonalDetailVM> personalDetailList = new List<PersonalDetailVM>();
        private static List<UserDetailVM> usersList = new List<UserDetailVM>();


        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient restClient;
        public UserDetails(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;
            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            restClient = new RestClient(apiBaseUrl);
        }
        public IActionResult UserDetailList()
        {
            try
            {
                var restRequest = new RestRequest("/GetAllUsersDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;

                RestResponse response = restClient.Execute(restRequest);

                var content = response.Content;

                var user = JsonConvert.DeserializeObject<ServiceResponse<List<UserDetailVM>>>(content);
                usersList = user.data;
                return View(usersList);
            }
            catch (Exception)
            {
                return View(null);
            }
        }
        public IActionResult UserDetailAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserDetailAdd(UserDetailVM userDetail)
        {
            try
            {
                string salt = Crypto.GenerateSalt();
                string password = userDetail.Password + salt;
                string hashedPassword = Crypto.HashPassword(password);

                RestRequest request = new RestRequest("/AddUsersDetail", Method.Post);
                userDetail.SaltKey = salt;
                userDetail.Password = hashedPassword;
                userDetail.CreatedDate = DateTime.Now;
                userDetail.UpdatedDate = DateTime.Now;

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(UserDetailVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(userDetail);
                        request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                    }
                }

                var response = restClient.Execute(request);
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);

            }
            catch (Exception ex)
            {
                return Json(new { status_code = "000", message = ex.Message.ToString() });
            }
        }
        public IActionResult UserDetailUpdate(int id)
        {
            UserDetailVM userdetail = new UserDetailVM();
            try
            {
                if (id > 0)
                {
                    userdetail = usersList.Where(m => m.Id == id).FirstOrDefault();
                }
                
            }
            catch (Exception ex)
            {

            }
            return View(userdetail);
        }
        [HttpPost]
        public IActionResult UserDetailUpdate(UserDetailVM updatedDetail)
        {
            try
            {
                string salt = Crypto.GenerateSalt();
                string password = updatedDetail.Password + salt;
                string hashedPassword = Crypto.HashPassword(password);

                RestRequest request = new RestRequest("/UpdateUsersDetail", Method.Post);
                updatedDetail.SaltKey = salt;
                updatedDetail.Password = hashedPassword;
                updatedDetail.CreatedDate = DateTime.Now;
                updatedDetail.UpdatedDate = DateTime.Now;

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(UserDetailVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(updatedDetail);
                        request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                    }
                }

                var response = restClient.Execute(request);
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);

            }
            catch (Exception ex)
            {
                return Json(new { status_code = "000", message = ex.Message.ToString() });
            }
        }
        public IActionResult UserDetailDelete(int id)
        {
            try
            {
                RestRequest request = new RestRequest("/DeleteUsersDetail", Method.Post);
                var updateItem = usersList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(UserDetailVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(updateItem);
                        request.AddParameter(property.Name.ToString(), value == null ? "" : value.ToString());
                    }
                }

                var response = restClient.Execute(request);
                ServiceResponse<bool> serviceResponse = JsonConvert.DeserializeObject<ServiceResponse<bool>>(response.Content);
                return Json(serviceResponse);

            }
            catch (Exception ex)
            {
                return Json(new { status_code = "000", message = ex.Message.ToString() });
            }
        }
        public IActionResult UserDetailView()
        {
            return View();
        }
    }
}
