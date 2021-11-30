using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Attachments : Controller
    {
        public IActionResult AttachmentsAdd()
        {
            return View();
        }
        public IActionResult AttachmentsView()
        {
            return View();
        }
        public IActionResult AttachmentsEdit()
        {
            return View();
        }
    }
}
