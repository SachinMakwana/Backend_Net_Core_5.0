using System;

namespace GECP_DOT_NET_API.Models
{
    public class AttachmentVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Attachment1 { get; set; }
        public int PageId { get; set; }
        public bool? IsVisible { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
