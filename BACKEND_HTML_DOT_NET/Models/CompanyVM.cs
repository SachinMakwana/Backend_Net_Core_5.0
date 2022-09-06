using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class CompanyVM
    {
        public CompanyVM()
        {
            DepartmentList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string RelevantDepartments { get; set; }
        public string Image { get; set; }
        public string Logo { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
        public IList<SelectListItem> DepartmentList { get; set; }
    }
}
