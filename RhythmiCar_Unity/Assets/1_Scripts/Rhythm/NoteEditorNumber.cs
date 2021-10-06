using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoteEditorNumber : MonoBehaviour
{
    public NoteEditor NoteEditor;
    //private bool isCashed = false;
    public Text TextNoteNumber;

    private void Update()
    {
        //if (isCashed)
        //{
        //    if (NoteEditor)
        //    {
        //        transform.position = 
        //            Camera.main.WorldToScreenPoint(NoteEditor.transform.position);
        //    }
        //    else
        //    {
        //        Destroy(TextNoteNumber.transform);
        //        Destroy(this.transform);
        //    }
        //}

        if (NoteEditor)
        {
            transform.position =
                Camera.main.WorldToScreenPoint(NoteEditor.transform.position);
        }
        else
        {
            Destroy(TextNoteNumber.transform);
            Destroy(this.transform);
        }

    }

    public void SetNoteEditor(NoteEditor noteEditor, int number)
    {
        this.NoteEditor = noteEditor;
        TextNoteNumber.text = number.ToString();
        //isCashed = true;
    }
}