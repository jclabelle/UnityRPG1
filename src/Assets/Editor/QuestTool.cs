using System.Collections;
using System.Collections.Generic;
using System.Linq;  
using UnityEngine;
using UnityEngine.SceneManagement;
using Quests;
//#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
//#endif

//#if UNITY_EDITOR
namespace CustomTools
{
    public class QuestTool : EditorWindow
    {
        public List<Quest> allQuests;
        public List<string> allStarts;
        public List<string> allCheckpoints;
        public List<string> allEnds;
        public List<Scene> allScenes;

        [MenuItem("Tools/Quest Tool")]
        public static void ShowWindow()
        {
            GetWindow<QuestTool>("Quest Tool");
        }

        private void Awake()
        {
            RefreshQuestList();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh Quests"))
            {
                RefreshQuestList();
            }


            //EditorGUILayout.BeginHorizontal();
            //Object myGameObject = new Object();
            //myGameObject = EditorGUILayout.ObjectField(myGameObject, typeof(Quest), true) as Quest;


            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Quests");

            string details = "";

            foreach (Quest q in allQuests)
            {
                details += $"{q.name.Trim()}\tPrerequisites: ";

                if (q.prerequisites is null || (q.prerequisites.Length == 0))
                {
                    details += "NONE";
                }
                else
                {
                    foreach (var prereq in q.prerequisites)
                    {
                        details += $"{prereq.name.Trim()}, ";
                    }
                }

                details += "\tObjectives: ";

                if (q.objectives is null || (q.objectives.Length == 0))
                {
                    details += "NONE";

                }
                else
                {
                    foreach (var objective in q.objectives)
                    {
                        details += $"{objective.name.Trim()}, ";
                    }
                }

                details += "\n";
            }

            var content = new GUIContent(details);

            GUIStyle listStyle = AssetTools.CreateStyle(12, Color.white);
            var size = listStyle.CalcSize(content);

            EditorGUILayout.LabelField(details, listStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
            EditorGUILayout.EndVertical();



            EditorGUILayout.BeginVertical();
            int _selected = 0;
            string[] quest_names = new string[allQuests.Count];
            for (int i = 0; i < allQuests.Count; i++)
            {
                quest_names[i] = allQuests[i].name;
            }

            EditorGUILayout.Popup("Select Quest:", _selected, quest_names);
            //EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();


        }

        void RefreshQuestList()
        {
            allQuests = AssetTools.GetAllAssetsOfType<Quest>("Quest");
            allScenes = new List<Scene>();
            allStarts = new List<string>();
            allCheckpoints = new List<string>();
            allEnds = new List<string>();

            var scenes = AssetTools.GetAllSceneNames();

            var paths = AssetTools.GetAssetPathsOfType("Scene");


            // todo: Make this a pull down selectable menu where can select a specific scene to see it's Quest Start/Checkpoints/Ends, because every time we load a new scene we lose references to the previous scene's objects.
            // todo: Make verifications of Quest being unique and other checks scene by scene, we can't pull a list of all Quest Starts/Ends/Checkpoints because every time we load a new scene we lose references to the previous scene's objects.
            // Unless we make copies, save the name and scene, edit the copy then overwrite the original by reloading the scene...
            foreach (var p in paths)
            {
     

                EditorSceneManager.OpenScene(p);
                Debug.Log(EditorSceneManager.GetActiveScene().name);

                var scene = EditorSceneManager.GetActiveScene();
                if (scene.GetRootGameObjects() is GameObject[] hierarchy)
                {
                    foreach (var obj in hierarchy)
                    {
         

                        if (obj.GetComponent<QuestStart>() is QuestStart start)
                            allStarts.Add($"Quest Start: {start.name} found in {scene.name}");

                        if (obj.GetComponent<QuestCheckpoint>() is QuestCheckpoint checkpoint)
                            allCheckpoints.Add($"Quest Checkpoint: {checkpoint.name} found in {scene.name}");

                        if (obj.GetComponent<QuestEnd>() is QuestEnd end)
                            allEnds.Add($"Quest End: {end.name} found in {scene.name}");
                    }
                }

                Debug.Log(allStarts);
                Debug.Log(allCheckpoints);
                Debug.Log(allEnds);


            }

    
        }


    }
//#endif
}