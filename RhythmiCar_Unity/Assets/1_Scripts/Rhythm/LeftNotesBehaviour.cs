using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftNotesBehaviour : MonoBehaviour
{
    public float noteSpeed;
    bool isJudgeAble;
    private void OnEnable()
    {
        noteSpeed = NoteManager.Instance.bpm * 1.5f;
    }


    void Update()
    {
        transform.localPosition += noteSpeed * Time.deltaTime * Vector3.right;
        if(transform.localPosition.x >= -66)
        {
            gameObject.SetActive(false);
        }
        else if (transform.localPosition.x < -66 && transform.localPosition.x >= -141)
        {
            isJudgeAble = true;
            if (isJudgeAble && Input.GetKeyDown(KeyCode.Space))
            {
                gameObject.SetActive(false);
            }
        }
    }

}
