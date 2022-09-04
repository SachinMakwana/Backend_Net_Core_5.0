using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace BACKEND_HTML_DOT_NET.Models
{
    public class CommitteeMembersVM
    {
        public CommitteeMembersVM()
        {
            CommitteeList = new List<SelectListItem>();
            FacultyList = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public int CommitteeId { get; set; }
        public string Role { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
        public IList<SelectListItem> CommitteeList { get; set; }
        public IList<SelectListItem> FacultyList { get; set; }
    }
}
