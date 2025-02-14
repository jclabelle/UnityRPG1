using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Quests;
using UnityEditor;

namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Objectives")]
    public class ObjectivesDB : BaseDB
    {
        public List<Objective> all;

#if UNITY_EDITOR

        void Reset()
        {
            Refresh(all);
        }

#endif
    }
}