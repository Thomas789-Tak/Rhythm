using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RhythmManager : MonoBehaviour
{
    private AudioSource AudioSource;
    private bool isTestMode = false;
    private float testStartTime;

    public float metre;
    public ESongDifficulty difficulty;
    public float speedAccel;
    public SongInformationData currentSongInfortmationData;
    public List<float> noteData = new List<float>();
    public SongNoteData currentSongNoteData;

    [SerializeField]
    public List<SongInformationData> songInformationDatas = new List<SongInformationData>();
    public List<SongNoteData> songNoteDatas = new List<SongNoteData>();

    private void Awake()
    {

    }

   // Start is called before the first frame update
    void Start()
    {
        isTestMode = false;
        this.AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        KeyInput();
        Metronome();
    }

    public void KeyInput()
    {
        if (isTestMode)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                float currentTime = Time.time - testStartTime;
                Debug.Log(noteData.Count + " " + currentTime);
                noteData.Add(currentTime);
            }
        }
    }

    public void Metronome()
    {

    }
    
    public void StartTest()
    {
        if (metre == 0f)
        {
            Debug.LogError("Input Metre");
            return;
        }
        AudioSource.clip = currentSongInfortmationData.clip;
        Invoke(nameof(StartTestReal), 1f);
    }

    private void StartTestReal()
    {
        this.isTestMode = true;
        AudioSource.Play();
    }

    public void StopTest()
    {
        this.isTestMode = false;
        AudioSource.Stop();
    }

    public void SaveTest()
    {
        var songNoteData = currentSongNoteData;

        songNoteData.info = this.currentSongInfortmationData;
        songNoteData.difficulty = this.difficulty;
        songNoteData.speedAccel = this.speedAccel;
        songNoteData.noteData = this.noteData;

    }

    public void LoadTest()
    {
        SongNoteData rd;

        string title = "a";
        string artist = "허신행";
        var dif = ESongDifficulty.Easy;

        string rdName = title + "_" + dif;

        string path = Application.dataPath + "/" + rdName;


        Debug.Log(path);

        if (File.Exists(path))
        {
            Debug.Log("불러오기 성공");
        }
        else
        {
            
        }
    }

    public void SelectSongNoteData()
    {

    }
}