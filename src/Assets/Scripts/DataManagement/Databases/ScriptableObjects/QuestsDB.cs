using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using Quests;


namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Quests")]
    public class QuestsDB : BaseDB
    {
        public List<Quest> all;

#if UNITY_EDITOR

        void Reset()
        {
            Refresh(all);
        }

#endif
    }
}
