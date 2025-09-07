using System;
using System.Collections.Generic;

namespace Models
{
    public class BillingPeriod
    {
        public Guid Id { get; set; }
        public Guid TenantId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalCost { get; set; }
        public string Currency { get; set; } = null!;
        public ICollection<BillingUsage> Usage { get; set; } = new List<BillingUsage>();
    }
}
