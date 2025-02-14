using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameActions
{
   public abstract class GameAction : ScriptableObject, IGameAction
   {
      public abstract void DoAction();
   }

   public interface IGameAction
   {
      public void DoAction();
   }
}