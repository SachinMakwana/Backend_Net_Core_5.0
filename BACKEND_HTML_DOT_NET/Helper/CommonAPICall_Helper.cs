using BACKEND_HTML_DOT_NET.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages.Html;

namespace BACKEND_HTML_DOT_NET.Helper
{
    public class CommonAPICall_Helper
    {
        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient client;

        public CommonAPICall_Helper()
        {
           
        }
        public CommonAPICall_Helper(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {
            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

        public ServiceResponse<List<CompanyVM>> GetCompanyList()
        {
            ServiceResponse<List<CompanyVM>> responseData = new ServiceResponse<List<CompanyVM>>();
            try
            {
                var restRequest = new RestRequest("/GetAllCompanyDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;

                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                responseData = JsonConvert.DeserializeObject<ServiceResponse<List<CompanyVM>>>(content);
            }
            catch(Exception ex)
            {

            }
            return responseData;
        }

        public IList<SelectListItem> GetCompanySelectList()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var restRequest = new RestRequest("/GetAllCompanyDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;

                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                var responseData = JsonConvert.DeserializeObject<ServiceResponse<List<CompanyVM>>>(content);
                if (responseData.data != null)
                {
                    list = responseData.data.Select(m => new SelectListItem()
                    {
                        Text = m.Title,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        public ServiceResponse<List<DepartmentVM>> GetDepartmentList()
        {
            ServiceResponse<List<DepartmentVM>> responseData = new ServiceResponse<List<DepartmentVM>>();
            try
            {
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;

                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                responseData = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
            }
            catch (Exception ex)
            {

            }
            return responseData;
        }

        public IList<SelectListItem> GetDepartmentSelectList()
        {
            IList<SelectListItem> list = new List<SelectListItem>();
            try
            {
                var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
                restRequest.AddHeader("Accept", "application/json");
                restRequest.RequestFormat = DataFormat.Json;

                RestResponse response = client.Execute(restRequest);

                var content = response.Content;
                var responseData = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
                if (responseData.data != null)
                {
                    list = responseData.data.Select(m => new SelectListItem()
                    {
                        Text = m.Name,
                        Value = m.Id.ToString()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return list;
        }
    }
}
