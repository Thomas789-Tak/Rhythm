using UnityEngine;

[CreateAssetMenu(fileName = "Song_Information_Data", menuName = "Scriptable Object/Song Information Data")]
[System.Serializable]
public class SongInformationData : ScriptableObject
{
    public string title;
    public string artist;
    public string BPM;
    public AudioClip clip;
}