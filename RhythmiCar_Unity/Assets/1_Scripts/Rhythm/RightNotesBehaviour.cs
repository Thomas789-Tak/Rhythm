using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class RightNotesBehaviour : MonoBehaviour // ���� ���ӿ� ������ �ִ� ��Ʈ
{
    public float noteSpeed;
    bool isJudgeAble;
    private void OnEnable()
    {
        noteSpeed = NoteManager.Instance.bpm * 1.5f;
        isJudgeAble = false;
    }


    void Update()
    {
        transform.localPosition += noteSpeed * Time.deltaTime * -Vector3.right;
        if (transform.localPosition.x <= 66) // Miss �������� ���� �� ���� ó��
        {
            ObjectPooler.instance.RightNoteQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
        else if (transform.localPosition.x > 66 && transform.localPosition.x <= 141) // Success ���� ó�� ����
        {
            if (NoteManager.Instance.isStart == false)
            {
                SoundManager.Instance.PlayBGM(SoundManager.EBGM._196BPM_SuddenShower); // musiccontrol���� �÷�������
                NoteManager.Instance.isStart = true;
            }
            isJudgeAble = true;
            if (isJudgeAble && Input.GetKeyDown(KeyCode.Space)||InputController.Instance.isTouch)
            {
                print("����");
                GameManager.Instance.SpeedUp();
                CarController.SuccessVFx.Play();
                ObjectPooler.instance.RightNoteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
                InputController.Instance.isTouch = false;
            }
        }


    }
}
