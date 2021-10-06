using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class RhythmEditor : MonoBehaviour
{
    //private InputManager InputManager;
    private REUIManager REUIManager;
    protected AudioSource AudioSource;
    //public SongNoteData CurrentSong;
    //public UnityEvent<EJudge> JudgeEvent;

    //public GameObject JudgeLine;
    //public GameObject NoteObject;
    //public List<GameObject> NotePool;

    //[SerializeField]
    //protected bool isTestMode = false;
    //public bool isSongStart = false;
    //public int currentNoteNum = 0;
    //public float songPlayTime = 0f;

    //public float noteSpeed = 10;
    //public float noteStartingX = 10;

    //public float speedAccel = 1f;
    //public float noteInstanceDistance = 10f;
    //public float endJudgeTime = 0.2f;
    //public float perfectJudgeTime = 0.1f;
    //public float goodJudgeTime = 0.1f;
    //public float badJudgeTime = 0.1f;

    enum ERhythmEditorMode
    { Stop = 0, Start = 1, Pause = 2 }
    private ERhythmEditorMode editorMode;

    public Slider SliderSongTimeBar;
    private bool isHanded;   // 슬라이더에 인풋을 받았는가?
    public InputField InputBPM;
    public InputField InputMutiple;

    public GameObject NoteEditorParent;
    public GameObject NoteEditor;
    public List<GameObject> ItemList;
    private List<GameObject> InNoteList = new List<GameObject>();
    private List<GameObject> InItemList = new List<GameObject>();

    [Header("리듬 노트 리스트에 들어갈 변수들")]
    public float firstNoteJudgeTime;
    public float noteJudgeIntervalTime;
    public int totalNoteNum;
    public float noteLocalPositionMutiple;
    

    private void Start()
    {
        REUIManager = FindObjectOfType<REUIManager>();
        AudioSource = GetComponent<AudioSource>();
        editorMode = ERhythmEditorMode.Stop;
        isHanded = false;
        //CreateNoteEditor();
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
        float mouseInput = Input.GetAxis("Mouse ScrollWheel");
        float newSongTime;
        float weight = 4f;

        if (mouseInput != 0)
        {
            newSongTime = AudioSource.clip.length * perLenght + mouseInput;
            newSongTime *= weight;
            ControlSongTime(newSongTime);
        }

        // Slider 가 조작되었을 경우.
        if (isHanded)
        {
            newSongTime = SliderSongTimeBar.value * AudioSource.clip.length;
            newSongTime *= weight;
            ControlSongTime(newSongTime);
        }
        else
        {
            SliderSongTimeBar.value = perLenght;
        }

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

        loPos.z = -AudioSource.time * noteLocalPositionMutiple;

        NoteEditorParent.transform.localPosition = loPos;
    }

    /// <summary>
    /// Note Editor (박자 체크용 오브젝트) 들을 BPM과 노래 시간에 맞게 생성한다.
    /// </summary>
    public void CreateNoteEditor()
    {
        int secondPerMinute = 60;
        //int BPM = 90;
        int BPM = int.Parse(InputBPM.text);
        float BPS = BPM / secondPerMinute;
        float songLength = AudioSource.clip != null ? AudioSource.clip.length : 60;
        int totalNoteNum = Mathf.CeilToInt(songLength * BPS);

        float FirstNoteTime = 2.0075f;
        //float NoteInterval = 2.6666f;
        float NoteMultiple = float.Parse(InputMutiple.text);
        float NoteInterval = ((float)secondPerMinute / BPM) / NoteMultiple;

        if (InNoteList.Count != 0) InNoteList.ForEach(x =>
        {
            Debug.Log(InNoteList.Count);
            Destroy(x.gameObject);
        });

        InNoteList.Clear();

        for (int i = 0; i < totalNoteNum * NoteMultiple; i++)
        {
            var noteEditor = Instantiate(NoteEditor, NoteEditorParent.transform);
            var loPos = noteEditor.transform.localPosition;
            loPos.z = (FirstNoteTime + NoteInterval * i) * noteLocalPositionMutiple;
            noteEditor.transform.localPosition = loPos;

            REUIManager.CreateNoteEditorNumber(noteEditor.GetComponent<NoteEditor>(), i);

            InNoteList.Add(noteEditor);
        }
    }

    public void CreateItem()
    {
        if (InNoteList == null)
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

        int rowPerDifficult = 6;
        int rowPerStage = 3 * rowPerDifficult;

        //int itemInterval = (int)stageInfo[currentStage]["itemInterval"];
        //int itemStartPos = (int)stageInfo[currentStage]["itemStartPos"];
        int roadCount = (int)stageInfo[currentStage]["road"];   // 도로의 수
        int itemLength = (int)stageInfo[currentStage]["itemLength"]; // 한 도로에 배치된 아이템의 총 수

        if (InItemList.Count != 0)
        {
            InItemList.ForEach(x => Destroy(x));
        }

        InItemList.Clear();

        for (int i = 0; i < InNoteList.Count; i++)
        {
            if (itemLength < i) break;

            GameObject Note = InNoteList[i];
            for (int j = 0; j < roadCount; j++)
            {
                int id = (currentStage * rowPerStage) + (currentDifficult * rowPerDifficult) + j;
                GameObject item = ItemList[(int)spwanData[currentStage * 6 + j][i + "m"]];
                Vector3 pos = new Vector3(-4 + 2 * j, 2f, Note.transform.position.z);

                var nItem = Instantiate(item, this.NoteEditorParent.transform);
                nItem.transform.position = pos;
                InItemList.Add(nItem);
            }
        }
    }

    //[ContextMenu("Make Note Editor")]
    //public void MakeNoteList()
    //{
    //    List<float> noteList = new List<float>();

    //    noteList.Add(firstNoteJudgeTime);

    //    for (int i = 1; i < totalNoteNum; i++)
    //    {
    //        var noteTime = firstNoteJudgeTime + noteJudgeIntervalTime * i;
    //        noteList.Add(noteTime);
    //    }

    //    this.CurrentSong.noteData = noteList;
    //}

}