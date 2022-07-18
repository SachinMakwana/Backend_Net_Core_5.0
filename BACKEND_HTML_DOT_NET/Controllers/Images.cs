using GECP_DOT_NET_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Controllers
{
    public class Images : Controller
    {

        private string apiBaseUrl = "https://localhost:44374/api";
        HttpClient hc = new HttpClient();
        private List<GalleryVM> galleryListVM = new List<GalleryVM>();
        public IActionResult ImagesView(int id=0)
        {
            GalleryVM galleryVM = new GalleryVM();
            galleryVM = galleryListVM.Where(m => m.Id == id).FirstOrDefault();

            return View(galleryVM);
        }
        public IActionResult ImagesAdd()
        {
            return View();
        }

    }
}
