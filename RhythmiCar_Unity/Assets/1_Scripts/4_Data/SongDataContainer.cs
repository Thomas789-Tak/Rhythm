using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "SongDataContainer", menuName = "Scriptable Object/Song Data Container")]
public class SongDataContainer : ScriptableObject
{
    public List<SongInformationData> songInformationDatas = new List<SongInformationData>();
    public List<SongNoteData> songNoteDatas = new List<SongNoteData>();

}


[System.Serializable]
public class SongNoteData
{
    public string name;
    [SerializeField]
    private SongInformationData info;
    public ESongDifficulty difficulty;
    public float speedAccel;
    public bool isOpend = false;
    public List<float> noteData;
    public List<EnumItemVO.EItemType> line = new List<EnumItemVO.EItemType>();
    [SerializeField]
    public List<List<EnumItemVO.EItemType>> lines = new List<List<EnumItemVO.EItemType>>();

    /// <summary>
    /// Line 안에 담긴 정보를 Debug.log를 통해 본다.
    /// </summary>
    [ContextMenu("LogLine")]
    public void LogLine()
    {
        string txt = "";

        foreach (List<EnumItemVO.EItemType> line in lines)
        {
            foreach (EnumItemVO.EItemType item in line)
            {
                txt += item.ToString() + " ";
            }
            txt += "\n";
        }

        Debug.Log(txt);
    }

    [ContextMenu("AddItemToLines")]
    public void AddItemToLines()
    {
        lines.Add(line);
    }

    public string title { get =>  info.title; }
    public string artist { get => info.artist; }
    public int BPM { get => info.BPM; } 
    public AudioClip clip { get => info.clip; }
    public SoundManager.EBGM EBGM { get => info.EBGM; }
    public void SetSongInformaitonData(SongInformationData songInformationData)
    {
        info = songInformationData;
    }

}

[System.Serializable]
public class SongInformationData
{
    public string title;
    public string artist;
    public int BPM;
    public AudioClip clip;
    public SoundManager.EBGM EBGM;
}

public enum ESongDifficulty
{
    Easy = 0,
    Normal,
    Hard
}