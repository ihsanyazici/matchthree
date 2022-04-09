using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private DropType dropType;

    private BoardTile selfTile;

    //  Animation
    private Animator anim;

    #region Swap-Lerp
    [SerializeField] private float lerpTime;
    private float lerpTimer;
    private bool isSwapping;
    private Vector3 startPosition;
    private BoardTile destinationTile;
    #endregion

    #region Slide-Lerp
    [SerializeField] private float slideTime;
    private float slideTimer;
    private bool isSliding;
    #endregion

    private void Start()
    {
        //  Get the animator of drop
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isSwapping)
        {
            MoveToDestinationTile();
        }
    }

    #region Swipe-Check
    //  Checks Whether We can swipe to Destination
    public bool CanSwipeToDestination(BoardTile destinationTile, SwipeDirection swipeDirection)
    {
        //  If destination tile or its drop is not null and there is a match
        if (destinationTile != null && destinationTile.GetDrop() != null && CheckMatch(destinationTile, swipeDirection))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //  Checks whether swipe action is permitted to destination tile
    bool CheckMatch(BoardTile destinationTile, SwipeDirection swipeDirection)
    {
        //  Check count of adjacent same type drops in column
        int columnCount = destinationTile.CheckConsecutiveColumns(dropType, swipeDirection);
        //  Check count of adjacent same type drops in row
        int rowCount = destinationTile.CheckConsecutiveRows(dropType, swipeDirection);

        //  If there are more than or equal to 2 adjacent same drops, there is a match!!
        if (columnCount >= 2 || rowCount >= 2)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Swap Drops


    public void SetDestination(BoardTile destinationTile)
    {
        //  Set start position as your current position
        startPosition = transform.position;
        //  Set destination tile
        this.destinationTile = destinationTile;
        //  Set Yourself As Drop of Destination Tile
        destinationTile.SetDrop(this);
        //  Start Swap
        isSwapping = true;
        //  Set New Parent to Destinatian Tile
        transform.parent = destinationTile.transform;
    }

    public void MoveToDestinationTile()
    {
        lerpTimer += Time.deltaTime;

        if (lerpTimer <= lerpTime)
        {
            //  Lerp Towards Destination Tile Location
            transform.position = Vector3.Lerp(startPosition, destinationTile.transform.position, lerpTimer / lerpTime);
        }
        else
        {
            //  Reset Lerp Timer
            lerpTimer = 0;
            //  Stop Swap
            isSwapping = false;
            //  Check Match
            if (CheckMatch(destinationTile,SwipeDirection.Null))
            {
                selfTile.MatchDrops();
            }
        }
    }


    #endregion

    #region Match
    public void Match()
    {
        //  Add this drop to matching drops
        Board.instance.AddToMatchingDrops(this);
        //  Switch to match animation
        anim.CrossFadeInFixedTime("Match", 0f);
        //  Remove Self From Tile and Add to the Pool
        RemoveFromTile();
        //  Remove from matchind drops
        Board.instance.RemoveFromMatchingDrops(this);
    }

    public void RemoveFromTile()
    {
        //  Empty tile
        selfTile.SetDrop(null);
        //  Add yourself to the object pool
        GameManager.instance.Pool.AddDropToPool(this);
        //  Remove yourself from drops list
        Board.instance.GetDropSpawner().RemoveFromDropList(this);
    }
    #endregion

    //  Getters & Setters
    #region Getters & Setters
    public DropType GetDropType()
    {
        return dropType;
    }
    public void SetDropType(DropType dropType)
    {
        this.dropType = dropType;
    }
    public BoardTile GetTile()
    {
        return selfTile;
    }
    public void SetTile(BoardTile boardTile)
    {
        selfTile = boardTile;
    }
    public void SetTileEmpty()
    {
        selfTile = null;
    }
    #endregion

}
