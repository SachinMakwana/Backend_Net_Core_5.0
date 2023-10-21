using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class PlacementVM
    {
        public PlacementVM()
        {
            CompanyList = new List<SelectListItem>();
            DepartmentList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string StudentPic { get; set; }
        public int DeptId { get; set; }
        public long AnnualPackage { get; set; }
        public long MonthlyPackage { get; set; }
        public int CompanyId { get; set; }
        public DateTime PlacementDate { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }

        public IList<SelectListItem> CompanyList { get; set; }
        public IList<SelectListItem> DepartmentList { get; set; }
    }
}
