using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class VendorSpendModel
    {
        [Key, Required]
        public Guid Id { get; set; }

        public VendorSpendModel Vendor { get; set; }

        public MerchantViewModel Merchant { get; set; }

        public BuyVoucherViewModel Voucher { get; set; }
    }
}