using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public enum StrengthRank : byte
    {
        HighCard = 0,
        Pair ,
        TwoPairs,
        Triplet,
        Straight,
        Flush,
        FullHouse,
        Quarter,
        StraightFlush
    }
    public class HandStrength
    {
        public StrengthRank strength { get; set; }
        public override string ToString()
        {
            var strStrength = "";
            switch (strength)
            {
                case StrengthRank.HighCard:
                    strStrength = "high card";
                    break;
                case StrengthRank.Pair:
                    strStrength = "pair";
                    break;
                case StrengthRank.TwoPairs:
                    strStrength = "two pairs";
                    break;
                case StrengthRank.Triplet:
                    strStrength = "triplet";
                    break;
                case StrengthRank.Straight:
                    strStrength = "straight";
                    break;
                case StrengthRank.Flush:
                    strStrength = "flush";
                    break;
                case StrengthRank.FullHouse:
                    strStrength = "full house";
                    break;
                case StrengthRank.Quarter:
                    strStrength = "quater";
                    break;
                case StrengthRank.StraightFlush:
                    strStrength = "straight fulsh";
                    break;
            }
            return strStrength;
        }
    }
}
