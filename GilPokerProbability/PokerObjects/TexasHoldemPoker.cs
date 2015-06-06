using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability
{
    public class TexasHoldemPoker
    {
        //public Deck deck;
        //public Hand hand;
        //public Hand cardsOnTable;
        //public Hand dummy;
        public PokerObjects.Table table;

        public TexasHoldemPoker()
        {
            //deck = new Deck();
            //hand = new Hand();
            //dummy = new Hand();
            //cardsOnTable = new Hand();
            

        }

        public void StartDebugGame()
        {
            ////Probabilities prob = new Probabilities();
            ////prob.InitPreFlopDB();
            //deck.Shuffle();
            //hand.AddToHand(deck, 2);
            //hand.Show();
            ////prob.ShowPreFlop(hand);
            ////Console.ReadKey();
            ////show probability

            //cardsOnTable.AddToHand(deck, 3);
            //cardsOnTable.Show();
            ////show prob
            ////prob.GetProbabilities(deck, hand, cardsOnTable);
            ////prob.ShowStrength();
            ////Console.ReadKey();

            ////prob.GetProbabilities(deck,dummy,cardsOnTable);
            ////prob.ShowStrength();
            ////Console.ReadKey();

            //cardsOnTable.AddToHand(deck, 1);
            //Console.WriteLine(cardsOnTable.Cards.Last().ToString());
            ////show pro
            ////prob.GetProbabilities(deck, hand, cardsOnTable);
            ////prob.ShowStrength();
            ////Console.ReadKey();

            //cardsOnTable.AddToHand(deck, 1);
            //Console.WriteLine(cardsOnTable.Cards.Last().ToString());
            ////Evaluator.Evaluate(hand, new Hand(), 0, deck.Cards.Count);
            ////ShowProbability()
            ////prob.GetProbabilities(deck, hand, cardsOnTable);
            ////prob.ShowStrength();
            ////Console.ReadKey();

            //Strength result = new Strength();
            //Strength improvedResult = new Strength();
            //Card[] finalCards = new Card[7];
            //// get 7 card sorted array.
            //finalCards = cardsOnTable.Cards.Concat(hand.Cards).OrderBy(card => card.CardNumber).ToArray();
            //result = Scores.ValueHand7cards(finalCards);
            //result.show();
            //improvedResult = Scores.ImprovedEval7Cards(finalCards);
            //improvedResult.show();

        }
        public void StartHelperGame()
        {

        }
        

        public void StartComputerGame()
        {

        }
        public void StartComputerAgentGame()
        {

        }
        public void Start1v1ArenaGame()
        {
            
            table = new PokerObjects.Table(1, 1,1000, false,true);
            table.TableGame(20, 10);
        }

    }
}
