using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{

    public struct myCarData
    {
        
    }

    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Data/cardata");
        print(data[0]["id"]);
    }
}
