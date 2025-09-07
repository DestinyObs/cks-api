using System;

namespace Models
{
    public class Metric
    {
        public Guid Id { get; set; }
        public string MetricData { get; set; } = "{}"; // JSON
        public string Values { get; set; } = "[]"; // JSON
    }
}
