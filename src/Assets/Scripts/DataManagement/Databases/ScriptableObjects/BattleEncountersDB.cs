using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BattleV3;


namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Encounters")]
    public class BattleEncountersDB : BaseDB
    {
        public List<BattleEncounter> all;

#if UNITY_EDITOR

        void Reset()

        {
            Refresh(all);

        }
#endif
    }
}
