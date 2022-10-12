using System;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class DesignationVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Class { get; set; }
        public string Payband { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
