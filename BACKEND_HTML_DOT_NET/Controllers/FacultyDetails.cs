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
    public class FacultyDetails : Controller
    {
        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<FacultyDetailsVM> facultyDetailsVM = new List<FacultyDetailsVM>();

        public IActionResult FacultyAdd()
        {

            var restClient = new RestClient(apiBaseUrl);
            var restRequest = new RestRequest("/GetAllFacultyDetails", Method.Get);
            restRequest.AddHeader("Accept", "application/json");
            restRequest.RequestFormat = DataFormat.Json;

            RestResponse response = restClient.Execute(restRequest);

            var content = response.Content;

            var user = JsonConvert.DeserializeObject<ServiceResponse<List<FacultyDetailsVM>>>(content);
            return View(user.data);
        }
        public IActionResult FacultyView(int id = 0)
        {

            FacultyDetailsVM newsVM = new FacultyDetailsVM();
            newsVM = facultyDetailsVM.Where(m => m.Id == id).FirstOrDefault();
            return View(newsVM);
        }
        public IActionResult FacultyEdit()
        {
            return View();
        }
        public IActionResult FacultyDetail()
        {
            return View();
        }
    }
}
