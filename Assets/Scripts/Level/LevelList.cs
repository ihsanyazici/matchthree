using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelList", menuName = "ScriptableObjects/LevelList", order = 1)]
public class LevelList : ScriptableObject
{
    public List<Level> levels;
    [HideInInspector]
    public GameReplayType gameReplayType;

    [HideInInspector]
    public int startIndex;
    [HideInInspector]
    public int endIndex;

    public Level GetLevel(int index)
    {
        if (index < levels.Count)
        {
            return levels[index];
        }
        else
        {
            switch (gameReplayType)
            {
                case GameReplayType.FromBeginning:
                    return levels[index % levels.Count];
                case GameReplayType.LastLevel:
                    return levels[levels.Count - 1];
                case GameReplayType.FromLevelIndex:
                    int mod = levels.Count - startIndex;
                    int i = startIndex + ((index - levels.Count) % mod);
                    return levels[i];
                case GameReplayType.RandomBetweenLevels:
                    i = Random.Range(startIndex, endIndex + 1);
                    return levels[i];
                default:
                    return levels[index % levels.Count];
            }
        }
    }
}


public enum GameReplayType
{
    FromBeginning,
    LastLevel,
    FromLevelIndex,
    RandomBetweenLevels
}
