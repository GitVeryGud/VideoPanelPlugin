using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBeePlugin
{
    // Class for utility methods
    public static class Utilities
    {
        #region WriteLines that only work in debug builds 
        public static void debugPrint(string message)
        {
#if DEBUG
            Console.WriteLine(message);
#endif
        }

        public static void debugPrint(string message, bool condition)
        {
#if DEBUG
            if (condition)
            {
                Console.WriteLine(message);
            }
#endif
        }

        public static void debugPrint(string messageTrue, string messageFalse, bool condition)
        {
#if DEBUG
            if (condition)
            {
                Console.WriteLine(messageTrue);
            }

            else Console.WriteLine(messageFalse);
#endif
        }
        #endregion

        /// <summary>
        /// Takes string in minute:second format and returns it as seconds.
        /// </summary>
        /// <returns></returns>
        public static long ParseStartTime(string time)
        {
            if (time == "" || time == null) return 0;

            int index = time.IndexOf(":");
            string minutes = time.Substring(0, index);
            string seconds = time.Substring(index + 1);

            long time_seconds = long.Parse(minutes) * 60 + long.Parse(seconds);

            debugPrint("Parsed start time: " + time_seconds);

            return time_seconds;
        }
    }
}
