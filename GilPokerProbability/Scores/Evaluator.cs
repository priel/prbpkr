using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public static class Evaluator
    {
        public class HandEvaluation
        {
            public Hand Hand { get; set; }
            public int Evaluation { get; set; }
        }

        public class CardsNumbersCombination
        {
            public List<CardNumberEnum> CardNumbers { get; private set; }

            public CardsNumbersCombination(params CardNumberEnum[] cardNumbers)
            {
                CardNumbers = new List<CardNumberEnum>();
                CardNumbers.AddRange(cardNumbers);
            }

            public bool Conatins(CardNumberEnum cardNumber)
            {
                return CardNumbers.Contains(cardNumber);
            }

            public override int GetHashCode()
            {
                return CardNumbers.Aggregate(0, (current, cardNumber) => current * 20 + cardNumber.GetHashCode());
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                    return false;
                return GetHashCode() == obj.GetHashCode();
            }
        }

        //public static void Evaluate(Hand hand, Hand river, byte players, int cardsInDeck)
        //{
        //    var cardsToChooseFrom = cardsInDeck + players * 2;

        //    var o = new List<HandEvaluation>();

        //    //var ratio = EvaluateChanceFor_HighCard(hand, players);

        //    //what is the chance for straight-flush
        //    //what is the chance for 4 of a kind
        //    //what is the chance for full house
        //    //what is the chance for a flush
        //    //what is the chance for straight
        //    //what is the chance for 3 of a kind
        //    //what is the chance for two pairs
        //    //what is the chance for pair
        //    //what is the chance for a high card



        //    EvaluateOddsForWinWith_StraightFlush(hand, river, cardsToChooseFrom);

        //    //var pairChance = 0;

        //    //Card temp = null;
        //    //foreach (var card in hand.Cards)
        //    //{
        //    //    if (temp == null)
        //    //        temp = card;
        //    //    else if (card.CardNumber == temp.CardNumber)
        //    //        pairChance = 1;
        //    //}

        //    //Console.WriteLine(pairChance);

        //    //hand.Cards.SelectMany(c.CardNumber==d.CardNumber);


        //}

        //public static byte EvaluateOddsForWinWith_StraightFlush(Hand hand, Hand river, byte cardsToChooseFrom)
        //{

        //    var tempHand = new Hand(hand);

        //    #region Pre-Processing

        //    tempHand.Sort();

        //    Card card1 = tempHand.Cards[0];
        //    Card card2 = tempHand.Cards[1];


        //    if (card1.CardSuit == card2.CardSuit)
        //    {
        //        if (card2.CardNumber == CardNumberEnum.Ace)
        //        {
        //            if(card1.CardNumber <= CardNumberEnum.Five || card1.CardNumber >= 10)
        //                var val = Formula(cardsToChooseFrom, )


        //        }



        //    }
        //    else
        //    {

        //    }








        //    #endregion






        //    // (((a-c)choose(d-b))*(c choose b))/(a choose d) where a=50 , b=3 , c=3 ,d=5
        //    //a- the number of cards in the package.
        //    //b- the amount of cards that needed to be on the showing cards, so we reached the result.
        //    //c - the number of cards that are in the package and can be fit (b<=c always)
        //    //d- the number of cards that needed to be showing (b<=d always)


        //    ////chance to win           94%
        //    ////chance to share win      6%
        //    ////chance to loose          0%

        //    //what is the chance for straight-flush?
        //    //what is the chance that the stright-flush will be the highest one in the table?
        //    //what is the chance that no one else has the same stright-flush.
        //    //EvaluateChanceFor_StraightFlush(hand, players, out double oddsToWin)
        //    //oddsToWin = oddsToEvolve * oddsToWinIfEvolved // 1/2 * 1/2 = 1/4
        //    //     5,6
        //    //   2,3,4,5,6     1 out of 20      1 out of 8
        //    //   3,4,5,6,7     1 out of 20      1 out of 7
        //    //   4,5,6,7,8     1 out of 20      1 out of 6
        //    //   5,6,7,8,9     1 out of 20      1 out of 5

        //    //get all straight possibilites from this hand.

        //    //get all straight-flush possibilities from this hand.

        //    //per each possibility
        //    //  calculate per card odds
        //    //  multiply the the odds

        //    //hmm.. not sure.. think that not.. find the worst possiblity (ie: 1 to 42312)

        //    hand = new Hand
        //    {
        //        Cards = new List<Card>
        //        {
        //            new Card {CardNumber = CardNumberEnum.Ace, CardSuit = CardSuitEnum.Diamond},
        //            new Card {CardNumber = CardNumberEnum.Five, CardSuit = CardSuitEnum.Spade}
        //        }
        //    };

        //    hand.Sort();

        //    var straightCompletions = new List<Hand>();
        //    foreach (var card in hand.Cards)
        //        straightCompletions.AddRange(GetSraightFlushCombinations(card));

        //    straightCompletions = straightCompletions.Distinct().ToList();

        //    //var chanceForAnyCard = Deck.TotalCards - testHand.Cards.Count;

        //    var targetHand = straightCompletions[0];
        //    Console.WriteLine(targetHand);

        //    double all = 1;
        //    var riverSpots = 5;
        //    double t = cardsInDeck;
        //    foreach (var card in targetHand.Cards)
        //    {
        //        double cardProbability;
        //        if (hand.Cards.Contains(card))
        //            cardProbability = 1;
        //        else
        //        {
        //            cardProbability = riverSpots/t;
        //            riverSpots--;
        //            t--;
        //        }
        //        all *= cardProbability;
        //    }



        //    var p = GetProbability(3, 5, cardsInDeck);


        //    return 0;

        //}

        //public static double GetProbability(int matches, int available, int cardsInDeck)
        //{
        //    double combinedProbability = 0;
        //    for (var index = 0; index < Math.Pow(2, available); index++)
        //    {
        //        var combination = new bool[available];
        //        var divisor = index;
        //        var counter = 0;
        //        for (var j = 0; j < available; j++)
        //        {
        //            var value = divisor % 2 == 1;
        //            divisor /= 2;
        //            if (value)
        //                counter++;
        //            combination[available - 1 - j] = value;
        //        }
        //        if (counter != matches)
        //            continue;
        //        var m = matches;
        //        double t = cardsInDeck;
        //        double combinationProbability = 1;
        //        foreach (var parameter in combination)
        //        {
        //            if (parameter)
        //            {
        //                combinationProbability *= m / t;
        //                m--;
        //            }
        //            else
        //                combinationProbability *= (t - matches) / t;
        //            t--;
        //        }
        //        combinedProbability += combinationProbability;
        //    }
        //    return combinedProbability;
        //}

        //private static List<Hand> GetSraightFlushCombinations(Card card)
        //{
        //    var straightFlushCompletions = new List<Hand>();
        //    var numberCompletions = getStraightCombinations(card.CardNumber);
        //    foreach (var numberCompletion in numberCompletions)
        //    {
        //        var straightFlushCompletion = new Hand();
        //        foreach (var cardNumber in numberCompletion.CardNumbers)
        //            straightFlushCompletion.Cards.Add(new Card { CardNumber = cardNumber, CardSuit = card.CardSuit });
        //        straightFlushCompletions.Add(straightFlushCompletion);
        //    }
        //    return straightFlushCompletions;
        //}


        //public static double Formula(int a, int b, int c, int d)
        //{
        //    return 0;
        //}

        //private static List<CardsNumbersCombination> getStraightCombinations(CardNumberEnum cardNumber)
        //{
        //    var striaghtCombinations = new List<CardsNumbersCombination>
        //    {
        //        new CardsNumbersCombination(CardNumberEnum.Ace, CardNumberEnum.Two, CardNumberEnum.Three, CardNumberEnum.Four, CardNumberEnum.Five),
        //        new CardsNumbersCombination(CardNumberEnum.Two, CardNumberEnum.Three, CardNumberEnum.Four, CardNumberEnum.Five, CardNumberEnum.Six),
        //        new CardsNumbersCombination(CardNumberEnum.Three, CardNumberEnum.Four, CardNumberEnum.Five, CardNumberEnum.Six, CardNumberEnum.Seven),
        //        new CardsNumbersCombination(CardNumberEnum.Four, CardNumberEnum.Five, CardNumberEnum.Six, CardNumberEnum.Seven, CardNumberEnum.Eight),
        //        new CardsNumbersCombination(CardNumberEnum.Five, CardNumberEnum.Six, CardNumberEnum.Seven, CardNumberEnum.Eight, CardNumberEnum.Nine),
        //        new CardsNumbersCombination(CardNumberEnum.Six, CardNumberEnum.Seven, CardNumberEnum.Eight, CardNumberEnum.Nine, CardNumberEnum.Ten),
        //        new CardsNumbersCombination(CardNumberEnum.Seven, CardNumberEnum.Eight, CardNumberEnum.Nine, CardNumberEnum.Ten, CardNumberEnum.Jack),
        //        new CardsNumbersCombination(CardNumberEnum.Eight, CardNumberEnum.Nine, CardNumberEnum.Ten, CardNumberEnum.Jack, CardNumberEnum.Queen),
        //        new CardsNumbersCombination(CardNumberEnum.Nine, CardNumberEnum.Ten, CardNumberEnum.Jack, CardNumberEnum.Queen, CardNumberEnum.King),
        //        new CardsNumbersCombination(CardNumberEnum.Ten, CardNumberEnum.Jack, CardNumberEnum.Queen, CardNumberEnum.King, CardNumberEnum.Ace)
        //    };

        //    // replace this crap with the formula. but remember to mult the formula result by possible occurances.
        //    // in example, A has result of formula x2
        //    // another example, for five you need to mult the result by 5 (same for 6 and 7 etc..)
        //    // last example, J need to x4, queen x3.. etc..


        //    var straightCompletions = striaghtCombinations.Where(combination => combination.Conatins(cardNumber)).ToList();
        //    return straightCompletions;
        //}


        //////chance to win           94%
        //////chance to share win      6%
        //////chance to loose          0%

        ////private static double EvaluateChanceFor_HighCard(Hand hand, byte players)
        ////{
        ////    //high card means no one will recieve a pair,two pairs


        ////    //1. what is the chance for hand having the highest card on the table.
        ////    //

        ////    var highestCard = hand.FindHighestCard();

        ////    if (hand.ContainsCardNumber(CardNumber.Ace))
        ////        return 1;
        ////    else
        ////    {


        ////    }

        ////}

    }


}
