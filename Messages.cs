using System;
using System.Collections.Generic;

namespace TextAdv
{
    public class Messages
    {
        public static List<string> msgs = new List<string>();

        public static void ShowMsgs()
        {
            foreach(string str in msgs)
            {
                Console.WriteLine(str);
            }
        }
    }
}