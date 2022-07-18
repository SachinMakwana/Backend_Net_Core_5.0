using System;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class GalleryVM
    {
        public int Id { get; set; }
        public int GalleryTagId { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Image { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
