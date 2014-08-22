using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Partridge.Util;

namespace Partridge
{
    public class StatsCollection
    {
        private readonly ConcurrentDictionary<string, AtomicLong> counters = new ConcurrentDictionary<string,AtomicLong>();
        private readonly ConcurrentDictionary<string, FanoutMetric> metrics = new ConcurrentDictionary<string, FanoutMetric>();
        private readonly ConcurrentDictionary<string, Gauge> gauges = new ConcurrentDictionary<string, Gauge>();
        private readonly ConcurrentDictionary<StatsListener, object> listeners = new ConcurrentDictionary<StatsListener, object>();

        public StatsCollection() : this(false)
        {
        }

        public StatsCollection(bool includeDefaultGauges)
        {
            IncludeDefaultGauges = includeDefaultGauges;
        }

        public bool IncludeDefaultGauges { get; private set; }

        public ConcurrentDictionary<string, AtomicLong> Counters
        {
            get { return counters; }
        }

        public ConcurrentDictionary<string, FanoutMetric> Metrics
        {
            get { return metrics; }
        }

        public ConcurrentDictionary<string, Gauge> Gauges
        {
            get { return gauges; }
        }

        public ConcurrentDictionary<StatsListener, object> Listeners
        {
            get { return listeners; }
        }

        public void AddListener(StatsListener listener)
        {
            lock (this)
            {
                listeners.TryAdd(listener, new object());
                foreach (var kv in metrics)
                {
                    kv.Value.AddFanout(listener.GetMetric(kv.Key));
                }
            }
        }

        public FanoutMetric GetMetric(string key)
        {
            FanoutMetric metric;
            if (!metrics.TryGetValue(key, out metric))
            {
                metric = new FanoutMetric();
                lock (this)
                {
                    listeners.Each(l => metric.AddFanout(l.Key.GetMetric(key)));
                }
                metric = metrics.AddOrUpdate(key, metric, (k, m) => metric);
            }
            return metric;
        }

        public void AddGauge(string name, Gauge gauge)
        {
            gauges.AddOrUpdate(name, gauge, (n, g) => gauge);
        }

        public void DeleteGauge(string name)
        {
            Gauge value;
            gauges.TryRemove(name, out value);
        }

        public void RecordMetric(string name, long n)
        {
            var metric = GetMetric(name);
            metrics.AddOrUpdate(name, k => AddFanout(metric, n), (key, stat) =>
            {
                stat.Add(n);
                return stat;
            });
        }

        public void RecordMetric(string name, Action action)
        {
            RecordMetric(new[] { name }, action);
        }

        public void RecordMetric(string[] names, Action action)
        {
            var watch = new Stopwatch();
            watch.Start();
            action();
            watch.Stop();
            names.Each(n => RecordMetric(n, watch));
        }

        public void RecordMetric(string name, Stopwatch timer)
        {
            RecordMetric(new[] { name }, timer);
        }

        public void RecordMetric(string[] names, Stopwatch timer)
        {
            if (timer.IsRunning) timer.Stop();
            foreach (var name in names)
            {
                var metric = GetMetric(name);
                metrics.AddOrUpdate(name, k => AddFanout(metric, timer.ElapsedMilliseconds), (key, stat) =>
                {
                    stat.Add(timer.ElapsedMilliseconds);
                    return stat;
                });
            }
        }

        public T RecordMetric<T>(string name, Func<T> func)
        {
            return RecordMetric(new[] { name }, func);
        }

        public T RecordMetric<T>(string[] name, Func<T> func)
        {
            var watch = new Stopwatch();
            watch.Start();
            var answer = func();
            watch.Stop();
            name.Each(n => RecordMetric(n, watch));
            return answer;
        }

        public Metric DeleteTime(string name)
        {
            FanoutMetric value;
            metrics.TryRemove(name, out value);
            return value;
        }

        public long Increment(string name, long count)
        {
            return Increment(new[] { name }, count);
        }

        public long Increment(string[] names, long count)
        {
            long answer = 0;
            foreach (var name in names)
            {
                var counter = new AtomicLong(count);
                answer = counters.AddOrUpdate(name, counter, (k, v) => new AtomicLong(v.Value + counter.Value)).Value;
            }
            return answer;
        }

        public long Increment(string name)
        {
            return Increment(name, 1);
        }

        public long Increment(string[] names)
        {
            return Increment(names, 1);
        }

        public AtomicLong DeleteCounter(string name)
        {
            AtomicLong counter;
            counters.TryRemove(name, out counter);
            return counter;
        }

        public void ClearAll()
        {
            counters.Clear();
            gauges.Clear();
            metrics.Clear();
            listeners.Clear();
        }

        private static FanoutMetric AddFanout(FanoutMetric metric, long i)
        {
            metric.Add(i);
            return metric;
        }
    }
}
