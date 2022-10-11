using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class AntiRaggingCell : Controller
    {
        public IActionResult AntiRaggingAdd()
        {
            return View();
        }
        public IActionResult AntiRaggingView()
        {
            return View();
        }
        public IActionResult AntiRaggingAttachmentsAdd()
        {
            return View();
        }
        public IActionResult AntiRaggingAttachmentsView()
        {
            return View();
        }
        public IActionResult AntiRaggingMembersAdd()
        {
            return View();
        }
        public IActionResult AntiRaggingMembersDetails()
        {
            return View();
        }
    }
}
