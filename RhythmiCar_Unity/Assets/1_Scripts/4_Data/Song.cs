using System.Collections;
using UnityEngine;

[System.Serializable]
public class Song
{
    [SerializeField] private SoundManager.EBGM eBGM;
    
    [SerializeField] private string songName;
    [SerializeField] private string writerName;
    
    [SerializeField] private bool isOpend;
    [SerializeField] private int openCost;
    
    [SerializeField] private int bpm;
    [SerializeField] private int maxSpeed;
    

    public Song(SoundManager.EBGM eBGM, string songName, string writerName, bool isOpend, int openCost, int bpm, int maxSpeed)
    {
        this.eBGM = eBGM;
        this.songName = songName;
        this.writerName = writerName;
        this.isOpend = isOpend;
        this.openCost = openCost;
        this.bpm = bpm;
        this.maxSpeed = maxSpeed;
    }

    public SoundManager.EBGM EBGM { get => eBGM; private set => eBGM = value; }
    public string SongName { get => songName; private set => songName = value; }
    public string WriterName { get => writerName; private set => writerName = value; }
    public bool IsOpend { get => isOpend; private set => isOpend = value; }
    public int OpenCost { get => openCost; private set => openCost = value; }
    public int Bpm { get => bpm; private set => bpm = value; }
    public int MaxSpeed { get => maxSpeed; private set => maxSpeed = value; }
}