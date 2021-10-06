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
        scrollSpeed = 100; // ��ũ�� �ӵ�
        posValue = 1000; // ������ǥ���� ���̸�ŭ�� ��ġ��ǥ
    }

    void Update()
    {
        newPos = Mathf.Repeat(Time.time * scrollSpeed, posValue);
        transform.position = startPos - Vector3.right * newPos;
    }
}
