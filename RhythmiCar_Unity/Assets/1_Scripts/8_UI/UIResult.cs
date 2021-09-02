using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResult : MonoBehaviour, IUISetting
{
    private SoundManager SoundManager;
    private GameManager GameManager;
    private UIManager UIManager;

    public GameObject TitleClear;
    public GameObject TitleLose;
    public TextMeshProUGUI TextTime;
    public TextMeshProUGUI TextScore;
    public TextMeshProUGUI TextStage;
    public TextMeshProUGUI TextGold;
    public TextMeshProUGUI TextINote;
    public int StarCount;
    public Slider SliderAchievement;

    public void SetUI()
    {
        Debug.Log(this + " Start");
        this.UIManager = UIManager.Instance;
        this.GameManager = GameManager.Instance;
        this.SoundManager = SoundManager.Instance;
    }

    [ContextMenu("OnUI")]
    public void OnUI()
    {
        this.GameManager.GamePause();
        this.gameObject.SetActive(true);
        this.SetTime(GameManager.GameTime);
        this.TextScore.text = GameManager.Score.ToString();
        //this.TextStage.text = "WoolTaRi";
        this.TextGold.text = GameManager.Gold.ToString();
        this.TextINote.text = GameManager.Note.ToString();
        this.SliderAchievement.value = GameManager.Achievement;
    }
    public void SetTime(float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        int point = (int)(time * 100) % 100;

        TextTime.text =
            min.ToString("00") + ":" +
            sec.ToString("00") + ":" +
            point.ToString("00");
    }

    public void OffUI()
    {
        this.GameManager.GameResume();
        this.gameObject.SetActive(false);
    }

}
