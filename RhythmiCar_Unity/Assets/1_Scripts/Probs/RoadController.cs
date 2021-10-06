using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    [SerializeField] [Range(30f, 60f)] float scrollSpeed;
    [SerializeField] float posValue;

    Vector3 startPos;
    float newPos;

    void Start()
    {
        transform.position = new Vector3(1000, 0, 0);
        startPos = transform.position;
        scrollSpeed = 100; // 스크롤 속도
        posValue = 1000; // 원점좌표와의 차이만큼의 위치좌표
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * scrollSpeed, posValue);
        transform.position = startPos - Vector3.right * newPos;
    }
}
