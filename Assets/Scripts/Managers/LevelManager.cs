using UnityEngine;
using System.Collections.Generic;

public static class LevelManager
{
    private static int _levelIndex;

    private static LevelList _levelList;
    private static Level _currentLevel;

    //  Use this to set the level manager at the Start() of GameManager class!
    public static void SetLevelManager(LevelList list)
    {
        _levelList = list;
        _levelIndex = PlayerPrefs.GetInt("levelIndex", 0);

        LoadLevel();
    }

    //  Instantiates and sets _currentLevel
    public static void LoadLevel()
    {
        _currentLevel = GameObject.Instantiate(_levelList.levels[_levelIndex % _levelList.levels.Count]);
    }

    public static void NextLevel()
    {
        //  Destroy current level
        _currentLevel.DestroyLevel();

        //  Increment level index
        _levelIndex++;
        PlayerPrefs.SetInt("levelIndex", _levelIndex);

        LoadLevel();
    }

    public static void RestartLevel()
    {
        //  Destroy current level
        _currentLevel.DestroyLevel();

        LoadLevel();
    }

    public static Level GetCurrentLevel()
    {
        return _currentLevel;
    }

    public static int GetCurrentLevelIndex()
    {
        return _levelIndex;
    }

    public static int GetCurrentLevelNumber()
    {
        return _levelIndex + 1;
    }
}
