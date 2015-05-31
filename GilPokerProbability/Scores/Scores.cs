using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{

    //maybe needed to score out of 7 immidiatly --peformance sensitive...
    public static class Scores
    {
        public static Strength ImprovedEval7Cards(Card[] cards)
        {
            // for high cards we alwais will get the latest 5 cards so we good :)
            //frequency of 7 hand card poker:
                    //Royal flush	4,324	0.0032%	
                    //Straight flush (excl. royal flush)	37,260	0.0279%	
                    //Four of a kind	224,848	0.168%	
                    //Full house	3,473,184	2.60%	
                    //Flush	4,047,644	3.03%	
                    //Straight	6,180,020	4.62%	
                    //Three of a kind	6,461,620	4.83%
                    //Two pair	31,433,400	23.5%	
                    //One pair	58,627,800	43.8%	
                    //No pair	23,294,460	17.4%	
                    //Total	133,784,560	100%
            // so we need for: pair, high card and 2 pairs to work the fastest! other much less important
            Strength evaluate = new Strength();
            evaluate.value = 0;
            int[] same = new int[7];
            int[] sequential = new int[7];
            int[] colour = new int[4];
            bool[] isUsed = new bool[7];
            bool isFlush = false;
            int isAndWhereStraight = -1;
            int count = 0;
            int improvedPair=0;
            uint tmpValue = 0;
            //binary form:
            //pair 1
            //triple 2
            //quad 4
            //2nd pair 8
            //flush 16
            //straight 32

            colour[(int)cards[6].CardSuit]++;
            for (int i = 5; i >= 0; i--)
            {
                colour[(int)cards[i].CardSuit]++;
                //cases from most common to the list.
                if (cards[i].CardNumber < cards[i + 1].CardNumber - 1) //for performence, it will continue on most cases.
                {
                    continue;
                }
                else if (cards[i].CardNumber == cards[i + 1].CardNumber)
                {
                    same[i] = same[i + 1] + 1;
                    same[i+1] = 0;
                    count++;
                    improvedPair = i;
                }
                else if (cards[i].CardNumber == cards[i + 1].CardNumber - 1)
                    sequential[i]=sequential[i+1]+1;
                else if (same[i+1]>0)
                    sequential[i]=sequential[i+1];
            }

            // here if sequntial is 5 take the higher sequence and this is straight
            // same can include 0's with:
            //nothing (no pair)
            // 1 (pair),
            //1 1 (two pair)
            //1 1 1 (two pair count only the higher)
            // 12,21 112,121,211 (full house, alwais take the 2, and the highest pair).
            //if colour has 5 in it then it's flush

            //for performance check pair, 2 pairs, high card and then all the rest.
            for (int i = 2; i >= 0; i--)
            {
                if (sequential[i] == 5)
                    isAndWhereStraight = i;
            }
            for (int i = 0; i < 4; i++)
            {
                if (colour[i] >= 5)
                    isFlush = true;
            }
            // check first for pair, 2pairs and high card
            if (isAndWhereStraight == -1 && !isFlush)
            {
                if (count == 1)
                {
                    //pair
                    for (int i = 6,j=0; j < 3;i-- )
                    {
                        if (i == improvedPair || i == (improvedPair + 1))
                            continue;
                        // if we mult every time in 20 the highest number will have the highest impact.
                        evaluate.value += (uint)cards[i].CardNumber;
                        evaluate.value *= 16;
                        j++;
                    }
                    evaluate.value += 3300000000;
                    evaluate.value += ((uint)cards[improvedPair].CardNumber * 1000000);
                    evaluate.rank.strength = StrengthRank.Pair;
                    return evaluate;
                }
                else if (count == 0) //the check for high card is realy simple so i made it first.
                {
                    for (int j = 6; j >= 2; j--)
                    {
                        // if we mult every time in 20 the highest number will have the highest impact.
                        evaluate.value += (uint)cards[j].CardNumber;
                        evaluate.value *= 16; // maybe 16 is just shl ;)
                    }
                    evaluate.rank.strength = StrengthRank.HighCard;
                    return evaluate;
                }
                //we need to check 2 pairs (and nothing more)


            }

            return evaluate;
        }






        public static Strength ValueHand7cards(Card[] cards)
        {
            Strength tmpvalues = new Strength();
            Strength values = new Strength();
            values.value=0;
            Card[] cardsCombination = new Card[5];
            //we should create all combination like 00xxxxx 0x0xxxx ... x00xxxx...
            //to do so we will use double loops when i,j will be the zeros
            for (int i = 0; i < 6; i++)
            {
                for (int j = i+1; j < 7; j++)
                {
                    //now we have i,j in the indexes as zeros
                    //create the array
                    for (int k=0, l = 0; l < 7; l++)
                    {
                        if (l != i && l != j)
                        {
                            cardsCombination[k] = cards[l];
                            k++;
                        }
                    }
                    tmpvalues = ValueHand5cards(cardsCombination);
                    if (tmpvalues.value > values.value)
                        values = tmpvalues;
                }
            }
            return values;
        }
        public static Strength ValueHand5cards(Card[] cards)
        {

        //HighCard,       --the high cards
        //Pair,           --the pair than the high cards
        //TwoPairs,       --the highes pair then the other pair then the high card
        //Triplet,        --the triplet high card, then the 2nd high card then the 3rd
        //Straight,       --only the high card rule
        //Flush,          --only the high card rule
        //FullHouse,      --first the triplet then the pair
        //Quarter,        --only the high card of the 4 of a kind rule
        //StraightFlush   --only the high card rule


            // this function is only to evaluate 5 cards.
            // this is performance sensitive, assuming the cards are in non-decreasing values.
            Strength evaluate = new Strength();
            evaluate.value = 0;
            //straight-flush
            if (IsFlush(cards) && IsStraight(cards))
            {
                evaluate.value = 4000000000;
                evaluate.value += ((uint)cards[0].CardNumber);
                evaluate.rank.strength = StrengthRank.StraightFlush;
                return evaluate;
            }
            //quartet
            if (IsQuartet(cards))
            {
                evaluate.value = 3900000000;
                evaluate.value += ((uint)cards[2].CardNumber);
                evaluate.rank.strength = StrengthRank.Quarter;
                return evaluate;
            }

            //full house 2 ways
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3800000000;
                evaluate.value += ((uint)cards[0].CardNumber * 1000);
                evaluate.value += ((uint)cards[3].CardNumber);
                evaluate.rank.strength = StrengthRank.FullHouse;
                return evaluate;
            }
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3800000000;
                evaluate.value += ((uint)cards[3].CardNumber * 1000);
                evaluate.value += ((uint)cards[0].CardNumber);
                evaluate.rank.strength = StrengthRank.FullHouse;
                return evaluate;
            }

            //flush
            if (IsFlush(cards))
            {
                for (int j = 4; j >= 0; j--)
                {
                    // if we mult every time in 20 the highest number will have the highest impact.
                    evaluate.value += (uint)cards[j].CardNumber;
                    evaluate.value *= 16; // maybe 16 is just shl ;)
                }
                evaluate.value += 3700000000;
                evaluate.rank.strength = StrengthRank.Flush;
                return evaluate;
            }

            //straight
            if (IsStraight(cards))
            {
                evaluate.value = 3600000000;
                evaluate.value += (uint)cards[0].CardNumber;
                evaluate.rank.strength = StrengthRank.Straight;
                return evaluate;
            }

            //triplet
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber))
            {
                evaluate.value = 3500000000;
                evaluate.value += (uint)cards[3].CardNumber;
                evaluate.value += ((uint)cards[4].CardNumber * 100);
                evaluate.value += ((uint)cards[0].CardNumber * 10000);
                evaluate.rank.strength = StrengthRank.Triplet;
                return evaluate;
            }
            if ((cards[1].CardNumber == cards[2].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber))
            {
                evaluate.value = 3500000000;
                evaluate.value += (uint)cards[0].CardNumber;
                evaluate.value += ((uint)cards[4].CardNumber * 100);
                evaluate.value += ((uint)cards[1].CardNumber * 10000);
                evaluate.rank.strength = StrengthRank.Triplet;
                return evaluate;
            }
            if ((cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3500000000;
                evaluate.value += (uint)cards[0].CardNumber;
                evaluate.value += ((uint)cards[1].CardNumber * 100);
                evaluate.value += ((uint)cards[2].CardNumber * 10000);
                evaluate.rank.strength = StrengthRank.Triplet;
                return evaluate;
            }

            //two pairs
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber))
            {
                evaluate.value = 3400000000;
                evaluate.value += ((uint)cards[2].CardNumber * 10000);
                evaluate.value += ((uint)cards[0].CardNumber *100); 
                evaluate.value += (uint)cards[4].CardNumber ;
                evaluate.rank.strength = StrengthRank.TwoPairs;
                return evaluate;
            }
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3400000000;
                evaluate.value += ((uint)cards[3].CardNumber * 10000);
                evaluate.value += ((uint)cards[0].CardNumber * 100); 
                evaluate.value += (uint)cards[2].CardNumber;
                evaluate.rank.strength = StrengthRank.TwoPairs;
                return evaluate;
            }
            if ((cards[1].CardNumber == cards[2].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3400000000;
                evaluate.value += ((uint)cards[3].CardNumber * 10000);
                evaluate.value += ((uint)cards[1].CardNumber * 100);
                evaluate.value += (uint)cards[0].CardNumber;
                evaluate.rank.strength = StrengthRank.TwoPairs;
                return evaluate;
            }

            //a pair
            for (int i = 0; i < 4; i++)
            {
                if (cards[i].CardNumber == cards[i + 1].CardNumber)
                {
                    evaluate.value = 0;
                    for (int j = 4; j >= 0; j--)
                    {
                        if (j == i || j == (i + 1))
                            continue;
                        // if we mult every time in 20 the highest number will have the highest impact.
                        evaluate.value += (uint)cards[j].CardNumber;
                        evaluate.value *= 16;

                    }
                    evaluate.value += 3300000000;
                    evaluate.value += ((uint)cards[i].CardNumber * 1000000);
                    evaluate.rank.strength = StrengthRank.Pair;
                    return evaluate;
                }
            }

            //high card
            for (int j = 4; j >= 0; j--)
            {
                // if we mult every time in 20 the highest number will have the highest impact.
                evaluate.value += (uint)cards[j].CardNumber;
                evaluate.value *= 16; // maybe 16 is just shl ;)
            }
            evaluate.rank.strength = StrengthRank.HighCard;
            return evaluate;
        }

        //some of the functions could use other functions, but since it's perfomance sensitive, it's better that way..

        // all functions below take sorted array of 5 cards.
        public static bool IsFlush(Card[] cards)
        {
            return ((cards[0].CardSuit == cards[1].CardSuit) && (cards[1].CardSuit == cards[2].CardSuit) && (cards[2].CardSuit == cards[3].CardSuit) && (cards[3].CardSuit == cards[4].CardSuit));
        }
        public static bool IsStraight(Card[] cards)
        {
            return (((cards[0].CardNumber + 1) == cards[1].CardNumber) && ((cards[1].CardNumber + 1) == cards[2].CardNumber) &&
                ((cards[2].CardNumber + 1) == cards[3].CardNumber) && (((cards[3].CardNumber + 1) == cards[4].CardNumber) || ((cards[3].CardNumber + 9) == cards[4].CardNumber)));
        }
        public static bool IsQuartet(Card[] cards)
        {
            return (((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber)) ||
                ((cards[1].CardNumber == cards[2].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber)));
        }
        public static bool IsFullHouse(Card[] cards)
        {
            return (((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber)) ||
                ((cards[0].CardNumber == cards[1].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber)));
        }
        public static bool IsTriplet(Card[] cards)
        {
            return (((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber)) ||
                ((cards[1].CardNumber == cards[2].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber)) ||
                ((cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber)));
        }
        public static bool IsTwoPairs(Card[] cards)
        {
            //since it's sorted we have only 3 ways.
            return (((cards[0].CardNumber == cards[1].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber)) ||
                ((cards[0].CardNumber == cards[1].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber)) ||
                ((cards[1].CardNumber == cards[2].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber)));
           


            //    for (int i = 0; i < 5; i++)
            //      {
            //    // pay attention we want to send sorted Array!
           
            //    Card[] tempCards = new Card[4];
            //    for (int j = 0; j < 4; )
            //        if (j != i)
            //        {
            //            tempCards[j] = cards[i];
            //            j++;
            //        }
            //    if (tempCards[0]==tempCards[1] && tempCards[2] == tempCards[3])
            //        return true;
            //}
            //return false;
        }
        public static bool IsPair(Card[] cards)
        {
            for (int i = 0; i < 4; i++)
                if (cards[i].CardNumber == cards[i + 1].CardNumber)
                    return true;
            return false;
        }




    }
    
}
