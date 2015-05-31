using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public class Hand
    {
        public List<Card> Cards { get; set; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public Hand(Hand hand)
            : this()
        {
            Cards.AddRange(hand.Cards);
        }

        public bool ContainsCardNumber(CardNumberEnum cardNumber)
        {
            return Cards.Find(card => card.CardNumber == cardNumber) != null;
        }

        public Card FindHighestCard()
        {
            Card highest = null;
            foreach (var card in Cards.Where(card => highest == null || highest.CardNumber < card.CardNumber))
                highest = card;
            return highest;
        }


        public void ReturnAllCards(Deck deck)
        {
            deck.Cards.AddRange(Cards);
            Cards.RemoveAll(Cards.Contains);
        }

        public void AddToHand(Deck deck, byte numberOfCards)
        {
            Cards.AddRange(deck.TakeCards(numberOfCards));
        }

        public void Sort()
        {
            Cards = Cards.OrderBy(card => card.CardNumber).ThenBy(card => card.CardSuit).ToList();
        }

        public override int GetHashCode()
        {
            return Cards.Aggregate(0, (current, cardNumber) => current * 39 + cardNumber.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            return GetHashCode() == obj.GetHashCode();
        }

        public override string ToString()
        {
            return string.Join(",", Cards.ConvertAll(card => card.ToString()).ToArray());
        }
        public void Show()
        {
            foreach (var card in Cards)
                Console.Write("{0}, ", card);
            Console.WriteLine();
        }
    }
}
