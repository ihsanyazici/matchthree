using System.Collections.Generic;
using UnityEngine;

public class DropSpawner : MonoBehaviour
{
    #region Drops
    [SerializeField] private List<Drop> dropPrefabs;
    #endregion

    //  Grid Creator
    private GridCreator gridCreator;
    private BoardTile[] tiles;
    private List<Drop> drops;
    private int boardSize;

    public void InitDropSpawner()
    {
        //  Set grid creator
        gridCreator = Board.instance.GetGridCreator();
        //  Set self tiles list for easy access
        tiles = gridCreator.GetTiles();
        // Initialize the list
        drops = new List<Drop>();
        //  Set the board size
        boardSize = tiles.Length;
        SpawnDrops();
    }

    void SpawnDrops()
    {
        for (int i = 0; i < boardSize; i++)
        {
            //  First we need a list to hold initial drops!
            List<Drop> usableDrops = new List<Drop>(dropPrefabs);
            //  Columns can check possible drops with previous tiles    -- Each tile should check their north because of the way I lay tiles
            RemoveDropTypeFromList(usableDrops,tiles[i].CheckNorthNeighbors());
            //  Rows can check possible drops with previous tiles       -- Each tile should check their west because of the way I lay tiles
            RemoveDropTypeFromList(usableDrops, tiles[i].CheckWestNeighbors());
            //  Then we can pick random from remaining drops and Instantiate using own tile as Parent
            Drop tmp = Instantiate(GetRandomDrop(usableDrops),tiles[i].transform);
            //  Set drop of the tile and tile of the drop
            tiles[i].SetDrop(tmp);
            //  Add Created Drops to the list
            drops.Add(tmp);
        }
    }

    List<Drop> RemoveDropTypeFromList(List<Drop> dropsList,DropType removeType)
    {
        //  If our type is not null
        if (removeType != DropType.Null)
        {
            //  Loop through the list
            for (int i = 0; i < dropsList.Count; i++)
            {
                //  If you have same drop type, remove that drop from the list
                if (dropsList[i].GetDropType() == removeType)
                {
                    dropsList.Remove(dropsList[i]);
                }
            }
        }
        return dropsList;
    }

    Drop GetRandomDrop(List<Drop> usableDrops)
    {
        int rand = Random.Range(0,usableDrops.Count);
        return usableDrops[rand];
    }

    public List<Drop> GetDrops()
    {
        return drops;
    }

    public void RemoveFromDropList(Drop drop)
    {
        drops.Remove(drop);
    }
    public void AddToDropList(Drop drop)
    {
        drops.Add(drop);
    }

}
