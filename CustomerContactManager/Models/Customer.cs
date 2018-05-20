using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerContactManager.Models
{
    public class BaseCustomer
    {
        public string Id { get; set; }

        [Required]
        [Display (Name = "Name")]
        public string Name { get; set; }
    }

    public class Customer : BaseCustomer
    {
        [Required]
        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [Required]
        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        //public List<CustomerContact> Contacts { get; set; }
    }

    public class CustomerContact : BaseCustomer
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "ContactNumber")]
        public string ContactNumber { get; set; }

        [Required]
        [Display(Name = "CustomerId")]
        public string CustomerId { get; set; }
    }
}