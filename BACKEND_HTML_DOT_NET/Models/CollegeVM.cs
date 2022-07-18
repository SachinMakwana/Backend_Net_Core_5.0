using System;
using System.Collections.Generic;

namespace GECP_DOT_NET_API.Models
{
    public class CollegeVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Principal { get; set; }
        public string PrincipalMessage { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedDateInt { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedDateInt { get; set; }
    }
}
