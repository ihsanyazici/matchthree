using UnityEngine;

public class GridCreator : MonoBehaviour
{
    #region Board Creation
    [SerializeField] private Vector2Int boardSize;
    [SerializeField] private BoardTile tilePrefab;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Vector3 startPos;

    private Vector2 tileDistance;
    [HideInInspector] public BoardTile[] tiles; // Normally should be private but made it public for the editor code
    #endregion

    public void InitGridCreator()
    {
        //  Clear previous array if not null
        EraseGridCreator();
        //  Determine tile distance using sprite size for the tiles
        tileDistance = new Vector2(tilePrefab.GetComponent<SpriteRenderer>().bounds.size.x, tilePrefab.GetComponent<SpriteRenderer>().bounds.size.y);

        //  Since I will have fixed board size, no need to use list
        tiles = new BoardTile[boardSize.x * boardSize.y];

        SpawnGrids();
    }

    public void EraseGridCreator()
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
            {
                DestroyImmediate(tiles[i].gameObject);
            }
        }

        tiles = new BoardTile[0];
    }

    void SpawnGrids()
    {
        for (int counter = 0, column = 0; column < boardSize.x; column++)
        {
            for (int row = 0; row < boardSize.y; row++, counter++)
            {
                //  Instantiate the board tile and set parent
                BoardTile boardTile = Instantiate(tilePrefab, tileParent);

                //  Set position
                boardTile.transform.localPosition = new Vector3(startPos.x + tileDistance.x * row,startPos.y - tileDistance.y * column);
                boardTile.name = counter.ToString();

                //  Add created tiles to our tiles array
                tiles[counter] = boardTile;

                //  Set neighbors of the board tiles
                #region Set Neighbors
                if (row > 0)
                {
                    BoardTile.SetEastWestNeighbors(boardTile, tiles[counter - 1]);
                }
                if (column > 0)
                {
                    BoardTile.SetNorthSouthNeighbors(tiles[counter - boardSize.y], boardTile);
                }
                #endregion
            }
        }
    }

    // Getters & Setters
    public BoardTile[] GetTiles()
    {
        return tiles;
    }

}
