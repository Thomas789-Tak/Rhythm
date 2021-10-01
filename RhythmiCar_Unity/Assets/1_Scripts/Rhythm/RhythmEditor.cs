using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class RhythmEditor : MonoBehaviour
{
    //private InputManager InputManager;
    protected AudioSource AudioSource;
    public SongNoteData CurrentSong;
    public UnityEvent<EJudge> JudgeEvent;

    public GameObject JudgeLine;
    public GameObject NoteObject;
    public List<GameObject> NotePool;

    enum ERhythmEditorMode
    { Stop = 0, Start = 1, Pause = 2 }
    private ERhythmEditorMode editorMode;

    [SerializeField]
    protected bool isTestMode = false;
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

    public Slider SliderSongTimeBar;
    private bool isHanded;   // 슬라이더에 인풋을 받았는가?

    public GameObject NoteEditorParent;
    public GameObject NoteEditor;

    [Header("리듬 노트 리스트에 들어갈 변수들")]
    public float firstNoteJudgeTime;
    public float noteJudgeIntervalTime;
    public int totalNoteNum;

    private void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        editorMode = ERhythmEditorMode.Stop;
        isHanded = false;
        CreateNoteEditor();
    }

    private void Update()
    {
        if (editorMode == ERhythmEditorMode.Start)
        {
            ControlSong();
            OnClick();
            SetNoteEditorParentPosition();
        }
    }

    public void SongStart()
    {
        // 노래가 이미 실행 중이라면 함수를 종료한다.
        if (editorMode == ERhythmEditorMode.Start) return;

        if (AudioSource.clip)
        {
            AudioSource.Play();
            editorMode = ERhythmEditorMode.Start;
        }
    }

    public void SongStop()
    {
        // 노래가 이미 멈춘 상태라면 함수를 종료한다.
        if (editorMode == ERhythmEditorMode.Stop) return;

        AudioSource.Stop();
        editorMode = ERhythmEditorMode.Stop;
    }

    public void SongPause()
    {
        // 노래가 이미 일시정지된 상태라면 함수를 종료한다.
        if (editorMode == ERhythmEditorMode.Pause) return;

        AudioSource.Pause();
        editorMode = ERhythmEditorMode.Pause;
    }

    public void ControlSong()
    {
        float perLenght = AudioSource.time / AudioSource.clip.length;
        
        // 마우스 휠 인풋
        float va = Input.GetAxis("Mouse ScrollWheel");
        if (va != 0)
        {
            float a = AudioSource.clip.length * perLenght + va;
            ControlSongTime(a);
        }

        // Slider 가 조작되었을 경우.
        if (isHanded)
        {
            float b = SliderSongTimeBar.value * AudioSource.clip.length;
            ControlSongTime(b);
        }
        else
        {
            SliderSongTimeBar.value = perLenght;
        }

        //Debug.Log(perLenght);
        // 슬라이더 인풋 초기화
        isHanded = false;
    }

    public void ControlSongTime(float time)
    {
        if (time < 0) time = 0;
        else if (time > AudioSource.clip.length) time = AudioSource.clip.length;

        AudioSource.time = time;
    }

    public void Handling()
    {
        isHanded = true;
    }

    /// <summary>
    /// 마우스로 Note Editor 를 클릭 하였을 경우.
    /// </summary>
    public void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                var NoteEditor = hit.transform.GetComponent<NoteEditor>();
                if (NoteEditor) NoteEditor.Check();
            }
        }
    }

    /// <summary>
    /// 노트 에디터 Parent 의 현재 위치를 결정한다.
    /// </summary>
    public void SetNoteEditorParentPosition()
    {
        var loPos = NoteEditorParent.transform.localPosition;

        loPos.z = -AudioSource.time;

        NoteEditorParent.transform.localPosition = loPos;
    }

    /// <summary>
    /// Note Editor (박자 체크용 오브젝트) 들을 BPM과 노래 시간에 맞게 생성한다.
    /// </summary>
    public void CreateNoteEditor()
    {
        int secondPerMinute = 60;
        int BPM = 60;
        float BPS = BPM / secondPerMinute;
        float songLength = AudioSource.clip != null ? AudioSource.clip.length : 60;
        int totalNoteNum = Mathf.CeilToInt(songLength * BPS);

        float FirstNoteTime = 2.0075f;
        float NoteInterval = 2.6666f;

        for(int i = 0; i < totalNoteNum; i++)
        {
            var noteEditor = Instantiate(NoteEditor, NoteEditorParent.transform);
            var loPos = noteEditor.transform.localPosition;
            loPos.z = FirstNoteTime + NoteInterval * i;
            noteEditor.transform.localPosition = loPos;
        }
    }

    [ContextMenu("Make Note Editor")]
    public void MakeNoteList()
    {
        List<float> noteList = new List<float>();

        noteList.Add(firstNoteJudgeTime);

        for (int i = 1; i < totalNoteNum; i++)
        {
            var noteTime = firstNoteJudgeTime + noteJudgeIntervalTime * i;
            noteList.Add(noteTime);
        }

        this.CurrentSong.noteData = noteList;
    }

}