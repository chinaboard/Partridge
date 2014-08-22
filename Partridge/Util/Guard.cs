using System;

namespace Partridge.Util
{
    public static class Guard
    {
        public static void NotNull(object o)
        {
            if (o == null) throw new ArgumentNullException();
        }
    }
}
