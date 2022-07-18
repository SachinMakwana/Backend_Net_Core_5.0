using System;

namespace GECP_DOT_NET_API.Models
{
    public class MediaLinkVM
    {
        public int Id { get; set; }
        public int DeptId { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
