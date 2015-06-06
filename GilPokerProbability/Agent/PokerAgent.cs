using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{
    public class PokerAgent
    {
        public Probabilities myProb;
        public Probabilities opProb;
        public List<Card> unseenCards;
        public Random rnd;
        public int agressiveRaise, agressiveMoney, detector,decieveOponnent; 

        public PokerAgent()
        {
            myProb = new Probabilities();
            opProb = new Probabilities();
            unseenCards = new List<Card>();
            rnd = new Random();
            agressiveRaise = 8; // 0-100 indicate the random for casual raise.
            agressiveMoney = 30; // 0-100 indicate the minimum raise fruction.
            decieveOponnent = 40;
            detector = 0;
        }


        public PlayerDecision GetDecisionArena1v1(Table table, int numberOfRoound,ComPlayer comPlayer)
        {
            PlayerDecision decision = new PlayerDecision();
            int betNeeded = table.currBet - comPlayer.betSoFar;
            double ratioNeededBet = (double)betNeeded/comPlayer.money;
            int raiseRandom = rnd.Next(100);
            int moneyRandom = rnd.Next(agressiveMoney,100);
            int decieveRandom = rnd.Next(decieveOponnent,100);
            unseenCards.AddRange(table.deck.Cards);
            foreach (Player player in table.players)
                if (player != comPlayer)
                    unseenCards.AddRange(player.hand.Cards);

            double myChances = GetChances(numberOfRoound, comPlayer.hand, table.hand, table.players.Count);
            //Console.WriteLine("debug: chances are:" + myChances);
            
            if (raiseRandom < agressiveRaise)
            {
                decision.money = (int)((((double)moneyRandom) / 100) * comPlayer.money);
                decision.outcome = DecisionType.Raise;
                return decision;
            }
            switch (numberOfRoound)
            {
                case 0:
                
                    if (myChances < 0.37)
                    {
                        decision.money = 0;
                        if (betNeeded == 0)
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                        else
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                    }
                    else if (myChances < 0.62)
                    {
                        if (ratioNeededBet > (((double)decieveRandom) / 100))
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                        else if (ratioNeededBet < 0.05)
                        {
                            decision.money = Math.Max(comPlayer.money / 15,betNeeded);
                            decision.outcome = DecisionType.Raise;
                            break;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }

                    }
                    else
                    {
                        if (ratioNeededBet < 0.2)
                        {
                            decision.money = Math.Max(comPlayer.money / 10, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            return decision;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                    }
                case 1:
                    if (myChances < 0.3)
                    {
                        decision.money = 0;
                        if (betNeeded == 0)
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                        else
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                    }
                    else if (myChances < 0.83)
                    {
                        if (ratioNeededBet > (((double)decieveRandom) / 100))
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                        else if (ratioNeededBet < 0.05)
                        {
                            decision.money = Math.Max(comPlayer.money / 15, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            break;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }

                    }
                    else
                    {
                        if (ratioNeededBet < 0.2)
                        {
                            decision.money = Math.Max(comPlayer.money / 10, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            return decision;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                    }
                case 2:
                    if (myChances < 0.40)
                    {
                        decision.money = 0;
                        if (betNeeded == 0)
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                        else
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                    }
                    else if (myChances < 0.75)
                    {
                        if (ratioNeededBet > (((double)decieveRandom) / 100))
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                        else if (ratioNeededBet < 0.05)
                        {
                            decision.money = Math.Max(comPlayer.money / 15, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            break;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }

                    }
                    else
                    {
                        if (ratioNeededBet < 0.2)
                        {
                            decision.money = Math.Max(comPlayer.money / 10, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            return decision;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                    }
                case 3:
                    if (myChances < 0.5)
                    {
                        decision.money = 0;
                        if (betNeeded == 0)
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                        else
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                    }
                    else if (myChances < 0.80)
                    {
                        if (ratioNeededBet > (((double)decieveRandom) / 100))
                        {
                            decision.money = 0;
                            decision.outcome = DecisionType.Drop;
                            break;
                        }
                        else if (ratioNeededBet < 0.05)
                        {
                            decision.money = Math.Max(comPlayer.money / 15, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            break;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }

                    }
                    else
                    {
                        if (ratioNeededBet < 0.2)
                        {
                            decision.money = Math.Max(comPlayer.money / 10, betNeeded);
                            decision.outcome = DecisionType.Raise;
                            return decision;
                        }
                        else
                        {
                            decision.outcome = DecisionType.Call;
                            break;
                        }
                    }
            }
            unseenCards.Clear();
            return decision;
        }






        public double GetChances(int numberOfRound,Hand myHand,Hand tableHand,int participants)
        {
            if (numberOfRound == 0)//preflop
            {
                myProb.InitPreFlopDB();
                return myProb.GetPreFlopProb(myHand, participants);
            }
            myProb.GetProbabilities(unseenCards, myHand, tableHand);
            List<Card> allNotMyCards = new List<Card>();
            allNotMyCards.AddRange(unseenCards);
            allNotMyCards.AddRange(myHand.Cards);
            Hand emptyHand = new Hand();
            opProb.GetProbabilities(allNotMyCards, emptyHand, tableHand);
            double probToWin = myProb.GetAfterFlopProb(opProb);
            myProb.possibleRankValues.Clear();
            opProb.possibleRankValues.Clear();
            return probToWin;
        }
    }
}
