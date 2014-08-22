using System.Collections.Generic;

namespace Partridge
{
    /// <summary>
    /// TODO Capture point in time snapshot instead of references
    /// </summary>
    public class StatsSummary
    {
        private readonly IDictionary<string, AtomicLong> counters;
        private readonly IDictionary<string, Metric> metrics;
        private readonly IDictionary<string, Gauge> gauges;

        public StatsSummary(IDictionary<string, AtomicLong> counters, IDictionary<string, Metric> metrics, IDictionary<string, Gauge> gauges)
        {
            this.counters = counters;
            this.metrics = metrics;
            this.gauges = gauges;
        }

        public IDictionary<string, AtomicLong> Counters
        {
            get { return counters; }
        }

        public IDictionary<string, Metric> Metrics
        {
            get { return metrics; }
        }

        public IDictionary<string, Gauge> Gauges
        {
            get { return gauges; }
        }
    }
}
