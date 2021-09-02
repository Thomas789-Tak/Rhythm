using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemList;
    [SerializeField] List<LineInfo> line;
    [SerializeField] int itemInterval;
    [SerializeField] int itemStartPos;
    void Start()
    {
        for(int i=0; i<line.Count;i++)
        {
            for (int j = 0; j < line[i].SpawnList.Count; j++)
            {
                Instantiate(ItemList[(int)line[i].SpawnList[j]], new Vector3((j + itemStartPos) * itemInterval, 1.5f, (-8) * i), Quaternion.identity);
            }
        }

    }

    [System.Serializable]
    public class LineInfo
    {
        public List<ESpawnList> SpawnList;       
    }

    public enum ESpawnList
    {
        gold_copper,
        gold_silver,
        gold_gold,
        booster,
        star,
        note,
        rhythmEnergy,
        empty,
    }
}
