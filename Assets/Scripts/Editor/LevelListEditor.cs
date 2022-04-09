using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelList))]
public class LevelListEditor : Editor
{
    private LevelList levelList;

    public override void OnInspectorGUI()
    {
        levelList = target as LevelList;

        EditorGUI.BeginChangeCheck();
        GameReplayType grt = (GameReplayType)EditorGUILayout.EnumPopup("Game Replay Type", levelList.gameReplayType);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(levelList, "Game Replay Type");
            EditorUtility.SetDirty(levelList);
            levelList.gameReplayType = grt;
        }

        if (levelList.gameReplayType == GameReplayType.FromLevelIndex)
        {
            EditorGUI.BeginChangeCheck();
            int si = EditorGUILayout.IntSlider("Start Level", levelList.startIndex + 1, 1, levelList.levels.Count);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(levelList, "Start Level");
                EditorUtility.SetDirty(levelList);
                levelList.startIndex = si - 1;
            }
        }

        if (levelList.gameReplayType == GameReplayType.RandomBetweenLevels)
        {
            EditorGUI.BeginChangeCheck();
            int si = EditorGUILayout.IntSlider("Start Level", levelList.startIndex + 1, 1, levelList.endIndex + 1);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(levelList, "Start Level");
                EditorUtility.SetDirty(levelList);
                levelList.startIndex = si - 1;
            }

            EditorGUI.BeginChangeCheck();
            int ei = EditorGUILayout.IntSlider("End Level", levelList.endIndex + 1, levelList.startIndex + 1, levelList.levels.Count);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(levelList, "End Level");
                EditorUtility.SetDirty(levelList);
                levelList.endIndex = ei - 1;
            }
        }

        for (int i = 0; i < levelList.levels.Count; i++)
        {
            if (levelList.levels[i] != null)
            {
                levelList.levels[i].levelName = levelList.levels[i].name;
            }
        }

        base.OnInspectorGUI();
    }
}
