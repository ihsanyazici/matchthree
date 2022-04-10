using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private List<Drop> dropPool;

    // Start is called before the first frame update
    void Start()
    {
        //  Initialize the list
        dropPool = new List<Drop>();
    }

    //  Add & Remove From the Pool
    public void AddDropToPool(Drop drop)
    {
        //  Add drop to the pool
        dropPool.Add(drop);
        //  Add drop as child
        drop.transform.parent = transform;
    }
    public void RemoveFromPool(Drop drop)
    {
        //  Remove from the pool
        dropPool.Remove(drop);
        //  Activate
        drop.gameObject.SetActive(true);
    }


    //  Returns whether pool is empty or not
    public bool IsEmpty()
    {
        if (dropPool.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //  Returns pool count
    public int PoolCount()
    {
        return dropPool.Count;
    }
    
    //  Returns pool list
    public List<Drop> ReturnPoolList()
    {
        return dropPool;
    }

    //  Returns a random drop from the pool
    public Drop ReturnRandomDropFromPool()
    {
        //  I check whether list is empty or not using IsEmpty, therefor I won't check it here
        
        //  Pick a random index
        int index = Random.Range(0, dropPool.Count);
        //  Get a random drop
        Drop drop = dropPool[index];
        //  Remove from the pool
        RemoveFromPool(drop);
        //  Return random drop from pool
        return drop;       
    }
}
