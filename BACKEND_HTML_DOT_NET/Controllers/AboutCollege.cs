using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class AboutCollege : Controller
    {

        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<CollegeVM> CollegeVMList = new List<CollegeVM>();
        
        public IActionResult AboutCollegeAdd()
        {
            return View();
        }

        public IActionResult AboutCollegeView(int id = 0)
        {
            CollegeVM collegeVM = new CollegeVM();
            collegeVM = CollegeVMList.Where(m => m.Id == id).FirstOrDefault();

            return View(collegeVM);
        }
    }
}
