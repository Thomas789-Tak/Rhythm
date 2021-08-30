﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


/// <summary>
/// 음악 재생, 노트 생성, 노트 판정을 담당하는 스크립트
/// </summary>
public class RhythmJudge : MonoBehaviour
{
    //private InputManager InputManager;
    private AudioSource AudioSource;
    public SongNoteData CurrentSong;
    public UnityEvent<EJudge> JudgeEvent;

    public GameObject JudgeLine;
    public GameObject NoteObject;
    public List<GameObject> NotePool;

    [SerializeField]
    private bool isTestMode = false;
    public bool isSongStart = false;
    public int currentNoteNum = 0;
    public float songPlayTime = 0f;

    public float noteSpeed = 10;
    public float noteStartingX = 10;

    public float speedAccel = 1f;
    public float noteInstanceDistance = 10f;
    public float endJudgeTime = 0.2f;
    public float perfectJudgeTime = 0.1f;
    public float goodJudgeTime = 0.1f;
    public float badJudgeTime = 0.1f;

    // Use this for initialization
     protected virtual void Start()
    {
        // Audio Source 할당
        if (SoundManager.Instance)
            AudioSource = SoundManager.Instance.bgm;
        else
            AudioSource = GetComponent<AudioSource>() ?
                GetComponent<AudioSource>() : gameObject.AddComponent<AudioSource>();

        isSongStart = false;
        currentNoteNum = 0;
        songPlayTime = 0f;

        ObjectPooling();

        if (!isTestMode)
            SongStart();

    }

    protected virtual void Update()
    {
        TimeCheck();
        MissCheck();
        //CreateNote();
    }

    /// <summary>
    /// 현재 노래 데이터 설정
    /// </summary>
    /// <param name="songNoteData"></param>
    public void SetSongNoteData(SongNoteData songNoteData)
    {
        this.CurrentSong = songNoteData;
    }

    /// <summary>
    /// 노래 시작.
    /// SoundManager 의 음악 재생.
    /// InputManager 의 Event 등록.
    /// </summary>
    [ContextMenu("GameStart")]
    public void SongStart()
    {
        this.isSongStart = true;

        //this.AudioSource.clip = CurrentSong.info.clip;
        //this.AudioSource.Play();
        SoundManager.Instance.PlayBGM(this.CurrentSong.EBGM);

        this.JudgeEvent.AddListener((judge) => InputManager.Instance.touchEvent.Invoke(judge));
        this.JudgeEvent.AddListener((judge) => this.DeleteNote());

    }

    /// <summary>
    /// 노래가 시작되고 난 후 매 프레임마다 호출됨.
    /// 현재 재생 시간을 측정한다.
    /// </summary>
    private void TimeCheck()
    {
        if (!isSongStart) return;

        //songPlayTime += Time.deltaTime;
        songPlayTime = AudioSource.time;
    }

    /// <summary>
    /// 노래가 시작되고 난 후 매 프레임마다 호출됨.
    /// 가장 최근의 노트를 계속 감시하며, 해당 노트가 Miss 판정 범위인지를 검사한다.
    /// </summary>
    private void MissCheck()
    {
        if (!isSongStart) return;

        // 한 노트가 판정선 너머로 가버렸을 경우 (판정 Miss 의 경우)
        float nTime = CurrentSong.noteData[currentNoteNum];
        if (nTime + endJudgeTime < songPlayTime)
        {
            //Miss 판정 처리
            JudgeEvent.Invoke(EJudge.Miss);
            //currentNoteNum++;
        }
    }

    /// <summary>
    /// 유저 입력 감지시 호출됨
    /// 가장 최근 노트를 감시하여 판정 검사를 진행한다.
    /// </summary>
    public virtual void Judge()
    {
        if (!isSongStart) return;

        float nTime = CurrentSong.noteData[currentNoteNum];
        EJudge judge;

        if ((nTime -= perfectJudgeTime) < songPlayTime)
        {
            judge = EJudge.Perfect;
        }
        else if ((nTime -= goodJudgeTime) < songPlayTime)
        {
            judge = EJudge.Good;
        }
        else if ((nTime -= badJudgeTime) < songPlayTime)
        {
            judge = EJudge.Bad;
        }
        else
        {
            return;
        }

        //currentNoteNum++;
        JudgeEvent.Invoke(judge);
    }

    /// <summary>
    /// 노트들을 Current Song에 맞게 생성해둔다.
    /// </summary>
    private void ObjectPooling()
    {
        int pNum = 300;
        NotePool = new List<GameObject>();

        for (int i = 0; i < pNum; i++)
        {
            if (CurrentSong.noteData.Count <= i)
                break;

            var note = Instantiate(NoteObject, JudgeLine.transform).GetComponent<Note>();

            note.isSet = true;
            note.RhythmJudge = this;
            note.num = i;
            note.speed = noteSpeed;
            note.time = CurrentSong.noteData[i];
            //note.transform.localPosition =
            //    new Vector3(0, CurrentSong.noteData[i] * noteSpeed);

            //note.gameObject.SetActive(false);
            NotePool.Add(note.gameObject);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void CreateNote()
    {

        foreach(GameObject note in NotePool)
        {
            if (note.activeInHierarchy)
                continue;

            //note.transform.localPosition = 

        }
    }

    /// <summary>
    /// 맞추거나 놓친 노트를 비활성화 해둔다.
    /// </summary>
    /// <param name="judge"></param>
    private void DeleteNote()
    {
        NotePool[currentNoteNum].SetActive(false);
        currentNoteNum++;
    }







}