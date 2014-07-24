using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class MerchantViewModel
    {
        [Key]
        public string UserId { get; set; }

        public Guid SecretSauce { get; set; }

        [Url(ErrorMessage = "Please enter a valid url")]
        [DisplayName("Image URL (185x85 px)")]
        [DataType(DataType.Url)]
        public string ImageUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid url")]
        [DisplayName("Website URL")]
        [DataType(DataType.Url)]
        public string WebsiteUrl { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Website Name")]
        public string WebsiteName { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Website Description")]
        public string WebsiteDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Vendor to accept payment from")]
        public Vendor VendorSelect { get; set; }

        [DataType(DataType.Text)]
        [DisplayName("Vendor Details")]
        public string VendorDetails { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please enter a valid email")]
        [DisplayName("Email Address")]
        public string EmailAddress { get; set; }

        public Boolean Approved { get; set; }
    }

}