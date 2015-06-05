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
            //improve the performance of the frequent case!!!!!!

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
            int whichFlush = -1;
            uint highestNumber = 100;
            int isAndWhereStraight = -1;
            bool isStraightWithAce = false;
            int count = 0;
            int improvedPair = 0;
            int i;
            uint j, k = 0;


            colour[(int)cards[6].CardSuit]++;
            for (i = 5; i >= 0; i--)
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
                    same[i + 1] = 0;
                    count++;
                    improvedPair = i;
                    sequential[i] = sequential[i + 1];
                }
                else if (cards[i].CardNumber == cards[i + 1].CardNumber - 1)
                    sequential[i] = sequential[i + 1] + 1;
                //else if (same[i + 1] > 0)
                //    sequential[i] = sequential[i + 1];
            }

            // here if sequntial is 5 take the higher sequence and this is straight
            // same can include 0's with:
            //nothing (no pair)
            // 1 (pair),
            //if colour has 5 in it then it's flush

            //for performance check pair, 2 pairs, high card and then all the rest.
            for (i = 2; i >= 0; i--)
            {
                if (sequential[i] == 4)
                    isAndWhereStraight = i;
            }
            if ((sequential[0] == 3) && (cards[0].CardNumber == CardNumberEnum.Two) && (cards[6].CardNumber == CardNumberEnum.Ace))
            {
                isAndWhereStraight = 0;
                isStraightWithAce = true;
            }
            for (i = 0; i < 4; i++)
            {
                if (colour[i] >= 5)
                    whichFlush = i;
            }
            // check first for pair, 2pairs and high card
            if (isAndWhereStraight == -1 && whichFlush == -1)
            {
                if (count == 1)
                {
                    //pair
                    for (i = 6, j = 0; j < 3; i--)
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
                    for (j = 6; j >= 2; j--)
                    {
                        // if we mult every time in 20 the highest number will have the highest impact.
                        evaluate.value += (uint)cards[j].CardNumber;
                        evaluate.value *= 16; // maybe 16 is just shl ;)
                    }
                    evaluate.rank.strength = StrengthRank.HighCard;
                    return evaluate;
                }


                //for 2 pairs:
                evaluate.value = 3400000000;
                evaluate.rank.strength = StrengthRank.TwoPairs;
                for (i = 6, j = 4096, k = 0; i > 0; i--)
                {
                    if (same[i - 1] == 1)
                    {
                        k++;
                        evaluate.value += ((uint)cards[i].CardNumber * j);
                        j = j / 16;
                        i--;
                    }
                    else if (highestNumber == 100)
                    {
                        highestNumber = (uint)cards[i].CardNumber;
                        evaluate.value += highestNumber;
                    }
                }

                if (count == 3 && k == 3)// need a fix to sub the latest pair
                {
                    evaluate.value -= ((uint)cards[1].CardNumber * 16);// if there is 3 pairs the list pair will alwais sit in cards[1].
                    // find the highest number, sub it too than take 
                    if (highestNumber == 100 || highestNumber < (uint)cards[1].CardNumber)
                    {
                        evaluate.value += (uint)cards[1].CardNumber;
                    }
                    return evaluate;
                }

                if (k == 2 && count == 2)
                    return evaluate;

                //we can reach here only if we are not 2 pairs,pair or high card! nor flush nor straight
                if (count == 2)
                {
                    //we are 3 of a kind
                    evaluate.value = 3500000000;
                    evaluate.rank.strength = StrengthRank.Triplet;
                    for (i = 0; i < 6; i++)
                    {
                        //find where the triple
                        if (same[i] == 2)
                        {
                            evaluate.value += ((uint)cards[i].CardNumber * 256);
                        }
                    }
                    for (i = 6, j = 0, k = 16; j < 2; i--)
                    {
                        if (same[i] != 2 && same[i - 1] != 2 && same[i - 2] != 2)
                        {
                            evaluate.value += ((uint)cards[i].CardNumber * k);
                            k = 1;
                            j++;
                        }
                    }
                    return evaluate;
                }
            }
            {
                int[] flushSequential = new int[7];
                List<int> possibleFlushCards = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
                int flushSuit = -1;
                int whereQurterBegun = -1;
                //we left to check for (in this order): straight, flush, fullhouse, 4 of a kind, straight flush

                //straight flush
                if (whichFlush >= 0 && isAndWhereStraight >= 0)
                {
                    for (i = 0; i < 4; i++)
                    {
                        if (colour[i] >= 5)
                            flushSuit = i;
                    }
                    //CardNumberEnum numberCard = cards[6].CardNumber;
                    //CardSuitEnum suitCard = cards[6].CardSuit;

                    //for (i = 6, j = 0; i > 0; i--)
                    //{
                    //    if (numberCard == cards[i - 1].CardNumber + 1)
                    //    {
                    //        if (flushSuit == (int)cards[i - 1].CardSuit)
                    //        {
                    //            if (j == 3)
                    //                listValueCard = i - 1;
                    //            j++;
                    //            numberCard = cards[i - 1].CardNumber;
                    //            suitCard = cards[i - 1].CardSuit;
                    //        }
                    //    }
                    //    else if (cards[i].CardNumber != cards[i - 1].CardNumber)
                    //    {
                    //        j = 0;
                    //        numberCard = cards[i - 1].CardNumber;
                    //        suitCard = cards[i - 1].CardSuit;
                    //    }
                    //}
                    for (i = 0; i < 7; i++)
                    {
                        if ((int)cards[i].CardSuit == flushSuit)
                            possibleFlushCards[i] = (int)cards[i].CardNumber;
                    }
                    //possibleFlushCards.Remove(possibleFlushCards.RemoveAll(s => s == 0));
                    for (i = 0, j = (uint)possibleFlushCards.Count; i < j; i++)
                    {
                        if (possibleFlushCards[i] == 0)
                        {
                            possibleFlushCards.RemoveAt(i);
                            i--;
                            j--;

                        }
                    }
                    // we left with list
                    for (i = possibleFlushCards.Count - 2; i >= 0;i--)
                    {
                        //cases from most common to the list.
                        if (possibleFlushCards[i] == possibleFlushCards[i + 1] - 1)
                            flushSequential[i] = flushSequential[i + 1] + 1;
                        //else if (same[i + 1] > 0)
                        //    sequential[i] = sequential[i + 1];
                    }
                    for (i = 2; i >= 0; i--)
                    {
                        if (flushSequential[i] == 4)
                        {
                            evaluate.value = 4000000000;
                            evaluate.value += ((uint)possibleFlushCards[i]);
                            evaluate.rank.strength = StrengthRank.StraightFlush;
                            return evaluate;
                        }
                    }

                    if ((flushSequential[0] == 3) && (possibleFlushCards[0] == (int)CardNumberEnum.Two) && (possibleFlushCards[possibleFlushCards.Count - 1] == (int)CardNumberEnum.Ace))
                    {
                        //straight flush with A 2 3...
                        evaluate.value = 4000000001;
                        evaluate.rank.strength = StrengthRank.StraightFlush;
                        return evaluate;
                    }

                    //    bool isUpdate = false;
                    //    for (i = 6, j = 0; i > 0; i--) // could done much more nice code, if i just count the number of sequential and flush, and keep the same if pair, reset if not and not
                    //    {
                    //        isUpdate = false;
                    //        if (((int)cards[i].CardNumber == (int)cards[i - 1].CardNumber + 1) && (((int)cards[i].CardSuit == flushSuit) && ((int)cards[i - 1].CardSuit == flushSuit)))
                    //        {
                    //            if (j == 3)
                    //                listValueCard = i - 1;

                    //            j++;
                    //            isUpdate = true;
                    //            continue;
                    //        }
                    //        if (i - 2 >= 0)
                    //        {
                    //            if (((int)cards[i].CardNumber == (int)cards[i - 2].CardNumber + 1) && (((int)cards[i].CardSuit == flushSuit) && ((int)cards[i - 2].CardSuit == flushSuit)))
                    //            {
                    //                    if (j == 3)
                    //                        listValueCard = i - 2;
                    //                    j++;
                    //                    i--;
                    //                    isUpdate = true;
                    //                continue;
                    //            }
                    //        }
                    //        if (i - 3 >= 0)
                    //        {
                    //            if (((int)cards[i].CardNumber == (int)cards[i - 3].CardNumber + 1) && (((int)cards[i].CardSuit == flushSuit) && ((int)cards[i - 3].CardSuit == flushSuit)))
                    //            {
                    //                    if (j == 3)
                    //                        listValueCard = i - 3;
                    //                    i -= 2;
                    //                    j++;
                    //            }
                    //            continue;
                    //        }
                    //        j=0;
                    //    }

                    //    if (j >= 4)
                    //    {
                    //        evaluate.value = 4000000000;
                    //        evaluate.value += ((uint)cards[listValueCard].CardNumber);
                    //        evaluate.rank.strength = StrengthRank.StraightFlush;
                    //        return evaluate;
                    //    }
                    //    else if (j == 3)
                    //    {

                    //        // we can reach here if the Straight starting from A and we have flush.
                    //        // we surely have 4 cards (3 connection) of straight flush, 
                    //        // check if it start from 2
                    //        if (cards[0].CardNumber == CardNumberEnum.Two && cards[6].CardNumber == CardNumberEnum.Ace && sequential[0] >= 3)
                    //            // now we need to check that we have the correct ace
                    //            if (((int)cards[6].CardSuit == flushSuit ||
                    //                (cards[5].CardNumber == CardNumberEnum.Ace && (int)cards[5].CardSuit == flushSuit) ||
                    //                (cards[4].CardNumber == CardNumberEnum.Ace && (int)cards[4].CardSuit == flushSuit))
                    //                && (cards[listValueCard].CardNumber == CardNumberEnum.Two))//check that we have Ace from the same colour.                             
                    //            {
                    //                //here we know that we started from 2 and we have 3 cards from it
                    //                evaluate.value = 4000000000;
                    //                evaluate.value += ((uint)cards[listValueCard].CardNumber) - 1;
                    //                evaluate.rank.strength = StrengthRank.StraightFlush;
                    //                return evaluate;
                    //            }
                    //    }

                }

                //4 of a kind
                if (count >= 3)
                {
                    for (i = 0; i < 6; i++)
                        if (same[i] == 3)
                            whereQurterBegun = i;
                    if (whereQurterBegun != -1)
                    {
                        //we're in quarter
                        evaluate.rank.strength = StrengthRank.Quarter;
                        evaluate.value = 3900000000;
                        if (whereQurterBegun == 3)
                        {
                            evaluate.value += ((uint)cards[3].CardNumber * 16);
                            evaluate.value += ((uint)cards[2].CardNumber);
                        }
                        else
                        {
                            evaluate.value += ((uint)cards[6].CardNumber);
                            evaluate.value += ((uint)cards[whereQurterBegun].CardNumber * 16);
                        }
                        return evaluate;
                    }
                }
                //now check for full house:
                // the option left for us are: 22 21 12 
                if (count == 3)
                {
                    //it's for sure full house here.
                    evaluate.value = 3800000000;
                    evaluate.rank.strength = StrengthRank.FullHouse;

                    for (i = 0; i < 6; i++)
                    {
                        //find where the triple
                        if (same[i] == 2)
                        {
                            evaluate.value += ((uint)cards[i].CardNumber * 16);
                        }
                        else if (same[i] == 1)
                        {
                            evaluate.value += ((uint)cards[i].CardNumber);
                        }
                    }
                    return evaluate;
                }
                if (count == 4)
                {
                    //here we should handle the case of 3 2 2 and 3 3 1


                    //this is for the case of 3 3 1 which we have 2 triplet
                    //it's for sure full house here.
                    evaluate.value = 3800000000;
                    evaluate.rank.strength = StrengthRank.FullHouse;

                    for (i = 6, j = 16; i >= 0; i--)
                    {
                        //find where the triple
                        if (same[i] == 2)
                        {
                            evaluate.value += ((uint)cards[i].CardNumber * j);
                            j = j / 16;
                        }
                    }
                    if (j == 1)
                    {
                        //we move only 1 time in the for so we have 3 2 2
                        //so we need to add the higher pair
                        if (same[4] == 2)
                            evaluate.value += ((uint)cards[3].CardNumber);
                        else
                            evaluate.value += ((uint)cards[5].CardNumber);
                    }

                    return evaluate;
                }
                // we have only flush and straight now :)
                if (whichFlush >= 0)
                {
                    evaluate.value = 0;
                    for (i = 6, j = 0; j < 5; i--)
                    {
                        if ((int)cards[i].CardSuit == whichFlush)
                        {
                            evaluate.value += (uint)cards[i].CardNumber;
                            evaluate.value *= 16; // maybe 16 is just shl ;)
                            j++;
                        }
                    }
                    evaluate.value += 3700000000;
                    evaluate.rank.strength = StrengthRank.Flush;
                    return evaluate;
                }
                if (isAndWhereStraight >= 0)
                {

                    evaluate.value = 3600000000;
                    evaluate.value += (uint)cards[isAndWhereStraight].CardNumber;
                    if (isStraightWithAce)
                        evaluate.value--;
                    evaluate.rank.strength = StrengthRank.Straight;
                    return evaluate;
                }
            }
            Console.WriteLine("very big error!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Console.WriteLine("the error in score");
            return evaluate;
        }







        public static Strength ValueHand7cards(Card[] cards)
        {
            Strength tmpvalues = new Strength();
            Strength values = new Strength();
            values.value = 0;
            Card[] cardsCombination = new Card[5];
            //we should create all combination like 00xxxxx 0x0xxxx ... x00xxxx...
            //to do so we will use double loops when i,j will be the zeros
            for (int i = 0; i < 6; i++)
            {
                for (int j = i + 1; j < 7; j++)
                {
                    //now we have i,j in the indexes as zeros
                    //create the array
                    for (int k = 0, l = 0; l < 7; l++)
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
                if ((cards[0].CardNumber == CardNumberEnum.Two) && (cards[4].CardNumber == CardNumberEnum.Ace))
                    evaluate.value--;
                evaluate.rank.strength = StrengthRank.StraightFlush;
                return evaluate;
            }
            //quartet
            if (IsQuartet(cards))
            {
                evaluate.value = 3900000000;
                evaluate.value += ((uint)cards[2].CardNumber * 16);
                if (cards[0].CardNumber == cards[1].CardNumber)
                    evaluate.value += (uint)cards[4].CardNumber;
                else
                    evaluate.value += (uint)cards[0].CardNumber;
                evaluate.rank.strength = StrengthRank.Quarter;
                return evaluate;
            }

            //full house 2 ways
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3800000000;
                evaluate.value += ((uint)cards[0].CardNumber * 16);
                evaluate.value += ((uint)cards[3].CardNumber);
                evaluate.rank.strength = StrengthRank.FullHouse;
                return evaluate;
            }
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3800000000;
                evaluate.value += ((uint)cards[3].CardNumber * 16);
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
                if (cards[0].CardNumber == CardNumberEnum.Two && cards[4].CardNumber == CardNumberEnum.Ace)
                    evaluate.value--;
                evaluate.rank.strength = StrengthRank.Straight;
                return evaluate;
            }

            //triplet
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[1].CardNumber == cards[2].CardNumber))
            {
                evaluate.value = 3500000000;
                evaluate.value += (uint)cards[3].CardNumber;
                evaluate.value += ((uint)cards[4].CardNumber * 16);
                evaluate.value += ((uint)cards[0].CardNumber * 256);
                evaluate.rank.strength = StrengthRank.Triplet;
                return evaluate;
            }
            if ((cards[1].CardNumber == cards[2].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber))
            {
                evaluate.value = 3500000000;
                evaluate.value += (uint)cards[0].CardNumber;
                evaluate.value += ((uint)cards[4].CardNumber * 16);
                evaluate.value += ((uint)cards[1].CardNumber * 256);
                evaluate.rank.strength = StrengthRank.Triplet;
                return evaluate;
            }
            if ((cards[2].CardNumber == cards[3].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3500000000;
                evaluate.value += (uint)cards[0].CardNumber;
                evaluate.value += ((uint)cards[1].CardNumber * 16);
                evaluate.value += ((uint)cards[2].CardNumber * 256);
                evaluate.rank.strength = StrengthRank.Triplet;
                return evaluate;
            }

            //two pairs
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[2].CardNumber == cards[3].CardNumber))
            {
                evaluate.value = 3400000000;
                evaluate.value += ((uint)cards[2].CardNumber * 4096);
                evaluate.value += ((uint)cards[0].CardNumber * 256);
                evaluate.value += (uint)cards[4].CardNumber;
                evaluate.rank.strength = StrengthRank.TwoPairs;
                return evaluate;
            }
            if ((cards[0].CardNumber == cards[1].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3400000000;
                evaluate.value += ((uint)cards[3].CardNumber * 4096);
                evaluate.value += ((uint)cards[0].CardNumber * 256);
                evaluate.value += (uint)cards[2].CardNumber;
                evaluate.rank.strength = StrengthRank.TwoPairs;
                return evaluate;
            }
            if ((cards[1].CardNumber == cards[2].CardNumber) && (cards[3].CardNumber == cards[4].CardNumber))
            {
                evaluate.value = 3400000000;
                evaluate.value += ((uint)cards[3].CardNumber * 4096);
                evaluate.value += ((uint)cards[1].CardNumber * 256);
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
