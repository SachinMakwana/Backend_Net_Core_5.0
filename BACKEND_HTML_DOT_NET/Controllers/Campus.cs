using BACKEND_HTML_DOT_NET.Models;
using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Campus : Controller
    {
        public IActionResult CampusAdd()
        {
            return View();
        }


        [HttpPost]
        public IActionResult CampusAdd(IFormCollection collection)
        {
            return Json("FHRERHR");
        }
        public IActionResult CampusView()
        {
            return View();
        }
        public IActionResult CampusEdit()
        {
            return View();
        }
    }
}
