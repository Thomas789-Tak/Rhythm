using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    Rigidbody MyRigidBody;
    SphereCollider MySphereCollider;
    Transform GroundCheckPos;
    Transform FrontLeftWheel;
    Transform FrontRightWheel;
    Transform CarColliderPos;
    TrailRenderer LeftSkidMark;
    TrailRenderer RightSkidMark;
    ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;
    public MusicController music;
    Scrollbar Kick;
    Scrollbar SteeringWheel;

    [Header("유저 데이터")]
    [SerializeField] int myID;
    [SerializeField] string myVehicleName;
    [SerializeField] int myMusicEquipCount;

    [Header("유저 스탯")]
    int musicEquipCount;
    [SerializeField] [Tooltip("가속도")] [Range(1, 100)] float myAcceleration;
    [SerializeField] [Tooltip("최대 부스터게이지량")] [Range(1, 300)] float myBoosterMaxGauge;
    [SerializeField] [Tooltip("부스터속도")] [Range(1, 300)] float myBoosterSpeed;
    [SerializeField] [Tooltip("최대회전각도")] [Range(1, 300)] float myMaxSteering=45;
    [SerializeField] [Tooltip("회전속도")] [Range(1, 300)] float myTurnStrength =30f;
    [SerializeField] [Tooltip("제동력")] [Range(1, 300)] float myBrakeForce = 50f;
    [SerializeField] [Tooltip("제동마찰력")] [Range(1, 300)] float myFriction =40f;
    [SerializeField] [Tooltip("현재속도")] [Range(1, 300)] public float myCurrentSpeed;
    float myBoosterCurrentGauge;
    float rhythmPower;
    public float maxSpeed;

    float gravityForce =2000f; // 중력
    float groundRayLength = 1.5f; // 바닥체크 레이 길이
    float groundDrag = 3f; //지상에서의 공기저항
    float groundAngularDrag = 2f;
    float turnInput; // 좌우 방향키 입력값
    float turnLerpSpeed=160f; // 턴할 때 부드러운 정도
    float HillLerpSpeed=10f; // 언덕올라갈 때 부드러운 정도
    float driftTime;

    bool isGrounding; //지상인지 체크
    bool isDrifting; // 드리프트중인지 체크
    bool isBoosting; // 부스터중인지 체크
    bool isTouchWheel;


    int GroundLayer;
    int hillLayer;

    void Awake()
    {
        InitRigidBody(); // (1)
        Kick = GameObject.Find("Kick").GetComponent<Scrollbar>();
        SteeringWheel = GameObject.Find("SteeringWheel").GetComponent<Scrollbar>();
    }

    // (1) 리지드바드 + 스페어콜라이더 초기화하는 함수
    void InitRigidBody()
    {
        MyRigidBody = transform.GetChild(2).GetComponent<Rigidbody>();
        MyRigidBody.interpolation = RigidbodyInterpolation.Extrapolate;
        MySphereCollider = MyRigidBody.GetComponent<SphereCollider>();
        CarColliderPos = MyRigidBody.transform.GetChild(0).GetComponent<Transform>();
        GroundCheckPos = transform.GetChild(3).GetComponent<Transform>();
        GroundCheckPos.parent = null;
        FrontLeftWheel = transform.Find("Wheels").transform.Find("Meshes").transform.Find("FLW").GetComponent<Transform>();
        FrontRightWheel = transform.Find("Wheels").transform.Find("Meshes").transform.Find("FRW").GetComponent<Transform>();
        RightSkidMark = transform.Find("Wheels").transform.Find("Meshes").transform.Find("FRW").transform.Find("rightSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        LeftSkidMark = transform.Find("Wheels").transform.Find("Meshes").transform.Find("FLW").transform.Find("leftSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
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
        CheckGround();
        CarEngine(); // (3)   

    }

    void Update()
    {
        GetInput(); // (2)
        UpdateObjectTransform(); // (4)
        UpdateCurrentState(); // (6)
    }
    // (4) 각종 오브젝트 위치를 할당하는 함수
    void UpdateObjectTransform()
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



    // (6) 현재 상태를 업데이트 하는 함수
    void UpdateCurrentState()
    {

        if(isTouchWheel==false)
        {
            SteeringWheel.value = 0.5f;
        }
        //if(music.isKickButtonDown)
        //{
        //    driftTime += Time.deltaTime;
        //}
        if(Input.GetKey(KeyCode.LeftShift))
        {
            driftTime += Time.deltaTime;
        }
        else
        {
            driftTime = 0;
        }
    }

    // (5) 점프 시 콜라이더 비활성화
    void SetColliderWhenFly()
    {
        //Invoke("DelaySetCollider", 2f);
        if(transform.position.y<=-19f)
        {
            GameManager.Instance.Retry.gameObject.SetActive(true);
            MusicManager.Instance.ifI.Pause();
            MusicManager.Instance.wakeUp.Pause();
            Time.timeScale = 0;

        }

    }
    void DelaySetCollider()
    {
        CarColliderPos.gameObject.SetActive(true);
    }


    public void GetSpeedUp()
    {
        if (myCurrentSpeed <= maxSpeed)
        {
            myCurrentSpeed += myAcceleration;
        }
        if(myCurrentSpeed>maxSpeed)
        {
            myCurrentSpeed=maxSpeed;
        }
        SuccessVFx.Play();
    }
    public void GetSpeedDown()
    {
        if(myCurrentSpeed>0)
        {
            myCurrentSpeed -= myAcceleration * 0.5f;
            FailVFx.Play();
        }
        else
        {
            myCurrentSpeed = 0;
        }

    }

    // (2) 플레이어의 인풋을 받아오는 함수
    void GetInput()
    {

        //turnInput = (SteeringWheel.value-0.5f)*2f;
        if (myCurrentSpeed != 0)
        {
            turnInput = Input.GetAxis("Horizontal");
        }
        else
        {
            turnInput = 0;
        }

        //차량 위치 => 리지드바디 위치로 갱신
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myCurrentSpeed += myAcceleration;
        }

        //드리프트
        if(driftTime>=0.2f && isGrounding&&myCurrentSpeed>30f) //속도가 일정이하로 떨어지면 드리프트 안되게 하자  , ,속도에 대한 정의 필요
        {
            isDrifting = true;
            if (MusicManager.Instance.Skid.isPlaying == false)
            {
                MusicManager.Instance.Skid.Play();
            }
            LeftSkidMark.emitting = true;
            RightSkidMark.emitting = true;
        }
        else
        {
            isDrifting = false;
            LeftSkidMark.emitting = false;
            RightSkidMark.emitting = false;
        }
    }

    // (3) 차량에 동력을 전달하는 함수
    void CarEngine()
    {
        //바닥체크 함수
        if (isGrounding)
        {       
            //엑셀
            MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f,ForceMode.Force);
            MyRigidBody.drag = groundDrag;
        }
        else
        {
            //공중에 떴을 때 처리
            MyRigidBody.drag = 0f;
            MyRigidBody.angularDrag = 0f;
            MyRigidBody.AddForce(gravityForce * -Vector3.up,ForceMode.Force);
        }

        //드리프트
        if (isDrifting)
        {
            float referenceDrift=myCurrentSpeed*0.3f;
            //오른쪽
            if (Input.GetAxis("Horizontal") > 0)
            {
                //현재 속도를 참조해서 하자  
                /// 턴인풋 받기
                MyRigidBody.AddForce(Vector3.Slerp(-transform.right, -transform.right * myFriction*referenceDrift*turnInput, 500f * Time.deltaTime), ForceMode.Impulse);
                print("드리프트 속도"+transform.right * myFriction * referenceDrift * turnInput);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength * 0.5f * Time.deltaTime, 0f)), turnLerpSpeed * Time.deltaTime);
                if (myCurrentSpeed > 0)
                {
                    myCurrentSpeed -= myBrakeForce *referenceDrift* Time.deltaTime;
                }
            }
            else if(Input.GetAxis("Horizontal")==0)
            {
                if(myCurrentSpeed > 0)
                {
                    myCurrentSpeed -= myBrakeForce * Time.deltaTime;
                }
            }
            //왼쪽
            else
            {
                if (myCurrentSpeed > 0)
                {
                    MyRigidBody.AddForce(Vector3.Slerp(transform.right, transform.right * myFriction * referenceDrift * Mathf.Abs(turnInput), 500f * Time.deltaTime), ForceMode.Impulse);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength * 0.5f * Time.deltaTime, 0f)), turnLerpSpeed * Time.deltaTime);
                    myCurrentSpeed -= myBrakeForce * referenceDrift * Time.deltaTime;
                }


            }

            //파티클 넣어주기

        }
    }

    //바닥체크 함수 + 각도 조정
    void CheckGround()
    {
        RaycastHit groundInfo;
        if (Physics.Raycast(GroundCheckPos.position, -transform.up, out groundInfo, groundRayLength, GroundLayer))
        {
            isGrounding = true;
            //각도가 있는 지형에서 차량의 각도를 조정해주는 부분
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, groundInfo.normal) * transform.rotation, Time.deltaTime * HillLerpSpeed);
            //좌우 이동
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength * Time.deltaTime, 0f)), turnLerpSpeed*Time.deltaTime);
        }
        else
        {
            isGrounding = false;
            SetColliderWhenFly(); // (5)     
        }
    }

    public void GetTouchDown()
    {
        isTouchWheel = true;
    }
    public void GetTouchUp()
    {
        isTouchWheel = false;
    }
}
