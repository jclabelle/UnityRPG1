using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animancer.FSM;
using UnityEngine;

namespace BattleV3
{
    public abstract class BattlerBaseState : IState
    {

        public abstract bool CanEnterState { get; }
        public abstract bool CanExitState { get; }
        public abstract void OnEnterState();

        public abstract void OnExitState();
    }
}