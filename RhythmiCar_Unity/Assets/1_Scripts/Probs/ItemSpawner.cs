using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemList;
    [SerializeField] List<LevelInfo> levels;
    int currentStage;
    [SerializeField] int itemInterval;
    [SerializeField] int itemStartPos;
    void Start()
    {
        currentStage = 0;
        for(int i=0;i<levels[currentStage].SpawnList.Count;i++)
        {
            Instantiate(ItemList[(int)levels[currentStage].SpawnList[i]], new Vector3((i + itemStartPos) * itemInterval, 1.5f, (-8)*levels[currentStage].SpawnPosZ[i]), Quaternion.identity) ;
        }
    }


    void Update()
    {
        
    }
    [System.Serializable]
    public class LevelInfo
    {
        public List<float> SpawnPosZ;
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
