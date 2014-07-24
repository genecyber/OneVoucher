using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Helpers;

namespace WebApplication2.Models
{
    public class BuyVoucherViewModel
    {
        [Key, Required]
        public Guid Id { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "Vendor")]
        [Required]
        public Vendor VendorSelect { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Amount")]
        [Range(50, 999, ErrorMessage = "Please provide a valid load amount between $50 and $999")]
        [Required]
        public int AmoutToLoad { get; set; }

        public Boolean Paid { get; set; }

        public Boolean Spent { get; set; }

        public Boolean CashedOutToMerchant { get; set; }

        public string LoadTxId { get; set; }

        public Guid VoucherGuid { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public String EmailAddress { get; set; }

        public Guid MerchantSpentAtId { get; set; }

        [Display(Name = "Date of Purchase"), DataType(DataType.Date)]
        public DateTime DateOfPurchase { get; set; }

        [Display(Name = "Date Spent"), DataType(DataType.Date)]
        public DateTime? DateSpent { get; set; }

        [Display(Name = "Date CashedOut"), DataType(DataType.Date)]
        public DateTime? DatePaidout { get; set; }
    }
}