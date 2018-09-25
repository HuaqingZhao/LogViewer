using System;

namespace LogViewer
{
    public static class UtilitiesExtention
    {
        public static string SenseString(this string me)
        {
            return LogManager.CaseSensitive ? me : me.ToUpper();
        }

        public static bool SenseEquals(this string me, string comapration)
        {
            return me.SenseString().Equals(comapration.SenseString());
        }

        public static bool SenseContains(this string me, string comapration)
        {
            return me.SenseString().Contains(comapration.SenseString());
        }
    }
}
