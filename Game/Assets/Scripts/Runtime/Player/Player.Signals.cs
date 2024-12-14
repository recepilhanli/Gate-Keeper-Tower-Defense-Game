using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.PlayerOperations
{
    //Player.Signals
    public partial class Player
    {
        public UnityAction onBowlingAttackStart;
        public UnityAction onBowlingAttackEnd;
        public UnityAction onWindAttack;
        public UnityAction onPunch;
    }
}