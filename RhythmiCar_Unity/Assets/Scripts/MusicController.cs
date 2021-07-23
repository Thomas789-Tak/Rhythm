using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour // 노래를 판정하고 조절하는 클래스
{
    CarController player;
    public Text speed;
    bool isTrack1Playing;
    bool isTrack2Playing;
    public bool isKickButtonDown;
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<CarController>();
        Application.targetFrameRate = 60;
        Invoke("DelayStart",2f);
    }

    void DelayStart()
    {
        isTrack1Playing = true;
    }
    private void FixedUpdate()
    {
        if (isTrack1Playing&&player.isGearChanging==false)
        {
            MusicManager.Instance.PlayWakeUp();

        }
        else if(isTrack2Playing && player.isGearChanging == false)
        {
            MusicManager.Instance.PlayIfI();

        }
    }

    public void GearUp(int gear)
    {
        MusicManager.Instance.PauseWakeUp();
        isTrack1Playing = false;
        isTrack2Playing = true;

    }

    public void GearDown(int gear)
    {
        if(gear.Equals(2))
        {
            MusicManager.Instance.PauseIfI();
            isTrack2Playing = false;
            isTrack1Playing = true;
        }

    }

    private void Update()
    {
        if (player.isGearReady)
        {
            speed.color = Color.green;
        }
        else
        {
            speed.color = Color.white;
        }
        if(player.myCurrentGear==1)
        {
            player.maxSpeed = 80;
        }
        else if(player.myCurrentGear==2)
        {
            player.maxSpeed = 120;
        }
        else
        {
            player.maxSpeed = 160;
        }
        speed.text = player.myCurrentSpeed.ToString("N0")+"Km/h" + "  현재기어"+player.myCurrentGear;
    }
    
    public void OnTouchKickDown()
    {
        isKickButtonDown = true;
    }
    public void OnTouchKickUp()
    {
        isKickButtonDown = false;
        player.driftTime = 0;
    }
    // 노트 판정
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (isKickButtonDown)
            {
                player.GetSpeedUp();
                collision.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if (collision.gameObject.activeSelf)
            {
                player.GetSpeedDown();
                collision.gameObject.SetActive(false);
                MusicManager.Instance.Error.Play();
            }
        }
    }
}
