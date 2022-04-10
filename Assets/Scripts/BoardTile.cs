using UnityEngine;
using System.Collections.Generic;

public class BoardTile : MonoBehaviour
{
    //  Neighbors
    [HideInInspector] public BoardTile east, west, north, south;    // Normally should be private, but for editor I set this to public

    //  Use this to set the tile as spawner
    #region Spawner
    [SerializeField] private bool isSpawner;
    [SerializeField] private float spawnCoolDown;
    private float spawnCoolDownTimer;
    private bool canSpawn;
    #endregion


    //  Drop
    private Drop drop;
    private DropType dropType;

    //  Will hold the matched tiles during check
    private List<BoardTile> matchedTilesColumn;
    private List<BoardTile> matchedTilesRow;

    private void Start()
    {
        //  Initialize the list
        matchedTilesColumn = new List<BoardTile>();
        matchedTilesRow = new List<BoardTile>();
    }

    private void Update()
    {
        //  If this is a spawner drop Will check whether self drop is empty, if so, will spawn drop!
        if (isSpawner && drop == null)
        {
            if(canSpawn)
            {
                Board.instance.GetDropSpawner().SpawnDrop(this);
                canSpawn = false;
            }
            else
            {
                CheckCoolDown();
            }
        }
    }

    //  Checks the cool down for spawning drops
    #region CoolDown
    void CheckCoolDown()
    {
        spawnCoolDownTimer += Time.deltaTime;

        if (spawnCoolDownTimer >= spawnCoolDown)
        {
            spawnCoolDownTimer = 0f;
            //  Set can spawn to true
            canSpawn = true;
        }
    }
    #endregion


    //  Use these functions to set the the neighbors of tiles during grid creation
    #region Set Neighbors On Tile Spawn
    public static void SetEastWestNeighbors(BoardTile east, BoardTile west)
    {
        west.east = east;
        east.west = west;
    }
    public static void SetNorthSouthNeighbors(BoardTile north, BoardTile south)
    {
        south.north = north;
        north.south = south;
    }

    #endregion


    //  Use these functions to prevent matching tiles for drop spawning
    #region Check Matching Tiles on Start
    public DropType CheckWestNeighbors()
    {
        //  We need to check for 2 consecutive same type drops
        DropType westType = DropType.Null;
        DropType westOfWestType = DropType.Null;

        if (west != null && west.west != null)
        {
            westType = west.GetDropType();
            westOfWestType = west.west.GetDropType();
        }

        if (westType == westOfWestType)
        {
            //  If both neighbors are same, return their type so we can remove it from the list
            return westType;
        }

        //  else return null type
        return DropType.Null;
    }

    public DropType CheckNorthNeighbors()
    {
        //  We need to check for 2 consecutive same type drops
        DropType northType = DropType.Null;
        DropType northOfNorthType = DropType.Null;

        if (north != null && north.north != null)
        {
            northType = north.GetDropType();
            northOfNorthType = north.north.GetDropType();
        }

        if (northType == northOfNorthType)
        {
            //  If both neighbors are same, return their type so we can remove it from the list
            return northType;
        }

        //  else return null type
        return DropType.Null;
    }

    #endregion


    //  Returns Count of Adjacent Drops of same color
    #region Check Match

    public int CheckConsecutiveColumns(DropType dropType, SwipeDirection swipeDirection)
    {
        //  Clear matches
        matchedTilesColumn.Clear();

        int total = 0;

        BoardTile neighbor = GetDirectionNeighbor(swipeDirection);

        //  Check whether we swipe towards up or down , if so count neighbors with same drop type in that direction, ignore self direction
        if (swipeDirection == SwipeDirection.Up || swipeDirection == SwipeDirection.Down)
        {
            if (neighbor != null && neighbor.GetDropType() == dropType)
            {
                total += CountNeighborsInDirectionRecursively(0, neighbor, dropType, swipeDirection);
            }
        }

        //  Check neighbors in both sides unless they are null or diferent drop type
        else
        {
            if (GetDirectionNeighbor(SwipeDirection.Up) != null && GetDirectionNeighbor(SwipeDirection.Up).GetDropType() == dropType)
            {
                total += CountNeighborsInDirectionRecursively(0, north, dropType, SwipeDirection.Up);
            }

            if (GetDirectionNeighbor(SwipeDirection.Down) != null && GetDirectionNeighbor(SwipeDirection.Down).GetDropType() == dropType)
            {
                total += CountNeighborsInDirectionRecursively(0, south, dropType, SwipeDirection.Down);
            }
        }

        return total;
    }

    public int CheckConsecutiveRows(DropType dropType, SwipeDirection swipeDirection)
    {
        matchedTilesRow.Clear();

        int total = 0;

        BoardTile neighbor = GetDirectionNeighbor(swipeDirection);

        //  Check whether we swipe towards left or right , if so count neighbors with same drop type in that direction
        if (swipeDirection == SwipeDirection.Left || swipeDirection == SwipeDirection.Right)
        {
            if (neighbor != null && neighbor.GetDropType() == dropType)
            {
                total += CountNeighborsInDirectionRecursively(0, neighbor, dropType, swipeDirection);
            }
        }

        //  Check neighbors in both sides unless they are null or diferent drop type
        else
        {
            if (GetDirectionNeighbor(SwipeDirection.Left) != null && GetDirectionNeighbor(SwipeDirection.Left).GetDropType() == dropType)
            {
                total += CountNeighborsInDirectionRecursively(0, west, dropType, SwipeDirection.Left);
            }

            if (GetDirectionNeighbor(SwipeDirection.Right) != null && GetDirectionNeighbor(SwipeDirection.Right).GetDropType() == dropType)
            {
                total += CountNeighborsInDirectionRecursively(0, east, dropType, SwipeDirection.Right);
            }
        }   
        return total;
    }

    #endregion


    //  Function to Check Given Side of Neighbors and Return Count of Adjacent Colors in Direction
    #region Recursive Neighbor-Side Check

    //  Recursively counts number of adjacent tiles
    private int CountNeighborsInDirectionRecursively(int adjacent, BoardTile directionNeighbor, DropType dropType,SwipeDirection direction)
    {
        // Base condition:
        if (directionNeighbor == null || directionNeighbor.GetDropType() != dropType)
        {
            //  Increment number of adjacent tiles
            return adjacent;
        }

        // Recursive Part:

        //  Add the Neighbor To The Matched List if it has a drop
        if (directionNeighbor.drop != null && (direction == SwipeDirection.Left || direction == SwipeDirection.Right))
        {
            matchedTilesRow.Add(directionNeighbor);
        }
        else if(directionNeighbor.drop != null && (direction == SwipeDirection.Up || direction == SwipeDirection.Down))
        {
            matchedTilesColumn.Add(directionNeighbor);
        }
        //  Increment adjacent
        adjacent++;
        //  Get Next Neighbor in Same direction
        BoardTile nextNeighbor = directionNeighbor.GetDirectionNeighbor(direction);
        //  Call the same function for the next neighbor
        return CountNeighborsInDirectionRecursively(adjacent, nextNeighbor , dropType, direction);
    }
    #endregion
    
        
    //  Match the Drops
    #region Destroy Matched
    public void MatchDrops()
    {
        //  Get Column List Count
        int listCountCol = matchedTilesColumn.Count;

        if (listCountCol >= 2)
        {
            for (int i = 0; i < listCountCol; i++)
            {
                //  Get target tile
                BoardTile targetTile = matchedTilesColumn[0];
                //  Match the drop in tile
                matchedTilesColumn[0].GetDrop().Match();
                //  Remove target tile from the list
                matchedTilesColumn.Remove(targetTile);
            }
        }
        

        int listCountRow = matchedTilesRow.Count;

        if (listCountRow >= 2)
        {
            for (int i = 0; i < listCountRow; i++)
            {
                //  Get target tile
                BoardTile targetTile = matchedTilesRow[0];
                //  Match the drop in tile
                matchedTilesRow[0].drop.Match();
                //  Remove target tile from the list
                matchedTilesRow.Remove(targetTile);
            }
        }

        //  Match Self Drop
        if (listCountRow >= 2 || listCountCol >= 2)
        {
            drop.Match();
        }
    }
    #endregion

    //  Fill Empty South Tiles
    #region Find Empty South Tiles

    //  Returns empty southern tile
    public BoardTile ReturnEmptySouthernTile(BoardTile south)
    {        
        //  Base Case

        //  If south neighbor of south is not null and doesn't contain a drop
        if (south.south == null || south.south.GetDropType() != DropType.Null)
        {
            return south;
        }

        return ReturnEmptySouthernTile(south.south);
    }
    #endregion


    //  Getters and Setters
    #region Getters & Setters

    //  Drops
    public void SetDrop(Drop drop)
    {
        if (drop == null)
        {
            this.drop.SetTile(null);
            this.drop = null;
            dropType = DropType.Null;
        }
        else
        {
            this.drop = drop;
            this.drop.SetTile(this);
            dropType = drop.GetDropType();
        }
    }

    public Drop GetDrop()
    {
        return drop;
    }

    public DropType GetDropType()
    {
        return dropType;
    }
    public void SetDropType(DropType dropType)
    {
        this.dropType = dropType;
    }
    //  Neighbors
    public BoardTile GetWestNeighbor()
    {
        return west;
    }

    public BoardTile GetEastNeighbor()
    {
        return east;
    }

    public BoardTile GetNorthNeighbor()
    {
        return north;
    }

    public BoardTile GetSouthNeighbor()
    {
        return south;
    }

    //  Returns the neighbor in provided swipe direction
    public BoardTile GetDirectionNeighbor(SwipeDirection direction)
    {
        switch (direction)
        {
            case SwipeDirection.Up:
                return north;
            case SwipeDirection.Down:
                return south;
            case SwipeDirection.Left:
                return west;
            case SwipeDirection.Right:
                return east;
            case SwipeDirection.Null:
                return null;
            default:
                return null;
        }
    }
    #endregion

}
