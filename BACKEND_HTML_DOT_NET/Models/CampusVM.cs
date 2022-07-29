using System;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class CampusVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Image { get; set; }

        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }

    }
}
