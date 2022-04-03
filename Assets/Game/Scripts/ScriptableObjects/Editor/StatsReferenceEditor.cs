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
        ts.stats.Stats().health.Value = EditorGUILayout.IntField("health", ts.stats.Stats().health.Value);
        ts.stats.Stats().mana.Value = EditorGUILayout.IntField("mana", ts.stats.Stats().mana.Value);
        ts.stats.Stats().agility.Value = EditorGUILayout.IntField("agility", ts.stats.Stats().agility.Value);
        ts.stats.Stats().strength.Value = EditorGUILayout.IntField("strength", ts.stats.Stats().strength.Value);
        ts.stats.Stats().healthRegeneration.Value = EditorGUILayout.IntField("healthRegeneration", ts.stats.Stats().healthRegeneration);
        ts.stats.Stats().manaRegeneration.Value = EditorGUILayout.IntField("manaRegeneration", ts.stats.Stats().manaRegeneration.Value);
        ts.stats.Stats().attackSpeed.Value = EditorGUILayout.IntField("attackSpeed", ts.stats.Stats().attackSpeed.Value);

        // Show default inspector property editor
    }
}
