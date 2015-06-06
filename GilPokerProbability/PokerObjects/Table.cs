using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{
    public class Table
    {
        public Hand hand;
        public int moneyOnTable;
        public int initMoneyForPlayers;
        public int currBet;
        public int currPlayers;
        public int diler;//zero base
        public List<Player> players;
        public Deck deck;
        public bool showProb;
        public bool isArena1v1;

        public Table(int numberOfHumanPlayers, int numberOfComputers, int _initMoneyForPlayers, bool _showProb, bool _isArena1v1)
        {
            initMoneyForPlayers = _initMoneyForPlayers;
            diler = 0;
            showProb = _showProb;
            hand = new Hand();
            players = new List<Player>();
            isArena1v1 = _isArena1v1;
            for (int i = 0; i < numberOfHumanPlayers; i++)
            {
                Console.WriteLine("please type your name (case sensitive):");
                string playerName = Console.ReadLine();
                Player player = new Player(initMoneyForPlayers, playerName, this);
                players.Add(player);
            }
            for (int i = 0; i < numberOfComputers; i++)
            {
                ComPlayer complayer = new ComPlayer(initMoneyForPlayers, "com" + i, this);
                players.Add(complayer);
            }
            deck = new Deck();
            deck.Shuffle();
        }


        public void TableGame(int blindFreqRaise, int initBlind)
        {
            Console.Clear();
            int i = 0;
            int smallBlind = initBlind;
            while (players.Count != 1)
            {
                if (i == blindFreqRaise)
                {
                    i = 0;
                    smallBlind *= 2;
                }
                //refresh players
                foreach (Player player in players)
                    player.isPlaying = true;
                currPlayers = players.Count;
                Console.Clear();
                currBet = 0;
                foreach (Player player in players)
                    player.betSoFar = 0;
                deck.Shuffle();

                LocalGame(smallBlind, smallBlind * 2);

                foreach (Player player in players)
                    player.returnCards();

                hand.ReturnAllCards(deck);

                for (int j = 0; j < players.Count; j++) //remove all players with no money
                {
                    if (players[j].money == 0)
                    {
                        players.RemoveAt(j);
                        j--;
                    }
                }
                diler = (diler + 1) % players.Count;

            }
            Console.WriteLine(" the winner is" + players[0].name + " with " + players[0].money + "$");
            return;

        }


        public void LocalGame(int smallBlind, int bigBlind)
        {
           
            foreach (Player player in players)
            {
                player.getCards(2);
                if (player.GetType() != typeof(ComPlayer))
                    player.ShowHand();
            }
            //pre-flop round
            SetBlinds(smallBlind, bigBlind);
            StartRound((diler + 3) % players.Count,0);
            //flop round
            hand.AddToHand(deck, 3);
            hand.Show();
            StartRound((diler + 1) % players.Count,1);
            //river round
            hand.AddToHand(deck, 1);
            hand.Show();
            StartRound((diler + 1) % players.Count,2);
            //turn round
            hand.AddToHand(deck, 1);
            hand.Show();
            StartRound((diler + 1) % players.Count,3);
            if (!isArena1v1 || currPlayers != 1)
                foreach (Player player in players)
                    if (player.isPlaying)
                    {
                        Console.WriteLine(player.name + "have");
                        player.hand.Show();
                    }
            AwardTheWinners();

            Console.ReadKey();
        }

        public void AwardTheWinners()
        {
            List<int> winners = new List<int>();
            uint maxHandValue = 0;
            foreach (Player player in players)
            {
                if (!player.isPlaying)
                    continue;
                if (player.DeterminsticValueHand().value>maxHandValue)
                    maxHandValue = player.DeterminsticValueHand().value;
            }
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].isPlaying)
                    continue;
                if (maxHandValue == players[i].DeterminsticValueHand().value)
                    winners.Add(i);
            }
            for (int i = 0; i < winners.Count; i++)
            {
                Console.WriteLine(players[winners[i]].name + " has won " + moneyOnTable / winners.Count + "$ with " + players[i].DeterminsticValueHand().rank.ToString() + " which value is " + players[i].DeterminsticValueHand().value);
                players[winners[i]].money += moneyOnTable / winners.Count;
            }
            moneyOnTable = 0;
        }

        public void SetBlinds(int smallBlind, int bigBlind)
        {
            int whoSmallBlind = (diler + 1) % (players.Count);
            if (players[whoSmallBlind].money > smallBlind)
            {
                players[whoSmallBlind].bet(smallBlind);
                Console.WriteLine(players[whoSmallBlind].name + " pay small blind of " + smallBlind + "$");
            }
            else
            {
                players[whoSmallBlind].bet(players[whoSmallBlind].money);
                Console.WriteLine(players[whoSmallBlind].name + " pay small blind of " + players[whoSmallBlind].money + "$");
            }
            int whoBigBlind = (diler + 2) % (players.Count);
            if (players[whoBigBlind].money > bigBlind)
            {
                players[whoBigBlind].bet(bigBlind);
                Console.WriteLine(players[whoBigBlind].name + " pay big blind of " + bigBlind + "$");
            }
            else
            {
                players[whoBigBlind].bet(players[whoBigBlind].money);
                Console.WriteLine(players[whoBigBlind].name + " pay big blind of " + players[whoBigBlind].money + "$");
            }
        }

        public void StartRound(int startWith, int numberRound)
        {
            PlayerDecision decision;
            for (int i = startWith, j = 0; (players[i % (players.Count)].betSoFar < currBet) || (j < players.Count); i = (i + 1) % (players.Count), j++)
            {
                if (currPlayers == 1)
                    return;
                if (!players[i].isPlaying)
                    continue;
                decision = players[i].GetDecition(numberRound);
                if (decision.outcome == DecisionType.Drop)
                    currPlayers--;
            }
            currBet = 0;
            foreach (Player player in players)
                player.betSoFar = 0;
            return;
        }

    }
}
