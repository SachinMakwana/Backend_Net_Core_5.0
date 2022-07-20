using System;

namespace GECP_DOT_NET_API.Models
{
    public class MissionVM
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string Description { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
