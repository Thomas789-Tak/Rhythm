using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PathCreation;

public class GameManager : MonoBehaviour
{
    public int gold { get; set; }
    public int star { get; set; }
    public int note { get; set; }
    public int score { get; set; }

    private static GameManager instance =null;
    public Text text;
    public CarController car;
    public Button RetryButton;
    public Image BoosterGauge;
    public Slider RhythmPower;

    private void Awake()
    {
        Time.timeScale = 1;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        RetryButton.gameObject.SetActive(false);
        Application.targetFrameRate = 60;
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            return instance;
        }
    }

    private void Update()
    {

        print("°ñµå·®"+gold);
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ToMainScene()
    {
        SceneManager.LoadScene(0);
    }    

    [ContextMenu("Send")]
    public void SendResult()
    {
         // VO.gold = gold
         // VO.star = star
    }
}
