using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jynx.Common.Chronography
{
    public static class ChronographyExtensions
    {
        private const string _iso8601Format = "yyyy-MM-ddTHH:mm:ss.fffffffZ";

        public static string ToIso8601String(this DateTime dateTime)
            => dateTime.ToString(_iso8601Format);

        public static string ToIso8601String(this DateTimeOffset dateTime)
            => dateTime.ToString(_iso8601Format);
    }
}
