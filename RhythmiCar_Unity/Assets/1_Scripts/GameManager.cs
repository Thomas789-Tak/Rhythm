using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PathCreation;

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

    private static GameManager instance = null;
    public CarController car;



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

    public void GamePause()
    {
        Time.timeScale = 0.0001f;
        SoundManager.Instance.bgm.Pause();
    }

    public void GameResume()
    {
        Time.timeScale = 1f;
        SoundManager.Instance.bgm.Play();
    }
}
