using System;

namespace WebApplication2.Models
{
    public class SpendRequest
    {
        public string Sauce { get; set; }
        public string EncryptedVoucher { get; set; }
    }

    public class SpendResponse
    {
        public string EncryptedMerchantId { get; set; }
        public Boolean Success { get; set; }
        public string SpendId { get; set; }
    }
}