using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Data.Gameplay
{
    [CreateAssetMenu(menuName = "Databases/Items")]
    public class ItemsDB : BaseDB
    {
        public List<Item> all;

#if UNITY_EDITOR

    void Reset()
    {
        Refresh(all);
    }

#endif
    }
}
#if UNITY_EDITOR
#endif