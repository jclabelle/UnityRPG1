
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//#if UNITY_EDITOR
using UnityEditor;
//#endif


namespace CustomTools
{
    public static class AssetTools
    {
//#if UNITY_EDITOR

        static public List<T> GetAllAssetsOfType<T>(string typename) where T : Object
        {
            List<T> list = new List<T>();

            var guids = AssetDatabase.FindAssets($"t:{typename.Trim()}");

            return GUIDSToAssets<T>(guids);
        }

        static public List<string> GetAssetPathsOfType(string typename) //where T : UnityEngine.Object
        {
            List<string> list = new List<string>();

            var guids = AssetDatabase.FindAssets($"t:{typename.Trim()}");

            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                list.Add(path);
            }

            if (typename == "Scene")
            {
                IEnumerable<string> stripTemplates =
                    from s in list
                    where s.Contains("Packages/com.unity.render-pipelines.universal/Editor/SceneTemplates/") is false
                    select s;

                return stripTemplates.ToList<string>();
            }

            return list;
        }

        static public List<T> GetAllAssetsOfType<T>(ref T type) where T : Object
        {
            List<T> list = new List<T>();

            var guids = AssetDatabase.FindAssets($"t:{type.ToString().Trim()}");

            return GUIDSToAssets<T>(guids);

        }

        static public T GetAssetByName<T>(string name) where T : Object
        {
            var guids = AssetDatabase.FindAssets($"{name.Trim()}");

            var list = GUIDSToAssets<T>(guids);

            if (list.Count > 1)
                Debug.Log("Warning: Tools.GetAssetByName(string name) found more than one object. Returning list[0].");

            return list[0];


        }

        static public List<T> GUIDSToAssets<T>(string[] guids) where T : Object
        {
            List<T> list = new List<T>();

            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                T item = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
                list.Add(item);
            }

            return list;
        }

        static public List<SceneAsset> GUIDSToScenes(string[] guids)
        {
            List<SceneAsset> list = new List<SceneAsset>();

            foreach (string guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);

            }

            return list;
        }

        static public GUIStyle CreateStyle(int fontsize = 48, Color? color = null, bool wordWrap = true,
            TextAnchor anchor = TextAnchor.UpperLeft, Texture2D texture = null)
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = fontsize;
            if (color is Color c)
            {
                style.normal.textColor = c;
            }
            else
            {
                style.normal.textColor = Color.black;
            }

            style.wordWrap = wordWrap;
            style.alignment = anchor;
            style.padding = new RectOffset(5, 5, 5, 5);
            if (texture is Texture2D tex)
                style.normal.background = texture;

            return style;
        }

        static public List<SceneAsset> GetAllSceneNames()
        {
            var guids = AssetDatabase.FindAssets("t:Scene");
            var scenes = GUIDSToAssets<SceneAsset>(guids);
            return scenes;

        }


        //#if UNITY_EDITOR
        //        var scenePath = UnityEditor.EditorBuildSettings.scenes
        //            .Select(s => s.path)
        //            .FirstOrDefault(s => s.EndsWith(sceneName + ".unity"));
        //        m_SceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);
        //        m_SceneName = sceneName;
        //    }
        //#endif
    }
}

