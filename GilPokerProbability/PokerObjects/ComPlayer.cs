using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{



    public class ComPlayer : Player
    {
        public PokerAgent agent;

        public ComPlayer(int comMoney, string comName, Table _table)
            : base(comMoney, comName, _table)
        {
            agent = new PokerAgent();
        }

        public override PlayerDecision GetDecition(int numberRound)
        {
            PlayerDecision decision = new PlayerDecision();
            decision = agent.GetDecisionArena1v1(table, numberRound,this);
            switch (decision.outcome)
            {
                case DecisionType.Call:
                    if (table.currBet - betSoFar < money)
                    {
                        bet(table.currBet - betSoFar);
                        decision.money = table.currBet - betSoFar;
                    }
                    else
                    {
                        bet(money);
                        decision.money = money;
                    }
                    Console.WriteLine(name + " has call and have " + money + "$");
                    break;
                    
                case DecisionType.Drop:
                    isPlaying = false;
                    Console.WriteLine(name + "has drop and have " + money + "$");
                    break;

                case DecisionType.Raise:
                    if ((decision.money > money) || (decision.money < table.currBet - betSoFar))
                        Console.WriteLine("error, agent return illegal value on raise" + decision.money);
                    bet(decision.money);
                    Console.WriteLine(name + "has raise "+decision.money + "$ and have " + money + "$");
                    break;
            }
            return decision;
        }
    }
}
