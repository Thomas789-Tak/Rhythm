using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingFalse : MonoBehaviour // Ÿ�̹��� ������ �� �ۿ��ϴ� Ŭ����
{
    CarController player;
    public MusicController MusicCon;
    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<CarController>();
        MusicCon = FindObjectOfType<MusicController>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Note"))
        {
            if(MusicCon.isKickButtonDown)
            {
                collision.gameObject.SetActive(false);
                player.GetSpeedDown();
                MusicManager.Instance.Error.Play();
            }
        }
    }
}
