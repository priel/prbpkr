using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public enum CardSuitEnum : byte
    {
        Club = 0,
        Diamond = 1,
        Heart = 2,
        Spade = 3
    }

    public enum CardNumberEnum : byte
    {
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
        Ace = 14
    }
    public class Card
    {
        public CardSuitEnum CardSuit { get; set; }
        public CardNumberEnum CardNumber { get; set; }

        //public override int GetHashCode()
        //{
        //    return (CardSuit.GetHashCode() - 1)*13 + CardNumber.GetHashCode();
        //}

        //public override bool Equals(object obj)
        //{
        //    if (obj == null || GetType() != obj.GetType())
        //        return false;
        //    return GetHashCode() == obj.GetHashCode();
        //}

        public override int GetHashCode()
        {
            return ((int)CardSuit - 1) * 13 + (int)CardNumber;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return GetHashCode() == obj.GetHashCode();
        }


        public override string ToString()
        {
            var suitFigure = '?';
            switch (CardSuit)
            {
                case CardSuitEnum.Club:
                    suitFigure = '♣';
                    break;
                case CardSuitEnum.Diamond:
                    suitFigure = '♦';
                    break;
                case CardSuitEnum.Heart:
                    suitFigure = '♥';
                    break;
                case CardSuitEnum.Spade:
                    suitFigure = '♠';
                    break;
            }
            var numberFigure = "";
            switch (CardNumber)
            {
                case CardNumberEnum.Two:
                    numberFigure = "2";
                    break;
                case CardNumberEnum.Three:
                    numberFigure = "3";
                    break;
                case CardNumberEnum.Four:
                    numberFigure = "4";
                    break;
                case CardNumberEnum.Five:
                    numberFigure = "5";
                    break;
                case CardNumberEnum.Six:
                    numberFigure = "6";
                    break;
                case CardNumberEnum.Seven:
                    numberFigure = "7";
                    break;
                case CardNumberEnum.Eight:
                    numberFigure = "8";
                    break;
                case CardNumberEnum.Nine:
                    numberFigure = "9";
                    break;
                case CardNumberEnum.Ten:
                    numberFigure = "10";
                    break;
                case CardNumberEnum.Jack:
                    numberFigure = "J";
                    break;
                case CardNumberEnum.Queen:
                    numberFigure = "Q";
                    break;
                case CardNumberEnum.King:
                    numberFigure = "K";
                    break;
                case CardNumberEnum.Ace:
                    numberFigure = "A";
                    break;
            }
            return string.Format("{0}{1}", numberFigure, suitFigure);
        }
    }
}
