using System;
using System.Collections.Generic;

#nullable disable

namespace BACKEND_HTML_DOT_NET.Models
{
    public partial class SubjectVM
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Department { get; set; }
        public int Semester { get; set; }
        public string Subject1 { get; set; }
        public string Acronym { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
