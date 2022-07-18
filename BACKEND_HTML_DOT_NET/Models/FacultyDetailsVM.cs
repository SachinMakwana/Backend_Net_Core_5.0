using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class FacultyDetailsVM
    {
            public int Id { get; set; }
            public string Name { get; set; }
            public int DeptId { get; set; }
            public int DesignationId { get; set; }
            public bool? IsDeleted { get; set; }
            public DateTime? CreatedDate { get; set; }
            public string CreatedDateInt { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string UpdatedDateInt { get; set; }
    }

}
