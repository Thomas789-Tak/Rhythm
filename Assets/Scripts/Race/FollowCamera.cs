using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("[카메라 위치 조정]")][SerializeField] Vector3 offset;
    [SerializeField] Transform target;
    float translateSpeed;
    float rotationSpeed;

    void Start()
    {
        transform.parent = null;
        offset.x = 0f;
        //offset.y = 4f;
        //offset.z = -7f;
        offset.y = 5f;
        offset.z = -9.5f;
        translateSpeed = 15f;
        rotationSpeed = 6f;
    }


    private void LateUpdate()
    {
        CameraTranslation();
        CameraRotation();
    }


    //카메라 움직임에 관여하는 함수
    void CameraTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        //움직임 보간
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }

    //카메라 각도에 관여하는 함수
    void CameraRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
