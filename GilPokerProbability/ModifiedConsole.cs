using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public static class ModifiedConsole
    {
        public static int GetInteger()
        {
            int q = -1;
            while (q == -1)
            {
                q = MyConvert(Console.ReadLine());
                if (q == -1)
                    Console.WriteLine("you didnt enter an integer, please try again:");
            }
            return q;
        }

        public static int MyConvert(string s)
        {
            if (s == null || s == "")
                return -1;
            for (int i = 0; i < s.Length; i++)
                if (s[i] < 48 || s[i] > 57)//ascii of integer?
                {
                    return -1;
                }
            return Convert.ToInt32(s);
        }
    }
}
