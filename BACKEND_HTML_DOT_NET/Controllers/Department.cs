using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Department : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<DepartmentVM> deptVMList = new List<DepartmentVM>();

        public IActionResult DepartmentList()
        {
            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllDepartmentDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<DepartmentVM>>>(content);
            deptVMList = user.data;
            return View(deptVMList);
        }
        public IActionResult DepartmentAdd()
        {
            return View();
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
