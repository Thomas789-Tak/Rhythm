using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

public class RightNotesBehaviour : MonoBehaviour // 실제 게임에 영향을 주는 노트
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
        if (transform.localPosition.x <= 66) // Miss 영역으로 갔을 시 실패 처리
        {
            ObjectPooler.instance.RightNoteQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
        else if (transform.localPosition.x > 66 && transform.localPosition.x <= 141) // Success 판정 처리 영역
        {
            if (NoteManager.Instance.isStart == false)
            {
                SoundManager.Instance.PlayBGM(SoundManager.EBGM._196BPM_SuddenShower); // musiccontrol에서 플레이하자
                NoteManager.Instance.isStart = true;
            }
            isJudgeAble = true;
            if (isJudgeAble && Input.GetKeyDown(KeyCode.Space)||InputController.Instance.isTouch)
            {
                print("성공");
                GameManager.Instance.SpeedUp();
                CarController.SuccessVFx.Play();
                ObjectPooler.instance.RightNoteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
                InputController.Instance.isTouch = false;
            }
        }


    }
}
