using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{

    public class Player
    {
        public string name;
        public int money;
        public Table table;
        public Hand hand;
        public bool isPlaying;
        public int betSoFar;
        

        public Player(int _money,string _name,Table _table)
        {
            name = _name;
            money = _money;
            table = _table;
            hand = new Hand();
            isPlaying = true;
            betSoFar = 0;
        }

        public Strength DeterminsticValueHand() //call this function only after you got 7 cards
        {
            Card[] finalCards = new Card[7];
            finalCards = table.hand.Cards.Concat(hand.Cards).OrderBy(card => card.CardNumber).ToArray();
            return Scores.ValueHand7cards(finalCards);
        }

        public void getCards(byte numberCards)
        {
            hand.AddToHand(table.deck, numberCards); 
        }
        public void returnCards()
        {
            hand.ReturnAllCards(table.deck);
        }
        public void bet (int bet)
        {
            money -= bet;
            table.moneyOnTable += bet;
            betSoFar += bet;
            if (betSoFar > table.currBet)
                table.currBet = betSoFar;
        }

        public void ShowHand()
        {
            Console.WriteLine(name + " got:");
            hand.Show();
        }

        public void ShowMoney()
        {
            Console.WriteLine(name + " have " + money + "$");
        }

        public virtual PlayerDecision GetDecition(int numberRound)
        {
            string d;
            PlayerDecision decision = new PlayerDecision();
            Console.WriteLine(name + ", you have: " + money + "$ and there is " + table.moneyOnTable + "$ on table");
            Console.WriteLine("current bet is: " + (table.currBet - betSoFar));
            Console.WriteLine("what would you like to do (c/r/d)?");
            //Console.WriteLine("Call / Raise / Drop ? (press: c/r/d)");
            d = Console.ReadLine();  
            while (((d != "r") && (d != "c") && (d != "d")) || ((d=="r") && (table.currBet-betSoFar>money)))
            {
                Console.WriteLine("please choose only c/r/d and don't raise if you dont have the money!");
                d = Console.ReadLine();
            }
            switch (d)
            {
                case "c":
                    decision.outcome = DecisionType.Call;
                    if (table.currBet - betSoFar<money)
                    {
                        bet(table.currBet - betSoFar);
                        decision.money = table.currBet - betSoFar;
                    }
                    else
                    {
                        bet(money);
                        decision.money=money;
                    }
                    break;
                case "r":
                    Console.WriteLine("how much do you want to raise?");
                    decision.money = ModifiedConsole.GetInteger();
                    while ((decision.money > money) || (decision.money < table.currBet - betSoFar))
                    {
                        Console.WriteLine("invalid number!");
                        decision.money = ModifiedConsole.GetInteger();
                    }
                    bet(decision.money);
                    break;
                case "d":
                    decision.outcome = DecisionType.Drop;
                    decision.money =0;
                    isPlaying = false;
                    break;
            }

            return decision;
        }
    }
}
