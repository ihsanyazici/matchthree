using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    //  This class acts like a board-manager
    private GridCreator gridCreator;
    private DropSpawner dropSpawner;

    //  Will hold a score, just for fun!
    private int score;

    //  Will hold the drops that are currently matching
    private List<Drop> matchingDrops;

    private void Start()
    {
        //  Get Grid Creator
        gridCreator = GetComponent<GridCreator>();
        //  Get Drop Spawner
        dropSpawner = GetComponent<DropSpawner>();

        //  Initialize Drop Spawner
        dropSpawner.InitDropSpawner(gridCreator);
        //  Initialize matching drops list
        matchingDrops = new List<Drop>();
    }


    #region Handle Matching Drops
    public void AddToMatchingDrops(Drop drop)
    {
        if (!matchingDrops.Contains(drop))
        {
            //  I could call this inside drop class's match function but doesn't change anything
            IncrementScore();
            matchingDrops.Add(drop);
        }
    }

    public void RemoveFromMatchingDrops(Drop drop)
    {
        matchingDrops.Remove(drop);
    }

    public bool IsMatchingListEmpty()
    {
        if (matchingDrops.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region Score
    void IncrementScore()
    {
        score++;

        UIManager.instance.SetScoreText(score);
    }
    #endregion

    #region CheckGameOver

    //  Checks whether any valid movement left or not
    void CheckValidMovements()
    {
        //  TO DO

        //  Create a bool isOver, set it to true  

        //  IF any time left, come back to this

        //  Loop through each tile, if they have any drop, continue below

        //  Each drop checks for each direction whether there are any valid moves or not

        //  Even if a single drop can move, set isOver to false and return
        
        //  Else keep isOver as it is and end the game by calling GameManager.instance.EndGame(false); meaning we lost
    }
    #endregion

    #region Getters & Setters
    public GridCreator GetGridCreator()
    {
        return gridCreator;
    }
    public DropSpawner GetDropSpawner()
    {
        return dropSpawner;
    }
    #endregion

}
