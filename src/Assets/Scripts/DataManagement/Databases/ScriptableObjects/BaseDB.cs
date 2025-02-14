using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace Data.Gameplay
{
    public abstract class BaseDB : ScriptableObject
    {

#if UNITY_EDITOR

    protected void Refresh<T>(List<T> so) where T : UnityEngine.Object
    {
        var type = so.GetType().GetTypeInfo().GenericTypeArguments[0];
        var assetGuids = AssetDatabase.FindAssets($"t:{type.ToString().Trim()}");

        foreach (var guid in assetGuids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath(path, type);
            so.Add((T)asset);
        }
    }

#endif
    }
}
#if UNITY_EDITOR
#endif