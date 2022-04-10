using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelName;
    [HideInInspector] public LevelType levelType;
    [HideInInspector] public int levelTemplate;

    //  Hold board inside
    private Board board;

    private void Awake()
    {
        //  Get the board from components
        board = GetComponent<Board>();
    }

    public void DestroyLevel()
    {
        //  Use this function to destroy level and anything related that needs to be destroyed
        Destroy(gameObject);
    }

    //  Getters & Setters
    public Board GetBoard()
    {
        return board;
    }
}
