using System;

namespace Partridge
{
    public static class DateTimeUtils
    {
        private const int NANOS_IN_A_MILLISECOND = 1000000;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly long epochTicks = epoch.Ticks;

        public static string ToIso8601(this long millisFromEpoch, DateTimePrecision precision)
        {
            var date = new DateTime(epochTicks + millisFromEpoch*TimeSpan.TicksPerMillisecond);
            return ToIso8601(date, precision);
        }

        public static string ToIso8601(this DateTime date, DateTimePrecision precision)
        {
            switch (precision)
            {
                case DateTimePrecision.Seconds:
                    return date.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
                default:
                    throw new InvalidOperationException();
            }
        }

        public static long MillisFromEpoch(this DateTime dateTime)
        {
            var t = dateTime - epoch;
            return (long)t.TotalMilliseconds;
        }

        public static long NanosFromEpoch(this DateTime dateTime)
        {
            return MillisFromEpoch(dateTime) / NANOS_IN_A_MILLISECOND;
        }

        public static long CurrentTimeMillis
        {
            get { return DateTime.UtcNow.MillisFromEpoch(); }
        }

        public static long CurrentTimeNanos
        {
            get { return DateTime.UtcNow.NanosFromEpoch() / NANOS_IN_A_MILLISECOND; }
        }

        public static DateTime Epoch
        {
            get { return epoch; }
        }
    }


    public enum DateTimePrecision
    {
        Seconds
    }
}
