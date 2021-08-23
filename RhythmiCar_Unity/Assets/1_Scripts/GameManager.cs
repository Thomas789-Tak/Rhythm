using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PathCreation;

public class GameManager : MonoBehaviour
{

    private static GameManager instance =null;
    public Text text;
    public CarController car;
    public Button RetryButton;
    public Button KickButton;
    public Button BoosterButton;
    public Image BoosterGauge;
    public Slider RhythmPower;
    public Scrollbar WheelScrollBar;
    public int myNum = 1;
    public int noteCount;
    public PathCreator[] road;
    public bool isTouchWheelUI;
    public bool isTouchKickUI;
    private void Awake()
    {
        text.text = (road[0].path.length/1.6f).ToString();
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
        text.text = car.MyRigidBody.velocity.magnitude.ToString("N0")+"Km/h";
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ToMainScene()
    {
        SceneManager.LoadScene(0);
    }
    public void WheelTouchDown()
    {
        isTouchWheelUI = true;
    }
    public void WheelTouchUp()
    {
        isTouchWheelUI = false;
    }
    public void KickTouchDown()
    {
        isTouchKickUI = true;
    }
    public void KickTouchUp()
    {
        isTouchKickUI = false;
    }
    public void SpeedUp()
    {
        car.SpeedUP();
    }
    public void BoosterTouchDown()
    {        
        car.isBoosting = true;
    }
}
