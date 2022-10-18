using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    [Authorize]
    public class AntiRaggingCell : Controller
    {
        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient client;
        public AntiRaggingCell(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }

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
