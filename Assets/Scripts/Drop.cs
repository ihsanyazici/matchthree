using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private DropType dropType;

    private BoardTile selfTile;

    private Board board;
    //  Animation
    private Animator anim;

    #region Swap-Lerp
    [SerializeField] private float swapTime;
    private float swapTimer;
    private bool isSwapping;
    private Vector3 startPosition;
    private BoardTile destTile;
    #endregion

    #region Slide-Lerp
    [SerializeField] private float slideLerpTime;
    private float slideLerpTimer;
    private bool isSliding;
    #endregion

    #region WrongMoveLerp
    [SerializeField] private float halfMoveTime;
    private float halfMoveTimer;
    private bool isFalseMove;
    private bool isDirection;
    #endregion

    private void Start()
    {
        board = LevelManager.GetCurrentLevel().GetBoard();
        //  Get the animator of drop
        anim = GetComponent<Animator>();
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
        else if (isFalseMove)
        {
            WrongMove();
        }
        else
        {
            CheckBelow();
        }
    }

    //  Checks whether a swipe is permitted
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
            //  Play Wrong move
            SetWrongDestination(destinationTile);
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

        Drop tmp = destinationTile.GetDrop();
        //  If there are more than or equal to 2 adjacent same drops, there is a match!!
        if (tmp != null && !tmp.IsInAction() && columnCount >= 2 || rowCount >= 2)
        {
            return true;
        }

        return false;
    }

    #endregion


    //  Swap animation -- using lerp
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
        swapTimer += Time.deltaTime;

        if (swapTimer <= swapTime)
        {
            //  Lerp Towards Destination Tile Location
            transform.position = Vector3.Lerp(startPosition, destTile.transform.position, swapTimer / swapTime);
        }
        else
        {
            //  Make sure final destination is reached
            transform.position = destTile.transform.position;
            //  Reset Lerp Timer
            swapTimer = 0;
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


    //  Slide animation -- using lerp
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


    //  False move animation -- using lerp
    #region FalseMove
    void SetWrongDestination(BoardTile destinationTile)
    {
        //  Set start position as your current position
        startPosition = transform.position;
        //  Set destination tile
        destTile = destinationTile;
        //  Start False Move
        isFalseMove = true;
        //  Set isDirection to move towards direction
        isDirection = true;
    }

    void WrongMove()
    {
        //  Increment Timer
        halfMoveTimer += Time.deltaTime;

        if (isDirection)
        {
            
            if (halfMoveTimer <= halfMoveTime)
            {
                //  Lerp Towards Destination Tile Location
                transform.position = Vector3.Lerp(startPosition, destTile.transform.position, halfMoveTimer/halfMoveTime);
            }
            else
            {
                //  Make sure we reached destination
                transform.position = destTile.transform.position;
                //  Reset start position
                startPosition = transform.position;
                //  Reset timer
                halfMoveTimer = 0f;
                //  Reverse direction
                isDirection = false;
            }
        }
        else
        {
            if (halfMoveTimer <= halfMoveTime)
            {
                //  Lerp Towards Self Tile Location
                transform.position = Vector3.Lerp(startPosition, selfTile.transform.position, halfMoveTimer / halfMoveTime);
            }
            else
            {
                //  Make sure we reached destination
                transform.position = selfTile.transform.position;
                //  Reset timer
                halfMoveTimer = 0f;
                //  Reset wrong move
                isFalseMove = false;
                //  Clear destination tile
                destTile = null;
            }
        }
    }
    #endregion


    //  Matches the drops and plays animation
    #region Match
    public void Match()
    {
        //  Add this drop to matching drops
        board.AddToMatchingDrops(this);
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
        board.GetDropSpawner().RemoveFromDropList(this);
    }
    #endregion


    //  Checks the tile below if there is one!
    #region CheckBelow
    void CheckBelow()
    {
        //  If drop has a south tile
        if (board.IsMatchingListEmpty() && selfTile != null && selfTile.south != null)
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


    //  Animator related stuff
    #region Animator
    void SetNewAnim(string newAnim)
    {
        anim.SetTrigger(newAnim);
    }
    #endregion


    //  Deactivates the drops and removes from list that holds inside currently matching drops
    public void DeactivateDrop()
    {
        //  Disable gameobject
        gameObject.SetActive(false);
        //  Remove from matchind drops
        board.RemoveFromMatchingDrops(this);
    }


    //  Resets the drops
    public void ResetDrop()
    {
        //  Reset anim trigger from matched
        anim.ResetTrigger("Match");
        //  Activate the drop
        gameObject.SetActive(true);
        //  Reset booleans
        isSliding = false;
        isSwapping = false;
        isFalseMove = false;
        //  Reset Position
        transform.localPosition = Vector3.zero;
        //  Reset timers
        swapTimer = 0f;
        slideLerpTimer = 0f;
        halfMoveTimer = 0f;
    }


    //  Returns whether selected drop is in action or not
    public bool IsInAction()
    {
        if (!isSwapping && !isSliding && !isFalseMove)
        {
            return false;
        }
        else
        {
            return true;
        }
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
