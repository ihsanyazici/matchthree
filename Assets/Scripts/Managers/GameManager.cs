using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager instance;

    void InitSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    [SerializeField] private LevelList _levelList;

    private static bool _gameStarted;

    [SerializeField] private Pool pool;


    private void Awake()
    {
        InitSingleton();
    }

    void Start()
    {   
        CameraManager.instance.Init();
        UIManager.instance.Init();
        LevelManager.SetLevelManager(_levelList);
    }

    public void StartGame()
    {
        StartGameInternal();
    }

    private void StartGameInternal()
    {
        _gameStarted = true;
        UIManager.instance.OnGameStart();

        //  ACTIONS     -->     Fill as you need
    }

    public void EndGame(bool isWon)
    {
        _gameStarted = false;

        UIManager.instance.OnGameEnd(isWon);
        if (isWon)
        {
            // Actions if game is won
        }
        else
        {
            // Actions if you lose
        }

        //  ACTIONS     -->     Fill as you need
    }

    public void NextGame()
    {
        LevelManager.NextLevel();
        StartGameInternal();

        //  ACTIONS     -->     Fill as you need
    }

    public void RestartGame()
    {
        LevelManager.RestartLevel();
        StartGameInternal();

        //  ACTIONS     -->     Fill as you need
    }

    public static bool IsGameStarted
    { get { return _gameStarted; } }

    public Pool Pool
    { get { return pool; } }
}
