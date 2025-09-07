using System;

namespace Models
{
    public class BillingUsage
    {
        public Guid Id { get; set; }
        public Guid BillingPeriodId { get; set; }
        public string ResourceType { get; set; } = null!;
        public decimal Amount { get; set; }
        public string Unit { get; set; } = null!;
        public decimal Cost { get; set; }
        public string Currency { get; set; } = null!;
        public BillingPeriod? BillingPeriod { get; set; }
    }
}
