using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    //  This class acts like a board-manager
    #region Singleton
    public static Board instance;

    private void Awake()
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

    [SerializeField] private GridCreator gridCreator;
    [SerializeField] private DropSpawner dropSpawner;

    //  Will hold the drops that are currently matching
    private List<Drop> matchingDrops;

    private void Start()
    {
        //  Initialize Drop Spawner
        dropSpawner.InitDropSpawner();
        //  Initialize matching drops list
        matchingDrops = new List<Drop>();
    }

    public void AddToMatchingDrops(Drop drop)
    {
        if (!matchingDrops.Contains(drop))
        {
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
