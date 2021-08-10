using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager instance =null;
    public Text text;
    public CarController car;
    public Button Retry;
    public int myNum = 1;

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

        Retry.gameObject.SetActive(false);
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


    public void RetryGame()
    {
        SceneManager.LoadScene(0);
    }
    public void ToMainScene()
    {
        SceneManager.LoadScene(1);
    }
}
