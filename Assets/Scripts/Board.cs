using UnityEngine;

public class Board : MonoBehaviour
{
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
 

    private void Start()
    {
        //  Initialize Drop Spawner
        dropSpawner.InitDropSpawner();
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
