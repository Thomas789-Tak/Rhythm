using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : Singleton<UIManager>
{
    public Text TextScore;
    public Text TextGold;
    public Text TextINote;
    public Text TextStar;

    public Text TextNoteNum;
    public Text TextNoteJudge;
    private int noteNum = 0;

    public Text TextGear;
    public Text TextSpeed;
    private int gear;
    private int speed;





    // Start is called before the first frame update
    void Start()
    {
        InputManager.Instance.touchEvent.AddListener((EJudge) => SetNoteNum(EJudge));
        InputManager.Instance.touchEvent.AddListener((EJudge) => SetNoteJudge(EJudge));

        TextScore.text = 0.ToString();
        TextGold.text = 0.ToString();
        TextINote.text = 0.ToString();
        TextStar.text = 0.ToString();

        TextGear.text = 0.ToString();
        TextSpeed.text = 0.ToString();

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

    public void SetCarInformation(float speed, int gear)
    {
        this.TextSpeed.text = speed.ToString();
        this.TextGear.text = gear.ToString();
    }

    public void SetGold(int gold)
    {
        this.TextGold.text = gold.ToString();
    }

    public void SetStar(int star)
    {
        this.TextStar.text = star.ToString();
    }

    public void SetNote(int note)
    {
        this.TextINote.text = note.ToString();
    }

    public void SetScore(int score)
    {
        this.TextScore.text = score.ToString();
    }

}
