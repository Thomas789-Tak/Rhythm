using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int Gold
    {
        get => gold; set
        {
            gold = value;
            //UIManager.Instance.SetGold(gold);
        }
    }
    public int Star
    {
        get => star; set
        {
            star = value;
            //UIManager.Instance.SetStar(star);
        }
    }
    public int Note
    {
        get => note; set
        {
            note = value;
            //UIManager.Instance.SetNote(note);
        }
    }
    public int Score
    {
        get => score; set
        {
            score = value;
            UIManager.Instance.SetScore(score);
        }
    }
    public float GameTime
    {
        get
        {
            return gameTime;
        }

        set
        {
            gameTime = value;
            UIManager.Instance.SetTime(value);
        }
    }
    public float Achievement { get => achievement; set => achievement = value; }

    private int gold;
    private int star;
    private int note;
    private int score;
    private float gameTime;
    private float achievement;

    private int stageNum;
    private int carNum;

    private static GameManager instance = null;
    [SerializeField] GameObject[] Cars;

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
        Application.targetFrameRate = 60;
        Cars[(int)ESelectCar.GreenCar].SetActive(true);
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
        GameTime = Time.time;
        print("°ñµå·®" + Gold);
    }
    public void RetryGame()
    {
        LoadingSceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMainScene()
    {
        LoadingSceneManager.LoadScene("LobbyScene");
    }

    [ContextMenu("Send")]
    public void SendResult()
    {
        var gameResult = new GameResult.GameResultData
        {
            gold = this.gold,
            score = this.score,
            iNote = this.note,
            star = this.star,
            time = this.gameTime,
            achievement = this.achievement,
            stage = this.stageNum,
        };

        GameResult.SetResult(gameResult);
    }

    public void GamePause()
    {
        Time.timeScale = 0.000001f;
        SoundManager.Instance.bgm.Pause();
    }

    public void GameResume()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.bgm.Play();
    }
}
