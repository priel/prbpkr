using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public class Deck
    {
        public List<Card> Cards { get; set; }

        public Deck()
        {
            Reset();
        }

        public void Reset()
        {
            Cards = Enumerable.Range(0, Enum.GetNames(typeof(CardSuitEnum)).Length)
                .SelectMany(s => Enumerable.Range((byte)CardNumberEnum.Two, Enum.GetNames(typeof(CardNumberEnum)).Length)
                    .Select(c => new Card
                    {
                        CardSuit = (CardSuitEnum)s,
                        CardNumber = (CardNumberEnum)c
                    }))
                .ToList();
        }

        public void Shuffle()
        {
            Cards = Cards.OrderBy(c => Guid.NewGuid())
                .ToList();
        }

        public Card TakeCard()
        {
            var card = Cards.FirstOrDefault();
            Cards.Remove(card);

            return card;
        }

        public IEnumerable<Card> TakeCards(byte numberOfCards)
        {
            var cards = Cards.Take(numberOfCards);
            var takeCards = cards as Card[] ?? cards.ToArray();
            Cards.RemoveAll(takeCards.Contains);
            return takeCards;
        }
        public void Sort()
        {
            Cards = Cards.OrderBy(card => card.CardNumber).ThenBy(card => card.CardSuit).ToList();
        }
    }
}
