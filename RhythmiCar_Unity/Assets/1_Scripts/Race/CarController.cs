using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public Rigidbody MyRigidBody;
    SphereCollider MySphereCollider;
    Transform GroundCheckPos;
    Transform FrontLeftWheel;
    Transform FrontRightWheel;
    Transform CarColliderPos;
    TrailRenderer BackLeftSkidMark;
    TrailRenderer BackRightSkidMark;
    public static ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;

    [Header("유저 데이터")]
    [SerializeField] int myID;
    [SerializeField] string myVehicleName;

    [Header("유저 스탯")]
    [SerializeField]
    List<SoundManager.EBGM> mySongEquipList = new List<SoundManager.EBGM>();
    [SerializeField] [Tooltip("가속도")] [Range(1, 100)] float myAcceleration;
    [SerializeField] [Tooltip("최대 부스터게이지량")] [Range(1, 300)] float myBoosterMaxGauge;
    [SerializeField] [Tooltip("부스터속도")] [Range(1, 300)] float myBoosterSpeed;
    [SerializeField] [Tooltip("회전속도")] [Range(1, 300)] float myTurnStrength;
    [SerializeField] [Tooltip("제동력")] [Range(1, 300)] float myBrakeForce;
    [SerializeField] [Tooltip("제동마찰력")] [Range(1, 300)] float myFriction;
    float myRhythmPower;
    public float myCurrentSpeed;
    float myMaxSteering=45;
    float myBoosterCurrentGauge;
    public float maxSpeed;

    float gravityForce =2000f; // 중력
    float groundRayLength = 1.5f; // 바닥체크 레이 길이
    float groundDrag = 3f; //지상에서의 공기저항
    float groundAngularDrag = 2f;
    float turnInput; // 좌우 방향키 입력값
    float turnLerpSpeed=160f; // 턴할 때 부드러운 정도
    float HillLerpSpeed=10f; // 언덕올라갈 때 부드러운 정도
    float driftTime;
    float optimizeRayCheckTime;

    bool isGrounding; //지상인지 체크
    bool isDrifting; // 드리프트중인지 체크
    public bool isBoosting { get; set; } // 부스터중인지 체크


    int GroundLayer;
    int hillLayer;

    void Awake()
    {
        InitRigidBody(); // (1)
        SoundManager.Instance.PlayOneShotSFX(SoundManager.ESFX.EngineStartSound);
    }

    void InitRigidBody() // (1) 리지드바드 + 스페어콜라이더 초기화하는 함수
    {
        /// 초기화 할때 mass 값과ㅓ 같은건 프리팹으로 따로 빼서 awake 때 호출하지 않게 하자 
        MyRigidBody = transform.GetChild(2).GetComponent<Rigidbody>();
        MyRigidBody.interpolation = RigidbodyInterpolation.Extrapolate;
        MySphereCollider = MyRigidBody.GetComponent<SphereCollider>();
        CarColliderPos = MyRigidBody.transform.GetChild(0).GetComponent<Transform>();
        GroundCheckPos = transform.GetChild(3).GetComponent<Transform>();
        GroundCheckPos.parent = null;
        FrontLeftWheel = transform.Find("Wheels").transform.Find("Meshes").transform.Find("FLW").GetComponent<Transform>();
        FrontRightWheel = transform.Find("Wheels").transform.Find("Meshes").transform.Find("FRW").GetComponent<Transform>();
        BackRightSkidMark = transform.Find("Wheels").transform.Find("Meshes").transform.Find("RRW").transform.Find("BackRightSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        BackLeftSkidMark = transform.Find("Wheels").transform.Find("Meshes").transform.Find("RLW").transform.Find("BackLeftSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        MySphereCollider.radius = 0.8f;
        MyRigidBody.mass = 700f;
        MyRigidBody.drag = groundDrag;
        MyRigidBody.angularDrag = groundAngularDrag;
        GroundLayer = 1 << LayerMask.NameToLayer("Ground") | 1<< LayerMask.NameToLayer("Hill");
        hillLayer = 1 << LayerMask.NameToLayer("Hill");
        MyRigidBody.transform.parent = null;
        FailVFx = transform.Find("FailFX").GetComponent<ParticleSystem>();
        SuccessVFx = transform.Find("SuccessFX").GetComponent<ParticleSystem>();
    }

    void FixedUpdate()
    {
        CheckGroundAndRotation();
        CarEngine();

    }

    void Update()
    {
        CheckRoadType();
        GetInput();
        UpdateObjectTransform();
        UpdateCurrentState();
        BoosterState();
        print(isBoosting +""+ myBoosterCurrentGauge);
    }

    void BoosterState()
    {
        GameManager.Instance.BoosterGauge.fillAmount = myBoosterCurrentGauge / myBoosterMaxGauge;
        if(isBoosting)
        {
            myBoosterCurrentGauge -= Time.deltaTime*30f;
            if(myCurrentSpeed<=maxSpeed*2f)
            {
                myCurrentSpeed += Time.deltaTime * 80f;
            }
            if(myBoosterCurrentGauge<=0)
            {
                if(myCurrentSpeed >= maxSpeed)
                {
                    myCurrentSpeed = maxSpeed;
                }
                isBoosting = false;
            }
        }
    }

    void CheckRoadType() // 레이캐스트 체크주기를 조절하여 최적화
    {
        optimizeRayCheckTime += Time.deltaTime;
        if(optimizeRayCheckTime>=0.3f)
        {
            RaycastHit groundInfo;
            if (Physics.Raycast(GroundCheckPos.position, -transform.up, out groundInfo, groundRayLength, GroundLayer))
            {
                isGrounding = true;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, groundInfo.normal) * transform.rotation, HillLerpSpeed); //각도가 있는 지형에서 차량의 각도를 조정해주는 부분
            }
            else
            {
                isGrounding = false;
            }
            optimizeRayCheckTime = 0f;
        }

    }

    void UpdateObjectTransform()  // (4) 각종 오브젝트 위치를 할당하는 함수
    {
        //리지드 바디가 커지면 여기 y값을 조절하면 된다
        transform.position = new Vector3(MyRigidBody.transform.position.x, MyRigidBody.transform.position.y-0.3f, MyRigidBody.transform.position.z);
        //레이 원점 => 리지드바디 위치로 갱신
        GroundCheckPos.position = MyRigidBody.transform.position;
        //차량 콜라이더 위치 갱신
        CarColliderPos.position = transform.position;
        CarColliderPos.rotation = transform.rotation;
        //바퀴 메쉬 움직임 구현
        FrontLeftWheel.localRotation = Quaternion.Euler(FrontLeftWheel.localRotation.eulerAngles.x, (turnInput * myMaxSteering), FrontLeftWheel.localRotation.eulerAngles.z);
        FrontRightWheel.localRotation = Quaternion.Euler(FrontRightWheel.localRotation.eulerAngles.x, turnInput * myMaxSteering, FrontRightWheel.localRotation.eulerAngles.z);

    }

    void PcController()
    {
        //좌우 회전
        if (myCurrentSpeed != 0)
        {
            turnInput = Input.GetAxis("Horizontal");
        }
        else
        {
            turnInput = 0;
        }

        //드리프트
        if (Input.GetKey(KeyCode.LeftShift))
        {
            driftTime += Time.deltaTime;
        }
        else
        {
            driftTime = 0;
        }
        //가속
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(myCurrentSpeed<=maxSpeed)
            {
                myCurrentSpeed += myAcceleration;
                if(myBoosterCurrentGauge<myBoosterMaxGauge)  // 모바일도 구현 부스터 게이지 차는 것
                {
                    myBoosterCurrentGauge += 10;
                }
                else
                {
                    myBoosterCurrentGauge = myBoosterMaxGauge;
                }
            }
            else
            {
                myCurrentSpeed = maxSpeed;
            }
        }
        //부스터
        if(Input.GetKeyDown(KeyCode.LeftControl)&&myBoosterCurrentGauge>=myBoosterMaxGauge)
        {
            isBoosting = true;
        }
    }

    void MobileController()
    {
        //좌우 회전
        if(myCurrentSpeed!=0)
        {
            turnInput = (GameManager.Instance.WheelScrollBar.value-0.5f)*2f;
        }
        else
        {
            turnInput = 0;
        }

        // 터치가 없을 경우 중립 위치로 조정
        if (GameManager.Instance.isTouchWheelUI == false)
        {
            GameManager.Instance.WheelScrollBar.value = 0.5f;
        }
        //드리프트
        if (GameManager.Instance.isTouchKickUI)
        {
            driftTime += Time.deltaTime;
        }
        else
        {
            driftTime = 0f;
        }
        if(myBoosterCurrentGauge>=myBoosterMaxGauge)
        {
            GameManager.Instance.BoosterButton.interactable = true;
        }
        else
        {
            GameManager.Instance.BoosterButton.interactable = false;
        }
    }
    void UpdateCurrentState() // (6) 현재 상태를 업데이트 하는 함수
    {     
        //드리프트
        if (driftTime >= 0.2f && isGrounding && myCurrentSpeed > 30f)
        {
            isDrifting = true;
            SoundManager.Instance.PlayLoopSFX(SoundManager.ESFX_Loop.CarDriftSound);
            BackLeftSkidMark.emitting = true;
            BackRightSkidMark.emitting = true;
        }
        else
        {
            isDrifting = false;
            BackLeftSkidMark.emitting = false;
            BackRightSkidMark.emitting = false;
        }
    }

    void SetColliderWhenFly() // (5) 점프 시 콜라이더 비활성화
    {
        //Invoke("DelaySetCollider", 2f);
        if (transform.position.y<=-19f) // 패배조건 설정; 
        {
            GameManager.Instance.RetryButton.gameObject.SetActive(true);
            Time.timeScale = 0;

        }

    }
    void DelaySetCollider()
    {
        CarColliderPos.gameObject.SetActive(true);
    }


    void GetInput() // (2) 플레이어의 인풋을 받아오는 함수
    {
        //MobileController();
        PcController();   
    }

    void CarEngine() // 차량에 동력을 전달하는 함수
    {
        if (isGrounding&&isDrifting==false) // 평상 시 가속
        {       
            MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f,ForceMode.Force);
            MyRigidBody.drag = groundDrag;
            MyRigidBody.angularDrag = groundAngularDrag;
        }
        else if(isDrifting) // 드리프트 시
        {
            MyRigidBody.drag = 1f;
            MyRigidBody.angularDrag = 1f;
            // 기존 MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f*0.01f, ForceMode.Force);
            MyRigidBody.AddForce(transform.forward* myCurrentSpeed * 1000f*0.5f, ForceMode.Force);
        }
    }

    void CheckGroundAndRotation() //바닥체크 함수 + 각도 조정
    {
        if (isGrounding)
        {
            if (isDrifting) // 드리프트 시 회전
            {
                if(turnInput>0) // 오른쪽 회전 시
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength*0.03f, 0f)), turnLerpSpeed);
                    MyRigidBody.AddForce(Vector3.Slerp(-transform.right, -transform.right * myFriction * myCurrentSpeed*0.05f * turnInput, 10f), ForceMode.Force);
                    if (myCurrentSpeed > 0)
                    {
                        myCurrentSpeed -= myBrakeForce;
                    }
                    else
                    {
                        myCurrentSpeed = 0f;
                    }
                }
                else if(turnInput==0) // 중립 시 
                {
                    if (myCurrentSpeed > 0)
                    {
                        myCurrentSpeed -= myBrakeForce;
                    }
                    else
                    {
                        myCurrentSpeed = 0f;
                    }
                }
                else // 왼쪽 회전 시
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength * 0.03f, 0f)), turnLerpSpeed);
                    MyRigidBody.AddForce(Vector3.Slerp(transform.right, transform.right * myFriction * myCurrentSpeed*0.05f * Mathf.Abs(turnInput), 10f), ForceMode.Force);
                    if (myCurrentSpeed > 0)
                    {
                        myCurrentSpeed -= myBrakeForce;
                    }
                    else
                    {
                        myCurrentSpeed = 0f;
                    }
                }
            }
            else // 드리프트를 하지 않는 평상시 좌우 회전
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength*0.03f, 0f)), turnLerpSpeed);
            }
        }
        else // 지상이 아닐때 상황
        {
            MyRigidBody.drag = 0f;
            MyRigidBody.angularDrag = 0f;
            MyRigidBody.AddForce(gravityForce * -Vector3.up, ForceMode.Force);
            SetColliderWhenFly();
        }
    }

    public void SpeedUP()
    {
        if (myCurrentSpeed <= maxSpeed)
        {
            myCurrentSpeed += myAcceleration;
        }
        else
        {
            myCurrentSpeed = maxSpeed;
        }
    }
}
