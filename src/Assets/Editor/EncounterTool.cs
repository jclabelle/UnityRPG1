using System.Collections;
using System.Collections.Generic;
using BattleV3;
using UnityEngine;
//#if UNITY_EDITOR
using UnityEditor;
//#endif

//#if UNITY_EDITOR
public class EncounterTool : EditorWindow
{
    public List<BattleEncounter> allEncounters;
    
    [MenuItem("Tools/Encounter Tool")]
    public static void ShowWindow()
    {
        GetWindow<EncounterTool>("Encounter Tool");
    }

    private void Awake()
    {
        RefreshEncounterList();
    }

    private void OnGUI()
    {
        RefreshEncounterList();

        Debug.Log("ENCOUNTERTOOL ACTIVATED");

        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("Encounters");

        string details = "";

        foreach (BattleEncounter battleEncounter in allEncounters)
        {
            details += $"{battleEncounter.name.Trim()}\tEnemies: ";

            if (battleEncounter.Enemies is null || (battleEncounter.Enemies.Count == 0))
                details += "NONE\t";

            var rewards = battleEncounter.GetRewards();


            foreach (var reward in rewards)
            {
                details += $"Gold: {reward.Gold}, XP: {reward.XP} \tItems: ";
                
                if (reward.Loot is null || (reward.Loot.Count == 0))
                {
                    details += "NONE\t";

                }
                else
                {
                    foreach (var item in reward.Loot)
                    {
                        details += $"{item}, ";
                    }
                }
                details += "\tObjectives: ";

                if (reward is IReward.SpecialReward special)
                {
                    if (special.QuestObjectives is null || (special.QuestObjectives.Count == 0))
                        details += "NONE\t";
                    else
                        foreach(var objective in special.QuestObjectives)
                            details += $"{objective.name.Trim()}, ";
                    
                    details += "\n";
                    
                    if (special.Abilities is null || (special.Abilities.Count == 0))
                        details += "NONE\t";
                    else
                        foreach(var ability in special.Abilities)
                            details += $"{ability.name.Trim()}, ";
                    
                    details += "\n";
                    
                    if (special.Reactions is null || (special.Reactions.Count == 0))
                        details += "NONE\t";
                    else
                        foreach(var reaction in special.Reactions)
                            details += $"{reaction.name.Trim()}, ";
                    
                    details += "\n";
                }
                
            }

        }
        var content = new GUIContent(details);

        GUIStyle listStyle = CustomTools.AssetTools.CreateStyle(12, Color.white);
        var size = listStyle.CalcSize(content);

        EditorGUILayout.LabelField(details, listStyle, GUILayout.Width(size.x), GUILayout.Height(size.y));
        EditorGUILayout.EndVertical();
    }

    private void RefreshEncounterRewards()
    {
        var guids =  AssetDatabase.FindAssets("t:Encounter");
        foreach(string guid in guids)
        {
            
            allEncounters = CustomTools.AssetTools.GetAllAssetsOfType<BattleEncounter>("Encounter");
            foreach(BattleEncounter battleEncounter in allEncounters)
            {
                battleEncounter.GetRewards();
            }
        }
        Debug.Log("The function ran.");
    }

    void RefreshEncounterList()
    {
        allEncounters = CustomTools.AssetTools.GetAllAssetsOfType<BattleEncounter>("Encounter");
    }
}

//#endif