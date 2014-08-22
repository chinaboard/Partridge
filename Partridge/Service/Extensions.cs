using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Partridge.Util;

namespace Partridge.Service
{
    public static class Extensions
    {
        public static void Write(this IDictionary<string, object> dict, StringBuilder builder, int indent)
        {
            dict.Each((key, value) =>
            {
                if (value == null) return;

                Enumerable.Range(0, indent).Each(i => builder.Append("  "));
                if (typeof(IDictionary<string, object>).IsAssignableFrom(value.GetType()))
                {
                    builder.Append(key).Append(": ").AppendLine();
                    Write((IDictionary<string, object>)value, builder, indent + 1);
                }
                else if (typeof(string) != value.GetType() && typeof(IEnumerable).IsAssignableFrom(value.GetType()))
                {
                    builder.Append(key)
                        .Append(": (")
                        .Append(string.Join(",", from object item in (IEnumerable)value select Convert.ToString(item)))
                        .Append(")")
                        .AppendLine();
                }
                else
                {
                    builder.Append(key)
                        .Append(": ")
                        .Append(value)
                        .AppendLine();
                }
            });
        }
    }
}
