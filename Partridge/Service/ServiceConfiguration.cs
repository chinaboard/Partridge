using System;
using System.Configuration;

namespace Partridge.Service
{
    public interface IServiceConfiguration
    {
        int Port { get; }
    }

    public class ServiceConfiguration : ConfigurationSection, IServiceConfiguration
    {
        [ConfigurationProperty("myAttrib1", DefaultValue = "7400", IsRequired = true)]
        [IntegerValidator(MinValue = 1024, MaxValue = 65535)]
        public int Port
        {
            get { return Convert.ToInt32(this["port"]); }
            set { this["port"] = value; }
        }
    }

    public class StaticServiceConfiguration : IServiceConfiguration
    {
        public StaticServiceConfiguration()
        {
            Port = 7000;
        }

        public StaticServiceConfiguration(int port)
        {
            Port = port;
        }

        public int Port { get; private set; }
    }
}
