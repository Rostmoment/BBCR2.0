using System;
using System.Collections.Generic;
using System.Text;

namespace BBCR.API.Extensions
{
    public static class BoolExtensions
    {
        public static bool FalseIfNull(this bool? value)
        {
            if (value == null)
                return false;

            return value.Value;
        }

        public static bool TrueIfNull(this bool? value)
        {
            if (value == null)
                return true;

            return value.Value;
        }
    }
}
