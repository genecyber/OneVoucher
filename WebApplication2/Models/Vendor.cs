using System.ComponentModel;

namespace WebApplication2.Models
{
    public enum Vendor
    {
        [Description("Perfect Money")]
        PerfectMoney = 0,
        [Description("Ego Pay")]
        EgoPay,
        [Description("Solid Trust Pay")]
        SolidTrustPay,
        PAYEER,
        [Description("Western Union")]
        WesternUnion,
        WebMoney,
        MoneyGram,
        Bank,
        [Description("Interac E­-Transfer")]
        InteracETransfer
    }
}