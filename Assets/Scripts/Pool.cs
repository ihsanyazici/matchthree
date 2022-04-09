using System.Collections;
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
        //  Remove drop parent
        drop.transform.parent = null;
        //  Activate
        drop.gameObject.SetActive(true);
    }
}
