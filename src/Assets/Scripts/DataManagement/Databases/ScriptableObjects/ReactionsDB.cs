using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Reactions")]
    public class ReactionsDB : BaseDB
    {
        public List<ReactionV2> all;

#if UNITY_EDITOR

        void Reset()
        {
            Refresh(all);
        }

#endif
    }
}