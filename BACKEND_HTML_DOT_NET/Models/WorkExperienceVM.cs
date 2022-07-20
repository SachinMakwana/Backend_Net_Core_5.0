using System;


namespace GECP_DOT_NET_API.Models
{
    public class WorkExperienceVM
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public string Title { get; set; }
        public string Organization { get; set; }
        public DateTime? FromDate { get; set; }
        public string FromDateInt { get; set; }
        public DateTime? ToDate { get; set; }
        public string ToDateInt { get; set; }
        public string Designation { get; set; }
        public string Expertise { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
