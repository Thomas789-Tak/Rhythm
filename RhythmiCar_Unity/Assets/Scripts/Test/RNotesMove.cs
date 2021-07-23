using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RNotesMove : MonoBehaviour
{
    public float noteSpeed;
    void Start()
    {
        noteSpeed = 108*2f;       
    }


    void Update()
    {
        transform.localPosition += -Vector3.right * noteSpeed * Time.deltaTime;
    }

}
