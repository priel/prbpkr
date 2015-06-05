using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{
    class Table
    {
        public Hand hand;
        public int moneyOnTable;
        public int currBet;
        public int diler;//zero base
        public List<Player> players;


        public Table()
        {
            hand = new Hand();
            players = new List<Player>();
            
        }
        public void SetBlinds(int smallBlind, int bigBlind)
        {
            int whoSmallBlind = (diler + 1) % (players.Count - 1);
            if (players[whoSmallBlind].money > smallBlind)
                players[whoSmallBlind].bet(smallBlind,this);
            else
                players[whoSmallBlind].bet(players[whoSmallBlind].money,this);
            int whoBigBlind = (diler + 2 ) % (players.Count-1);
            if (players[whoBigBlind].money > smallBlind)
                players[whoBigBlind].bet(bigBlind, this);
            else
                players[whoBigBlind].bet(players[whoBigBlind].money, this);
        }
        public 

    }
}
