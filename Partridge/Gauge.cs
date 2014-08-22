using System;
using System.Reflection;
using log4net;
using Partridge.Util;

namespace Partridge
{
    public class Gauge
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Func<double> func;

        public Gauge(Func<double> func)
        {
            Guard.NotNull(func);
            this.func = func;
        }

        public double Value
        {
            get
            {
                try
                {
                    return func();
                }
                catch (Exception e)
                {
                    logger.Warn(e);
                }
                return 0;
            }
        }
    }
}
