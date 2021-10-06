using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Rhythm Editor UI Manager
/// </summary>
public class REUIManager : MonoBehaviour
{
    #region Path
    private string noteEditorNumberPath = "UI/NoteEditorNumer";
    #endregion

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateNoteEditorNumber(NoteEditor noteEditor, int number)
    {
        var noteEditorNumber = Resources.Load(noteEditorNumberPath) as GameObject;
        Debug.Log(noteEditorNumber);

        var newNoteEditorNumber = Instantiate(noteEditorNumber, this.transform);
        newNoteEditorNumber.GetComponent<NoteEditorNumber>()
            .SetNoteEditor(noteEditor, number);
    }
}