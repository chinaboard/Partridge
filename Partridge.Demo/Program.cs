using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Partridge;
using Partridge.Service;
namespace Partridge.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var servcie = new HttpDiagnosticsService();
            servcie.Start();
            Random rand = new Random();
            while (true)
            {
                Stats.Time("demo", rand.Next(40, rand.Next(50, 6000)));
                Thread.Sleep(rand.Next(40));
                Console.Clear();
                var metric = Stats.GetDefault().GetMetric("demo");
                Console.WriteLine(metric.Count);
                Console.WriteLine(metric.Min);
                Console.WriteLine(metric.Mean);
                Console.WriteLine(metric.Max);
            }


        }
    }
}
