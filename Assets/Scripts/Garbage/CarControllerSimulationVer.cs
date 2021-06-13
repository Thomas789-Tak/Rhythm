using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarControllerSimulationVer : MonoBehaviour
{
    const string HORIZONTAL = "Horizontal";
    const string VERTICAL = "Vertical";
    
    
    float horizontalInput;
    float verticalInput;
    float steeringAngle;   
    float currentSteerAngle;
    float currentBrakeForce;
    bool isBraking;
    float centerMass;
    [Tooltip("�ڵ��� ���� �Ŀ�")] [Range(1 , 25000)]
    [SerializeField] private float motorForce;
    [Tooltip("�ڵ��� �극��ũ �Ŀ�")] [Range(1, 5000)]
    [SerializeField] private float brakeForce;
    [Tooltip("�ִ� ��Ƽ� ����")] [Range(1, 45)]
    [SerializeField] float maxSteeringAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    Rigidbody rigid;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        SetCenterOfMass();
    }

    //���� �߽� ����ִ� �Լ�
    void SetCenterOfMass()
    {
        centerMass = -0.9f;
        rigid.centerOfMass = new Vector3(0, centerMass, 0);
    }

    void FixedUpdate()
    {
        GetInput();
        CarEngine();
        CarSteering();
        UpdateWheelMesh();
    }
    void Update()
    {
        print(frontLeftWheelCollider.steerAngle);
    }

    #region ���� �̵� ���� �ڵ�

    //�ڵ����� ���°� ���õ� �Լ�(�̵� & �극��ũ)
    void CarEngine()
    {
        //(���� ����) ���� �������ִ� �κ�
        //frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        //frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBrakeForce = isBraking ? brakeForce : 0;
        ApplyBraking();
    }

    //�극��ũ�� �����ִ� �Լ�
    void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
        

    }

    //���� �¿� �̵� �Լ�
    void CarSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    //���� �ݶ��̴��� Ʈ�������� ������ �����ϴ� �Լ�
    void UpdateWheelMesh()
    {
        UpdateSingleWheel(frontLeftWheelCollider,frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider,frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider,rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider,rearRightWheelTransform);
    }

    //���� ���� ������ ������Ʈ�ϴ� �Լ�
    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    #endregion

    #region Ű �Է� ���� �ڵ�

    //Ű �Է� �Ҵ��ϴ� �Լ�
    void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
    }

    #endregion
}
