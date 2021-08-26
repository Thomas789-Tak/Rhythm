﻿using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    public RhythmJudge RhythmJudge;

    public bool isSet = false;
    public int num;
    public float time;
    public float speed;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SetPosition();
        SetLife();
    }

    private void SetPosition()
    {
        if (isSet == false) return;

        Vector3 vec = transform.localPosition;
        float posY = (time - RhythmJudge.songPlayTime) / speed;

        vec.y = posY;

        transform.localPosition = vec;
    }

    private void SetLife()
    {
        if (isSet == false) return;

        if (time < 0)
            gameObject.SetActive(false);

    }
}