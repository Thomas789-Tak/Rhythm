using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] GameObject Road;
    [SerializeField] GameObject Line;
    [SerializeField] GameObject OutLine;
    [SerializeField] GameObject Way;
    int roadCount;
    void Start()
    {
        roadCount = 6;
        Instantiate(OutLine, new Vector3(400, 0, 4), Quaternion.Euler(0, 90, 0), Way.transform);
        for (int i = 0; i < roadCount; i++)
        {
            Instantiate(Road, new Vector3(400, 0, i * -8f), Quaternion.Euler(0, 90, 0), Way.transform);
        }
        for (int i = 0; i <= roadCount - 2; i++)
        {
            Instantiate(Line, new Vector3(400, 0, -4 + (i * -8f)), Quaternion.Euler(0, 90, 0), Way.transform);
        }
        Instantiate(OutLine, new Vector3(400, 0, 4 + (roadCount * -8f)), Quaternion.Euler(0, 90, 0), Way.transform);
        Instantiate(Way, new Vector3(-1000, 0, 0), Quaternion.identity, Way.transform);
        Way.AddComponent<RoadController>();
    }

    void Update()
    {
        
    }
}
