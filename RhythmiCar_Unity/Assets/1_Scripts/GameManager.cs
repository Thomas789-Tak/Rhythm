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
    public Button Retry;
    public int myNum = 1;
    public int noteCount;
    public PathCreator[] road;

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

    private void Update()
    {
        text.text=car.MyRigidBody.velocity.magnitude.ToString("N0")+"차량속도" + "currentSpeed==>" + car.myCurrentSpeed.ToString("N0");
    }
    public void RetryGame()
    {
        SceneManager.LoadScene(1);
    }
    public void ToMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
