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
    }
}
