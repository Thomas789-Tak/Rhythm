using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightNotesBehaviour : MonoBehaviour
{
    public float noteSpeed;
    void Start()
    {
        noteSpeed = NoteManager.Instance.bpm*2;
    }


    void Update()
    {
        transform.localPosition += -Vector3.right * noteSpeed * Time.deltaTime;
    }

}
