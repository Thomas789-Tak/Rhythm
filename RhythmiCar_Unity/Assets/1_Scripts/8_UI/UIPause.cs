using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIPause : MonoBehaviour
{
    private SoundManager SoundManager;
    private GameManager GameManager;
    private UIManager UIManager;

    public TextMeshProUGUI TextStage;
    public TextMeshProUGUI TextGold;
    public TextMeshProUGUI TextINote;
    public int StarCount;

    public Slider SliderBGM;
    public Slider SliderSFX;
    public Toggle ToggleVibration;
    public Toggle ToggleShake;

    void Start()
    {
        UIManager = UIManager.Instance;
        GameManager = GameManager.Instance;
        SoundManager = SoundManager.Instance;
    }

    [ContextMenu("OnUI")]
    public void OnUI()
    {
        this.gameObject.SetActive(true);
        this.TextGold.text = GameManager.Gold.ToString();
        this.TextINote.text = GameManager.Note.ToString();
        this.TextStage.text = "WoolTaRi";
        this.StarCount = GameManager.Star;
    }

    public void OffUI()
    {
        this.gameObject.SetActive(false);
    }

    public void SetBGM()
    {
        SoundManager.bgm.volume = SliderBGM.value;
    }

    public void SetSFX()
    {
        SoundManager.sfx.volume = SliderSFX.value;
        SoundManager.sfx_Loop.volume = SliderSFX.value;
    }

}
