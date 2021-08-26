using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RhythmEditorTest : MonoBehaviour
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

    public SongDataContainer SongDataContainer;



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
        this.isTestMode = true;
        testStartTime = Time.time;
        AudioSource.clip = currentSongInfortmationData.clip;
        AudioSource.Play();
    }

    private void StartTestReal()
    {

    }

    public void StopTest()
    {
        this.isTestMode = false;
        AudioSource.Stop();
    }

    public void SaveTest()
    {
        var songNoteData = currentSongNoteData;

        songNoteData.SetSongInformaitonData(this.currentSongInfortmationData);
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