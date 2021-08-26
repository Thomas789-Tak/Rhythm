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
    public SongInformationData info;
    public ESongDifficulty difficulty;
    public float speedAccel;
    public bool isOpend = false;
    public List<float> noteData;
}

[System.Serializable]
public class SongInformationData
{
    public string title;
    public string artist;
    public string BPM;
    public AudioClip clip;
}

public enum ESongDifficulty
{
    Easy = 0,
    Normal,
    Hard
}
