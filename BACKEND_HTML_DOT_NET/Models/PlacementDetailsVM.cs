using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages.Html;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class PlacementDetailsVM
    {
        public PlacementDetailsVM()
        {
            DepartmentList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int DeptId { get; set; }
        public int PlacementYear { get; set; }
        public int NumberofRegisterdStudent { get; set; }
        public int PlacedStudent { get; set; }
        public int TotalStudent { get; set; }
        public int NoOfCompany { get; set; }
        public decimal HigestPackage { get; set; }
        public decimal LowestPackage { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
        public List<SelectListItem> DepartmentList { get; set; }
    }
}
