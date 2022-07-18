using System;

namespace GECP_DOT_NET_API.Models
{
    public class CommitteeMembersVM
    {
        public int Id { get; set; }
        public int FacultyId { get; set; }
        public string Role { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
