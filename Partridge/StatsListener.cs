using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Partridge.Util;

namespace Partridge
{
    public class StatsListener
    {
        private static readonly Lazy<StatsListener> defaultListener = new Lazy<StatsListener>(() => new StatsListener(Stats.GetDefault()), LazyThreadSafetyMode.ExecutionAndPublication);

        private readonly StatsCollection collection;
        private readonly ConcurrentDictionary<string, Metric> metrics = new ConcurrentDictionary<string, Metric>();
        private readonly ConcurrentDictionary<string, AtomicLong> lastCounters = new ConcurrentDictionary<string, AtomicLong>();

        public static StatsListener Default
        {
            get { return defaultListener.Value; }
        }

        public StatsListener(StatsCollection collection)
        {
            Guard.NotNull(collection);
            this.collection = collection;
            collection.AddListener(this);
            collection.Counters.Each(kv => lastCounters[kv.Key] = kv.Value);
        }

        public Metric GetMetric(string tag)
        {
            return metrics.GetOrAdd(tag, k => new Metric());
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public IDictionary<string, AtomicLong> GetCounters()
        {
            var deltas = new Dictionary<string, AtomicLong>();
            foreach (var kv in collection.Counters)
            {  
                deltas[kv.Key] = Stats.Delta(lastCounters.GetOrAdd(kv.Key, new AtomicLong(0)), kv.Value);
                lastCounters[kv.Key] = kv.Value;
            }
            return deltas;
        }

        public IDictionary<string, Gauge> GetGauges()
        {
            return collection.Gauges;
        }

        public IDictionary<string, Metric> GetMetrics()
        {
            var timingStatistics = new Dictionary<string, Metric>();
            metrics.Each(kv =>
            {
                var metric = kv.Value;
                timingStatistics[kv.Key] = metric.Snapshot();
                metric.Clear();
            });
            return timingStatistics;
        }

        public StatsSummary GetSummary()
        {
            return new StatsSummary(GetCounters(), GetMetrics(), collection.Gauges);
        }
    }
}
