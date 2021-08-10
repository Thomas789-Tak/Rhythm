using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightNotesBehaviour : MonoBehaviour
{
    public float noteSpeed;
    private void OnEnable()
    {
        noteSpeed = NoteManager.Instance.bpm * 2;
    }


    void Update()
    {
        transform.localPosition += noteSpeed * Time.deltaTime * -Vector3.right;
    }

}
