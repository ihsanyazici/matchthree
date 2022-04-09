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
        matchingDrops.Add(drop);
    }
    public void RemoveFromMatchingDrops(Drop drop)
    {
        matchingDrops.Remove(drop);

        //  If after removal, no drops left inside, fill empty tiles
        for (int i = 0; i < dropSpawner.GetDrops().Count; i++)
        {
            //dropSpawner.GetDrops()[i].GetTile().FillEmptyTiles();
        }

        for (int i = 0; i < gridCreator.GetTiles().Length; i++)
        {
            //gridCreator.GetTiles()[i].FillEmptyTiles();
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
