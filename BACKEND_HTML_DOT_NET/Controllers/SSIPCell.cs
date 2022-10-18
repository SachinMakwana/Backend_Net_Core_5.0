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
    public class SSIPCell : Controller
    {
        private readonly AppIdentitySettings _config;
        private string apiBaseUrl = string.Empty;
        private string imageBaseUrl = string.Empty;
        RestClient client;
        public SSIPCell(IOptions<AppIdentitySettings> appIdentitySettingsAccessor)
        {

            _config = appIdentitySettingsAccessor.Value;

            apiBaseUrl = _config.apiBaseUrl;
            imageBaseUrl = _config.imageBaseUrl;
            client = new RestClient(apiBaseUrl);
        }
        public IActionResult SSIPAdd()
        {
            return View();
        }
        public IActionResult SSIPView()
        {
            return View();
        }
        public IActionResult SSIPAttachmentsAdd()
        {
            return View();
        }
        public IActionResult SSIPAttachmentsView()
        {
            return View();
        }
        public IActionResult SSIPMemberAdd()
        {
            return View();
        }
        public IActionResult SSIPMemberDetails()
        {
            return View();
        }
    }
}
