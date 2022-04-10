using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    [SerializeField] private Swipe swipe;
    private Drop selectedDrop;
    private Camera mainCam;

    // Start is called before the first frame update
    void Start()
    {
        //  Set main camera
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.IsGameStarted)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CastRayToObject();
            }

            CheckSwipe();
        }       
    }

    //  Check Swipe -- Depending on the Given Directions,
    void CheckSwipe()
    {
        if (selectedDrop != null && selectedDrop.GetTile() != null)
        {
            //  Get swipe direction
            SwipeDirection swipeDirection = GetSwipeDirection();

            //  If there is a valid swipe in any direction (Up, Down, Left or Right)
            if (swipeDirection != SwipeDirection.Null)
            {
                //  Get the board tile of selected drop
                BoardTile boardTile = selectedDrop.GetTile();
                //  Get the destination tile
                BoardTile destinationTile = boardTile.GetDirectionNeighbor(swipeDirection);
                //  Get the drop in destination tile if it's not null
                if (destinationTile != null)
                {
                    Drop dropInDestinationTile = destinationTile.GetDrop();

                    if (selectedDrop.CanSwipeToDestination(boardTile.GetDirectionNeighbor(swipeDirection), swipeDirection))
                    {
                        selectedDrop.SetDestination(destinationTile);
                        dropInDestinationTile.SetDestination(boardTile);
                    }
                }
                //  Swipe the drop Towards Destination
                ResetSelectedDrop();
            }            
        }        
    }

    //  Returns the swipe direction
    SwipeDirection GetSwipeDirection()
    {
        if (swipe.SwipeUp)
        {
            return SwipeDirection.Up;
        }
        else if (swipe.SwipeDown)
        {
            return SwipeDirection.Down;
        }
        else if (swipe.SwipeLeft)
        {
            return SwipeDirection.Left;
        }
        else if (swipe.SwipeRight)
        {
            return SwipeDirection.Right;
        }
        else
        {
            return SwipeDirection.Null;
        }
    }

    //  Resets drop, we no longer require chosen drop object
    void ResetSelectedDrop()
    {
        //  Reset Drop
        selectedDrop = null;
    }

    //  Casts ray under mouse position and selects a drop if ray hits a drop collider
    void CastRayToObject()
    {
        //  Get Mouse Position
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z);

        RaycastHit2D hit = Physics2D.Raycast(mainCam.ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hit.collider != null && hit.collider.GetComponent<Drop>() != null)
        {
            Drop tmp = hit.collider.GetComponent<Drop>();

            if (!tmp.IsInAction())
            {
                selectedDrop = tmp;
            }
        }
    }

}
