using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GilPokerProbability.PokerObjects
{
    public enum DecisionType
    {
        Call,
        Raise,
        Drop
    }
    public class PlayerDecision
    {
        public DecisionType outcome;
        public int money;
    }
}
