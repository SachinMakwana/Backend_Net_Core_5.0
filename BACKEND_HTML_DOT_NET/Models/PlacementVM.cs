using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GECP_DOT_NET_API.Models
{
    public class PlacementVM
    {
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
    }
}
