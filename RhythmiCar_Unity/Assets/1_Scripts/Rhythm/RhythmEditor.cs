using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RhythmEditor : RhythmJudge
{
    public ERhythmEditorMode rhythmEditorMode;
    public enum ERhythmEditorMode
    { Play = 0, Record = 1 }

    public GameObject NoteObject;




    protected override void Start()
    {
        base.Start();

    }


    protected override void Update()
    {
        base.Update();



    }

    public override void Judge()
    {
        if (this.rhythmEditorMode == ERhythmEditorMode.Play)
        {
            base.Judge();
        }
        else
        {

        }
    }
}