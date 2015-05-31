using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public class Strength
    {
        public HandStrength rank { get; set; }
        public uint value { get; set; }

        public Strength()
        {
            rank = new HandStrength();
            value = new uint();
        }
        public void show()
        {
            Console.WriteLine("you have " + rank.ToString());
            Console.WriteLine("your score is: " + value);
        }
    }
}
