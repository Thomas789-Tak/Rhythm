using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Button_Stage : MonoBehaviour
{
    private Stage stage;
    private StageDataContainer stageDataContainer;
    private LUILobby LUILobby;
    [SerializeField]
    private Button Button;
    public TextMeshProUGUI TextStageNum;
    public TextMeshProUGUI TextStageName;
    public TextMeshProUGUI TextTime;
    public TextMeshProUGUI TextScore;
    public Slider SliderAchievement;
    public Image ImageStage;
    public GameObject GoSelected;
    public GameObject GoLock;

    // Use this for initialization
    void Start()
    {
        LUILobby = LUILobby.Instance;
        Button = GetComponent<Button>();
        
    }

    public void SetStage(Stage stage)
    {

    }

    public void SetStage(StageDataContainer stage)
    {
        this.stageDataContainer = stage;

        this.TextStageNum.text = "STAGE " + stage.stageNum.ToString();

        this.TextStageName.text = stage.stageName;

        this.ImageStage.sprite = stage.ImageStage;

        SetTime(stage.time);
        
        this.TextScore.text = stage.score.ToString("N0");

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

}