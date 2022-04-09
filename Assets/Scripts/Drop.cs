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
    private BoardTile destTile;
    #endregion

    #region Slide-Lerp
    [SerializeField] private float slideLerpTime;
    private float slideLerpTimer;
    private bool isSliding;
    #endregion

    private void Start()
    {
        //  Get the animator of drop
        anim = GetComponent<Animator>();
    }

    public void ResetDrop()
    {

    }

    private void Update()
    {
        if (isSwapping)
        {
            MoveToDestinationTile();
        }
        else if (isSliding)
        {
            SlideToDestinationTile();
        }
        else
        {
            CheckBelow();
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


    #region Swap

    public void SetDestination(BoardTile destinationTile)
    {
        //  Set start position as your current position
        startPosition = transform.position;
        //  Set destination tile
        destTile = destinationTile;
        //  Set Yourself As Drop of Destination Tile
        destinationTile.SetDrop(this);
        //  Start Swap
        isSwapping = true;
        //  Set New Parent to Destinatian Tile
        transform.parent = destinationTile.transform;
    }

    void MoveToDestinationTile()
    {
        lerpTimer += Time.deltaTime;

        if (lerpTimer <= lerpTime)
        {
            //  Lerp Towards Destination Tile Location
            transform.position = Vector3.Lerp(startPosition, destTile.transform.position, lerpTimer / lerpTime);
        }
        else
        {
            //  Make sure final destination is reached
            transform.position = destTile.transform.position;
            //  Reset Lerp Timer
            lerpTimer = 0;
            //  Stop Swap
            isSwapping = false;
            //  Check Match if not sliding
            if (!isSliding && CheckMatch(destTile,SwipeDirection.Null))
            {
                destTile.MatchDrops();
            }

            //  empty destination tile
            destTile = null;
        }
    }
    #endregion


    #region Slide
    void SetSlideDestination(BoardTile destinationTile)
    {
        //  Reset slide lerp timer
        slideLerpTimer = 0f;
        //  Empty selfTile since you don't swap but replace
        selfTile.SetDrop(null);
        //  Set start position as your current position
        startPosition = transform.position;
        //  Set destination tile
        destTile = destinationTile;
        //  Set Yourself As Drop of Destination Tile
        destinationTile.SetDrop(this);
        //  Start Swap
        isSliding = true;
        //  Set New Parent to Destinatian Tile
        transform.parent = destinationTile.transform;
    }

    void SlideToDestinationTile()
    {
        slideLerpTimer += Time.deltaTime;

        if (slideLerpTimer <= slideLerpTime)
        {
            //  Smooth t
            float t = EaseFunctions.EaseOut(slideLerpTimer / slideLerpTime);
            //  Lerp Towards Destination Tile Location
            transform.position = Vector3.Lerp(startPosition, destTile.transform.position,t);
        }
        else
        {
            //  Make sure final destination is reached
            transform.position = destTile.transform.position;
            //  Play drop anim
            anim.CrossFadeInFixedTime("Drop", 0f);
            //  Reset Lerp Timer
            slideLerpTimer = 0;
            //  Stop Slide
            isSliding = false;
            //  Check Match
            if (CheckMatch(destTile, SwipeDirection.Null))
            {
                destTile.MatchDrops();
            }
            //  empty destination tile
            destTile = null;
        }
    }
    #endregion


    #region Match
    public void Match()
    {
        //  Add this drop to matching drops
        Board.instance.AddToMatchingDrops(this);
        //  Remove Self From Tile and Add to the Pool
        RemoveFromTile();
        //  Switch to match animation
        //anim.CrossFadeInFixedTime("Match", 0f);
        SetNewAnim("Match");
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


    #region CheckBelow
    void CheckBelow()
    {
        //  If drop has a south tile
        if (Board.instance.IsMatchingListEmpty() && selfTile != null && selfTile.south != null)
        {
            //  if our south tile doesn't have a drop
            if (selfTile.south.GetDrop() == null)
            {
                BoardTile tile = selfTile.south.ReturnEmptySouthernTile(selfTile.south);
                SetSlideDestination(tile);
            }
        }
    }
    #endregion

    #region Animator
    void SetNewAnim(string newAnim)
    {
        anim.SetTrigger(newAnim);
    }
    #endregion
    public void DeactivateDrop()
    {
        //  Disable gameobject
        gameObject.SetActive(false);
        //  Remove from matchind drops
        Board.instance.RemoveFromMatchingDrops(this);
    }
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
