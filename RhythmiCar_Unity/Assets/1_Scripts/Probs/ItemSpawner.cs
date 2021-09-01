using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemList;
    public List<GameObject> SpawnList;
    public List<int> SpawnPosZ;
    public List<Tak> takgi;

    void Start()
    {
        SpawnList.Add(ItemList[0]);
        
    }


    void Update()
    {
        
    }
    public class Tak
    {

    }
}
