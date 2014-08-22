using System.Threading;

namespace Partridge
{
    public class AtomicLong
    {
        public static readonly AtomicLong MinValue = new AtomicLong(long.MinValue);
        public static readonly AtomicLong MaxValue = new AtomicLong(long.MaxValue);

        private long value;

        public AtomicLong(long value)
        {
            this.value = value;
        }

        public long Value
        {
            get { return Interlocked.Read(ref value); }
        }

        public AtomicLong Increment()
        {
            Interlocked.Increment(ref value);
            return this;
        }

        public long IncrementAndGet()
        {
            return Interlocked.Increment(ref value);
        }

        public AtomicLong Increment(int n)
        {
            Interlocked.Add(ref value, n);
            return this;
        }

        public long IncrementAndGet(int n)
        {
            return Interlocked.Add(ref value, n);
        }

        public bool Equals(AtomicLong other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.value == value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (AtomicLong)) return false;
            return Equals((AtomicLong) obj);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }
    }
}
