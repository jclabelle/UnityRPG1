using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Abilities")]
    public class AbilitiesDB : BaseDB
    {
        public List<AbilityV2> all;

#if UNITY_EDITOR

    void Reset()
    {
        Refresh(all);
        for(int i = all.Count -1; i>=0; i--)
        {
            if(all[i] is ReactionV2)
            {
                all.RemoveAt(i);
            }
        }
    }

#endif
    }
}