using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftNotesBehaviour : MonoBehaviour // 보여지기만 하는 가상의 노트
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
        transform.localPosition += noteSpeed * Time.deltaTime * Vector3.right;
        if(transform.localPosition.x >= -66) // Miss 영역으로 갔을 시 실패 처리
        {
            ObjectPooler.instance.LeftNoteQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
        else if (transform.localPosition.x < -66 && transform.localPosition.x >= -141) // Success 판정 처리 영역
        {
            isJudgeAble = true;
            if (isJudgeAble && Input.GetKeyDown(KeyCode.Space))
            {
                ObjectPooler.instance.LeftNoteQueue.Enqueue(gameObject);
                gameObject.SetActive(false);
            }
        }
    }

}
