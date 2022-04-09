using UnityEngine;

public class Level : MonoBehaviour
{
    public string levelName;
    [HideInInspector] public LevelType levelType;
    [HideInInspector] public int levelTemplate;

    public void DestroyLevel()
    {
        //  Use this function to destroy level and anything related that needs to be destroyed
        Destroy(gameObject);
    }
}
