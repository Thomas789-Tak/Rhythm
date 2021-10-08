using System.Collections;
using UnityEngine;
using System.Collections.Generic;


public class RhythmConstructer : MonoBehaviour
{
    private RhythmJudge RhythmJudge;

    private void Start()
    {
        RhythmJudge = FindObjectOfType<RhythmJudge>();

        RhythmJudge.ObjectPooling();
        this.CreateItem();
        RhythmJudge.SongStart();
    }

    public void CreateItem()
    {
        if (RhythmJudge.NotePool == null)
        {
            Debug.Log("Need \"Create Note!\"");
            return;
        }

        string stagePath = "Data/StageInfo";
        string spawnPath = "Data/SpawnData";

        List<Dictionary<string, object>> stageInfo = CSVReader.Read(stagePath);
        List<Dictionary<string, object>> spwanData = CSVReader.Read(spawnPath);

        int currentStage = 0;
        int currentDifficult = 0;

        int rowPerDifficult = 6 + 1;    // 6 Item Row + 1 Note Row
        int rowPerStage = 3 * rowPerDifficult;

        //int itemInterval = (int)stageInfo[currentStage]["itemInterval"];
        //int itemStartPos = (int)stageInfo[currentStage]["itemStartPos"];
        int roadCount = (int)stageInfo[currentStage]["road"];   // 도로의 수
        int itemLength = (int)stageInfo[currentStage]["itemLength"]; // 한 도로에 배치된 아이템의 총 수

        if (RhythmJudge.InItemList.Count != 0)
        {
            RhythmJudge.InItemList.ForEach(x => Destroy(x));
        }

        for (int i = 0; i < RhythmJudge.NotePool.Count; i++)
        {
            if (itemLength < i) break;

            GameObject Note = RhythmJudge.NotePool[i];
            for (int j = 1; j < roadCount; j++)
            {
                // 0' Row = Note Row, 1~6' Row = Item Row
                int id = (currentStage * rowPerStage) + (currentDifficult * rowPerDifficult) + j;
                GameObject item = RhythmJudge.ItemList[(int)spwanData[id][i + "m"]];
                //Vector3 pos = new Vector3(-4 + 2 * j, 2f, Note.transform.position.z);
                Vector3 localPos = new Vector3(-24 + 8 * j, 2f, 0);

                var nItem = Instantiate(item, new Vector3(0, 0, 0), Quaternion.identity);
                nItem.transform.parent = Note.transform;
                nItem.transform.localPosition = localPos;
                //nItem.transform.localScale = new Vector3(300, 300, 300);
                RhythmJudge.InItemList.Add(nItem);
            }
        }
    }





}