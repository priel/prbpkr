using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{



    class ComPlayer : Player
    {

        public ComPlayer(int comMoney, string comName, Table _table)
            : base(comMoney, comName, _table)
        {
            
        }
        PlayerDecision decision = new PlayerDecision();
        public override PlayerDecision GetDecition(int numberRound)
        {
            decision.outcome = DecisionType.Call;
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
            return decision;
        }
    }
}
