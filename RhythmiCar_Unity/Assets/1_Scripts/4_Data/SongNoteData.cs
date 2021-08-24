using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "Song_Note_Data", menuName = "Scriptable Object/Song Note Data")]
[ExecuteInEditMode]

public class SongNoteData : ScriptableObject
{
    public SongInformationData info;
    public ESongDifficulty difficulty;
    public float speedAccel;
    public bool isOpend = false;
    public List<float> noteData;
}

public enum ESongDifficulty
{
    Easy = 0,
    Normal,
    Hard
}