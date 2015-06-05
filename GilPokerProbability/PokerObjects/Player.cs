using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{

    class Player
    {
        public string name;
        public int money;
        public Hand hand;
        public bool isPlaying;
        public int betSoFar;

        public bool bet (int bet, Table table)
        {
            if (bet > money || (bet + betSoFar)<table.currBet)
                return false;
            money -= bet;
            table.moneyOnTable += bet;
            if (bet + betSoFar > table.currBet)
                table.currBet = bet + betSoFar;
            return true;
        }
    }
}
