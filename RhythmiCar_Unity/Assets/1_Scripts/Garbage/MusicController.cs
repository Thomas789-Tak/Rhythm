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
        Invoke("DelayStart",2f);
    }

    void DelayStart()
    {
        isTrack1Playing = true;
    }
    private void FixedUpdate()
    {

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

        speed.text = player.myCurrentSpeed.ToString("N0") + "Km/h" + "  현재기어";
    }
    
    public void OnTouchKickDown()
    {
        isKickButtonDown = true;
    }
    public void OnTouchKickUp()
    {
        isKickButtonDown = false;
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
