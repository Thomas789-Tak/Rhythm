using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : Singleton<MonoBehaviour>
{

    public Text TextNoteNum;
    private int noteNum = 0;
    public Text TextNoteJudge;

    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.touchEvent.AddListener((EJudge) => SetNoteNum(EJudge));
        InputManager.Instance.touchEvent.AddListener((EJudge) => SetNoteJudge(EJudge));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetNoteNum(EJudge eJudge)
    {
        if (eJudge == EJudge.Miss)
            noteNum = 0;
        else
            noteNum++;
        TextNoteNum.text = noteNum.ToString();
    }

    private void SetNoteJudge(EJudge eJudge)
    {
        TextNoteJudge.text = eJudge.ToString();
    }
}
