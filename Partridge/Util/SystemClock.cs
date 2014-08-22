using System;

namespace Partridge.Util
{
    public static class SystemClock
    {
        public static Func<DateTime> Now = () => DateTime.Now;
        public static Func<DateTime> UtcNow = () => DateTime.UtcNow;

        public static long Minutes()
        {
            return Convert.ToInt64((UtcNow() - DateTimeUtils.Epoch).TotalMinutes);
        }

        public static long Seconds()
        {
            return Convert.ToInt64((UtcNow() - DateTimeUtils.Epoch).TotalSeconds);
        }

        public static long Millis()
        {
            return Convert.ToInt64((UtcNow() - DateTimeUtils.Epoch).TotalMilliseconds);
        }

        public static DateTime Minus(TimeSpan ts)
        {
            return UtcNow() - ts;
        }

        public static void Advance(TimeSpan timeSpan)
        {
            var current = Now();
            Now = () => current + timeSpan;
        }

        public static void WaitFor(long millis)
        {
            var until = UtcNow() + TimeSpan.FromMilliseconds(millis);
            while (until < UtcNow())
            {
                //spin
            }
        }

        public static void WithFrozen(Action action)
        {
            using (Freeze())
            {
                action();
            }
        }

        public static IDisposable Freeze()
        {
            var now = UtcNow();
            UtcNow = () => now;
            return new DisposableAction(() => { UtcNow = () => DateTime.UtcNow; });
        }
    }
}
