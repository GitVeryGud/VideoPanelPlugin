using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicBeePlugin
{
    public static class Utilities
    {
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
    }
}
