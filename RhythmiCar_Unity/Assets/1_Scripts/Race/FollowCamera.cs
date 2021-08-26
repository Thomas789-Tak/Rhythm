using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [Header("[ī�޶� ��ġ ����]")][SerializeField] Vector3 offset;
    [SerializeField] Transform target;
    float translateSpeed;
    float rotationSpeed;

    void Start()
    {
        transform.parent = null;
        offset.x = 0f;
        //offset.y = 4f;
        offset.y = 15f;
        offset.z = -10f;
        translateSpeed = 15f;
        rotationSpeed = 11f;
    }

    private void LateUpdate()
    {
        //CameraTranslation();
        //CameraRotation();
    }


    //ī�޶� �����ӿ� �����ϴ� �Լ�
    void CameraTranslation()
    {
        var targetPosition = target.TransformPoint(offset);
        //������ ����
        transform.position = Vector3.Lerp(transform.position, targetPosition, translateSpeed * Time.deltaTime);
    }

    //ī�޶� ������ �����ϴ� �Լ�
    void CameraRotation()
    {
        var direction = target.position - transform.position;
        var rotation = Quaternion.LookRotation(direction,Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }
}
