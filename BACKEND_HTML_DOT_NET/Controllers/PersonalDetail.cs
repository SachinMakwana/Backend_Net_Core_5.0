using BACKEND_HTML_DOT_NET.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class PersonalDetail : Controller
    {
        HttpClient hc = new HttpClient();
        private static List<PersonalDetailVM> personalDetailList = new List<PersonalDetailVM>();
        private static List<FacultyDetailsVM> facultyList = new List<FacultyDetailsVM>();


        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient restClient;
        public PersonalDetail(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;
            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            restClient = new RestClient(apiBaseUrl);
        }

        public IActionResult PersonalDetailList()
        {
            try
            {

                var restRequest = new RestRequest("/GetAllPersonalDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;

                RestResponse response = restClient.Execute(restRequest);

                var content = response.Content;

                var user = JsonConvert.DeserializeObject<ServiceResponse<List<PersonalDetailVM>>>(content);
                personalDetailList = user.data;
                foreach (var data in user.data)
                {
                    data.DateOfBirth = data.Dob.ToLongDateString();
                }
                return View(personalDetailList);
            }
            catch (Exception)
            {
                return View(null);
            }
        }

        public IActionResult PersonalDetailAdd(long id = 0)
        {
            PersonalDetailVM personalDetailVM = new PersonalDetailVM();
            try
            {
                if (id > 0)
                {
                    personalDetailVM = personalDetailList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = restClient.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                    personalDetailVM.FacultySelectList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(personalDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDetailAdd(PersonalDetailVM personalDetailVM)
        {
            try
            {
                RestRequest request = new RestRequest("/AddPersonalDetail", Method.Post);
                personalDetailVM.CreatedDate = DateTime.Now;
                personalDetailVM.UpdatedDate = DateTime.Now;

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(PersonalDetailVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(personalDetailVM);
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

        public IActionResult PersonalDetailUpdate(int id)
        {
            PersonalDetailVM personalDetailVM = new PersonalDetailVM();
            try
            {
                if (id > 0)
                {
                    personalDetailVM = personalDetailList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = restClient.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                    personalDetailVM.FacultySelectList = user.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(personalDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> PersonalDetailUpdate(PersonalDetailVM personalDetailVM)
        {
            var updateItem = personalDetailList.Where(m => m.Id == personalDetailVM.Id).FirstOrDefault();
            updateItem.FacultyId = personalDetailVM.FacultyId;
            updateItem.FirstName = personalDetailVM.FirstName;
            updateItem.MiddleName = personalDetailVM.MiddleName;
            updateItem.LastName = personalDetailVM.LastName;
            updateItem.Dob = personalDetailVM.Dob;
            updateItem.MaritalStatus = personalDetailVM.MaritalStatus;
            updateItem.CurrentAddress = personalDetailVM.CurrentAddress;
            updateItem.PermanentAddress = personalDetailVM.PermanentAddress;
            updateItem.ContactNumber = personalDetailVM.ContactNumber;
            updateItem.WhatsAppNumber = personalDetailVM.WhatsAppNumber;
            updateItem.EmergencyContactNumber = personalDetailVM.EmergencyContactNumber;
            updateItem.Email = personalDetailVM.Email;
            updateItem.HighestQualification = personalDetailVM.HighestQualification;
            updateItem.AreaOfSpecialization = personalDetailVM.AreaOfSpecialization;
            updateItem.TeachingExperience = personalDetailVM.TeachingExperience;

            try
            {
                RestRequest request = new RestRequest("/UpdatePersonalDetail", Method.Post);
                personalDetailVM.CreatedDate = DateTime.Now;
                personalDetailVM.UpdatedDate = DateTime.Now;

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(PersonalDetailVM).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property.Name.ToString() != "FacultySelectList")
                    {
                        var value = property.GetValue(personalDetailVM);
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
        [HttpPost]
        public async Task<IActionResult> PersonalDetailDelete(int id)
        {
            
            try
            {
                RestRequest request = new RestRequest("/DeletePersonalDetail", Method.Post);
                var updateItem = personalDetailList.Where(m => m.Id == id).FirstOrDefault();
                updateItem.IsDeleted = true;
                updateItem.UpdatedDate = DateTime.Now;

                //iterate and add model to request as parameter
                PropertyInfo[] properties = typeof(PersonalDetailVM).GetProperties();
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

        public IActionResult PersonalDetailView(int id)
        {
            dynamic mymodel = new ExpandoObject();
            PersonalDetailVM personalDetailVM = new PersonalDetailVM();
            try
            {
                if (id > 0)
                {
                    personalDetailVM = personalDetailList.Where(m => m.Id == id).FirstOrDefault();
                }
                var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;
                RestResponse response = restClient.Execute(restRequest);

                var content = response.Content;
                if (content != null)
                {
                    var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
                    facultyList = user.data;
                    foreach (var data in facultyList)
                    {
                        data.Image = imageBaseUrl + data.Image;
                    }
                }
                mymodel.facultyList = facultyList;
                mymodel.personalDetail = personalDetailVM;
                return View(mymodel);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
    }
}
