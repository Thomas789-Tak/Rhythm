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
    [Tooltip("자동차 동력 파워")] [Range(1 , 25000)]
    [SerializeField] private float motorForce;
    [Tooltip("자동차 브레이크 파워")] [Range(1, 5000)]
    [SerializeField] private float brakeForce;
    [Tooltip("최대 스티어링 각도")] [Range(1, 45)]
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

    //무게 중심 잡아주는 함수
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

    #region 차량 이동 관련 코드

    //자동차의 동력과 관련된 함수(이동 & 브레이크)
    void CarEngine()
    {
        //(전륜 차량) 동력 전달해주는 부분
        //frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        //frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
        rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBrakeForce = isBraking ? brakeForce : 0;
        ApplyBraking();
    }

    //브레이크를 가해주는 함수
    void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
        

    }

    //차량 좌우 이동 함수
    void CarSteering()
    {
        currentSteerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    //바퀴 콜라이더와 트랜스폼을 쌍으로 전달하는 함수
    void UpdateWheelMesh()
    {
        UpdateSingleWheel(frontLeftWheelCollider,frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider,frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider,rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider,rearRightWheelTransform);
    }

    //차량 바퀴 움직임 업데이트하는 함수
    void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    #endregion

    #region 키 입력 관련 코드

    //키 입력 할당하는 함수
    void GetInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
    }

    #endregion
}
