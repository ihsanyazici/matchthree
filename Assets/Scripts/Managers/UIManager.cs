using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager instance;

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

    #region Panels
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _winPanel;
    [SerializeField] private GameObject _losePanel;
    [SerializeField] private GameObject _hud;
    #endregion

    #region Texts
    [SerializeField] private TextMeshProUGUI scoreText;
    #endregion

    private void Awake()
    {
        InitSingleton();
    }

    public void Init()
    {
        
    }

    public void OnGameStart()
    {
        //  Disable Start Panel
        _startPanel.SetActive(false);
        //  Enable HUD
        _hud.SetActive(true);
        //  Reset Score Text
        ResetScoreText();
    }
    public void OnGameEnd(bool isWon)
    {
        _hud.SetActive(false);

        //  Enable Win/Lose Panel depending on win condition
        if (isWon)
        {
            _winPanel.SetActive(true);
        }
        else
        {
            _losePanel.SetActive(true);
        }

        //  ACTIONS     -->     Fill as you need
    }

    void ResetScoreText()
    {
        scoreText.text = 0f.ToString();
    }

    public void SetScoreText(int score)
    {
        scoreText.text = score.ToString();
    }
}
