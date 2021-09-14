using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public List<GameObject> ItemList;
    [SerializeField] List<LineInfo> line= new List<LineInfo>();
    [SerializeField] int itemInterval;
    [SerializeField] int itemStartPos;
    int currentStage;
    int roadCount;
    int itemLength;

    private void Awake()
    {
        
        string stagePath = "Data/StageInfo";
        string spawnPath = "Data/SpawnData";
        List<Dictionary<string, object>> stageInfo = CSVReader.Read(stagePath);
        List<Dictionary<string, object>> spawnData = CSVReader.Read(spawnPath);

        currentStage = 0; // initGamemanager 에서 호출
        itemInterval = (int)stageInfo[currentStage]["itemInterval"];
        itemStartPos = (int)stageInfo[currentStage]["itemStartPos"];
        roadCount = (int)stageInfo[currentStage]["road"];
        itemLength = (int)stageInfo[currentStage]["itemLength"];
        for(int i=0;i<roadCount;i++)
        {           
            LineInfo road = new LineInfo();
            line.Add(road);

            for(int j=0;j<itemLength;j++)
            {
                line[i].SpawnList.Add((ESpawnList)spawnData[(currentStage*8)+i][j+"m"]);
            }
        }
    }
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
        public List<ESpawnList> SpawnList= new List<ESpawnList>();       
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
