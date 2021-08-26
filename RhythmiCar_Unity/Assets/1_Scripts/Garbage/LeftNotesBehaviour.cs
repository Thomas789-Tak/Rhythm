using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftNotesBehaviour : MonoBehaviour // �������⸸ �ϴ� ������ ��Ʈ
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
        if(transform.localPosition.x >= -66) // Miss �������� ���� �� ���� ó��
        {
            ObjectPooler.instance.LeftNoteQueue.Enqueue(gameObject);
            gameObject.SetActive(false);
        }
        else if (transform.localPosition.x < -66 && transform.localPosition.x >= -141) // Success ���� ó�� ����
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
