using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Armor))]
public class StatsReferenceEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Armor ts = (Armor)target;
        DrawDefaultInspector();
        ts.stats.Stats().health = EditorGUILayout.IntField("health", ts.stats.Stats().health);
        ts.stats.Stats().mana = EditorGUILayout.IntField("mana", ts.stats.Stats().mana);
        ts.stats.Stats().agility = EditorGUILayout.IntField("agility", ts.stats.Stats().agility);
        ts.stats.Stats().strength = EditorGUILayout.IntField("strength", ts.stats.Stats().strength);
        ts.stats.Stats().healthRegeneration = EditorGUILayout.IntField("healthRegeneration", ts.stats.Stats().healthRegeneration);
        ts.stats.Stats().manaRegeneration = EditorGUILayout.IntField("manaRegeneration", ts.stats.Stats().manaRegeneration);
        ts.stats.Stats().attackSpeed = EditorGUILayout.IntField("attackSpeed", ts.stats.Stats().attackSpeed);

        // Show default inspector property editor
    }
}
