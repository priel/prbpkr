using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{

    // //i need to build a formula so i can get from 2 cards to an id.

    // //26n-n^2 is the formula needed to found the sector i need.
    // //where n is starting from 0, and therefore n=14-card.number;
    // //secnod low card will determine the number, it's alwais begin with the higher card number.
    // //so the second card is added with the formula 2*distance(-1if they are the same colour).
    // // for example for 7,4 26n-n^2=26*7-7^2=133 +2*d(-1 if they are the same)=133+2*3-1=138
    // // thats for the same if they are not equal needed to add 1
    public class Probabilities
    {
        double[] probabilityForStrength = new double[9];
        public List<uint>[] possibleRankValues = new List<uint>[2];
        double[][] preFlopDb = new double[169][];


        public Probabilities()
        {
            possibleRankValues[0] = new List<uint>();
            possibleRankValues[1] = new List<uint>();
        }

        public void ShowPreFlop(Hand hand)
        {
            int highCard = Math.Max((int)hand.Cards[0].CardNumber,(int)hand.Cards[1].CardNumber);
            int lowCard  = Math.Min((int)hand.Cards[0].CardNumber,(int)hand.Cards[1].CardNumber);
            int isSameColour = hand.Cards[0].CardSuit==hand.Cards[1].CardSuit?1:0;
            int n = 14 - highCard;
            int entry = 26 * n - n * n + 2 * (highCard - lowCard) - isSameColour;
            Console.WriteLine("your chances to win againts multiple players (not including you) are:");
            for (int i = 0; i < 9; i++)
            {
                Console.Write((i + 1) + ": " + preFlopDb[entry][i]);
                Console.WriteLine();
            }
        }

        public void InitPreFlopDB()
        {
            System.Xml.Serialization.XmlSerializer reader =
        new System.Xml.Serialization.XmlSerializer(typeof(double[][]));
            System.IO.StreamReader file = new System.IO.StreamReader(
                @"C:\Users\priel\Documents\Visual Studio 2010\Projects\GilPokerProbability\GilPokerProbability\Scores\pokerPreFlopProb.xml");
            preFlopDb = (double[][])reader.Deserialize(file);
        }

        //public setProbabilities(Card[] tableCards, Card[] myHandCards)
        //{
        //    int totalPossibilitiesChecked=0;
        //    int[] indexes = new int[numOpenCards];
        //    for (int i = 0; i < numOpenCards; i++)
        //        indexes[i] = i;

        //     while (true)
        //    {
        //        //check results
        //        totalPossibilitiesChecked++;
        //        //for debug only, it should return the number of possibilities.
        //        //which is (#deck choose #opencards)
 
        //        for (int i = 0; i < numOpenCards; i++)
        //            toCreate[i] = deck.Cards[indexes[i]];

        //        Array.Copy(allShown, cardsToCheck, numShownCards);
        //        Array.Copy(toCreate, 0, cardsToCheck, allShown.Length, toCreate.Length);
        //        Array.Sort(cardsToCheck, delegate(Card card1, Card card2)
        //        {
        //            return card1.CardNumber.CompareTo(card2.CardNumber);
        //        });

        //        //Console.WriteLine("My array: {0}",string.Join(", ", cardsToCheck.Select(v => v.ToString())));
        //        //cardsToCheck = cardsToCheck.OrderBy(card => card.CardNumber).ToArray();
        //        //array.OrderBy(item => item.Fields["FieldName"].Value);
        //        currStrength = Scores.ValueHand7cards(cardsToCheck);
        //        countStrengths[(int)currStrength.rank.strength]++;


        //        //check limits
        //        if (numOpenCards==0 || indexes[0]==cardsInDeck-numOpenCards)
        //            break;

        //        //update array
        //        j=numOpenCards-1;
        //        while (true)
        //        {
        //            if (indexes[j] != cardsInDeck - numOpenCards + j)
        //            {
        //                indexes[j]++;
        //                for (j++; j < numOpenCards; j++)
        //                {
        //                    indexes[j] = indexes[j - 1] + 1;
        //                }
        //                break;
        //            }
        //            j--;
        //        }
        //    }
        //}



        public Probabilities GetProbabilities(Deck deck, Hand playerHand, Hand tableHand)
        {
            int j;
            int cardsInDeck = deck.Cards.Count;
            int texasCardsOpen = 7;
            int totalPossibilitiesChecked =0;
            int numShownCards = playerHand.Cards.Count + tableHand.Cards.Count;
            int numOpenCards = texasCardsOpen - numShownCards;
            int[] indexes = new int[numOpenCards];
            int[] countStrengths = new int[9];
            Card[] allShown = new Card[numShownCards];
            Card[] toCreate = new Card[numOpenCards];
            Card[] cardsToCheck = new Card[texasCardsOpen];
            Strength currStrength;
            allShown = playerHand.Cards.Concat(tableHand.Cards).ToArray();
            //OrderBy(card => card.CardNumber);
            //init  012..
            
            //for (int i = 0; i < numOpenCards; i++)
            //    indexes[i] = i;
            //deck.Sort();
            
            while (true)
            {
                //check results
                totalPossibilitiesChecked++;
                //for debug only, it should return the number of possibilities.
                //which is (#deck choose #opencards)
 
                for (int i = 0; i < numOpenCards; i++)
                    toCreate[i] = deck.Cards[indexes[i]];

                Array.Copy(allShown, cardsToCheck, numShownCards);
                Array.Copy(toCreate, 0, cardsToCheck, allShown.Length, toCreate.Length);
                Array.Sort(cardsToCheck, delegate(Card card1, Card card2)
                {
                    return card1.CardNumber.CompareTo(card2.CardNumber);
                });

                //Console.WriteLine("My array: {0}",string.Join(", ", cardsToCheck.Select(v => v.ToString())));
                //cardsToCheck = cardsToCheck.OrderBy(card => card.CardNumber).ToArray();
                //array.OrderBy(item => item.Fields["FieldName"].Value);
                currStrength = Scores.ValueHand7cards(cardsToCheck);
                countStrengths[(int)currStrength.rank.strength]++;


                //check limits
                if (numOpenCards==0 || indexes[0]==cardsInDeck-numOpenCards)
                    break;

                //update array
                j=numOpenCards-1;
                while (true)
                {
                    if (indexes[j] != cardsInDeck - numOpenCards + j)
                    {
                        indexes[j]++;
                        for (j++; j < numOpenCards; j++)
                        {
                            indexes[j] = indexes[j - 1] + 1;
                        }
                        break;
                    }
                    j--;
                }
            }
            Console.WriteLine(totalPossibilitiesChecked);

            for (int i = 0; i < 9; i++)
                probabilityForStrength[i] = (double)countStrengths[i] / totalPossibilitiesChecked;
            
            return this;
            
        }


        public void ShowStrength()
        {
            Console.WriteLine("the probabilities are:");
            HandStrength strength = new HandStrength();
            strength.strength=StrengthRank.HighCard;
            for (int i = 0; i < 9; i++)
            {
                Console.WriteLine(strength.strength.ToString() + ": " + probabilityForStrength[i]);
                strength.strength++;
            }
        }
    }
}
// TODO continue from here.
//for (int i =0;i          
//toCreate
//Console.WriteLine("My array: {0}",string.Join(", ", indexes.Select(v => v.ToString())));

//public void UpdateIndexes(int[] indexes)
//{
//    int j=indexes.Length-1;
//    if (indexes[j]!=
//}

//for (int i =0;i<deck.Cards.Count;i++)
//    Console.WriteLine(deck.Cards[i].ToString());
//Console.WriteLine(deck.Cards.Count);
//Array.Clear(probabilityForStrength, 0, probabilityForStrength.Length);