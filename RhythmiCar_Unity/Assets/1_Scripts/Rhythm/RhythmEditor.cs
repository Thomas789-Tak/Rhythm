using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RhythmEditor : RhythmJudge
{

    public Slider SliderSongTimeBar;
    private bool isHanded;   // 슬라이더에 인풋을 받았는가?

    public GameObject NoteEditorParent;
    public GameObject NoteEditor;

    public float firstNoteJudgeTime;
    public float noteJudgeIntervalTime;
    public int totalNoteNum;

    protected override void Start()
    {
        base.Start();
        isHanded = false;
        CreateNoteEditor();
    }


    protected override void Update()
    {
        base.Update();
        if (isSongStart)
        {
            ControlSong();
            OnClick();
            SetNoteEditorPosition();
        }
    }

    public void TestStart()
    {
        isTestMode = true;
        base.SongStart();
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


    public void OnClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray.origin, ray.direction, out hit))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }

    public void SetNoteEditorPosition()
    {
        var loPos = NoteEditorParent.transform.localPosition;

        loPos.z = - noteSpeed * AudioSource.time;

        NoteEditorParent.transform.localPosition = loPos;
    }

    public void CreateNoteEditor()
    {
        int num;
        if (AudioSource.clip)
            num = (int)(AudioSource.clip.length * 10);
        else
            num = 200;

        for(int i = 0; i < num; i++)
        {
            var go = Instantiate(NoteEditor, NoteEditorParent.transform);
            var loPos = go.transform.localPosition;
            loPos.z = i * noteSpeed / 10;
            go.transform.localPosition = loPos;
        }
    }

    public void MakeNoteList()
    {
        List<float> noteList = new List<float>();

        noteList.Add(firstNoteJudgeTime);

        for (int i = 1; i < totalNoteNum; i++)
        {
            var noteTime = firstNoteJudgeTime + noteJudgeIntervalTime * i;
            noteList.Add(noteTime);
        }
    }
}