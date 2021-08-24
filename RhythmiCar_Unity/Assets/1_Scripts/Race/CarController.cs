using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{

    [Header("유저 스탯")]
    [SerializeField]
    List<SoundManager.EBGM> songEquipList = new List<SoundManager.EBGM>();
    [SerializeField] [Tooltip("가속도")] [Range(1, 100)] float acceleration;
    [SerializeField] [Tooltip("최대 부스터게이지량")] [Range(1, 300)] float boosterMaxGauge;
    float boosterCurrentGauge;
    float boosterSpeed;    
    float rhythmPower;
    float maxRhythmPower;   
    public float currentSpeed;
    public float maxSpeed;
    float maxSteering=45;
    float turnInput; // 좌우 방향키 입력값
    public bool isBoosting { get; set; }

    int myDirection;
    int roadCount;
    //참조 영역
    Transform FrontLeftWheel;
    Transform FrontRightWheel;
    Transform Body;
    TrailRenderer BackLeftSkidMark;
    TrailRenderer BackRightSkidMark;
    public static ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;
    Rigidbody rigid;
    Vector3[] WayPoint = new Vector3[10];

    void Awake()
    {
        InitReference();
        SoundManager.Instance.PlayOneShotSFX(SoundManager.ESFX.EngineStartSound);
    }
    void Update()
    {
        DecreaseRhythmPower();
        GetInputType();
        Accelerate();
        Booster();
        SteeringWheel();
    }
    void InitReference() // 각종 참조를 하는 함수
    {
        roadCount = 6;
        maxRhythmPower = 100;
        rhythmPower = maxRhythmPower;
        boosterMaxGauge = 100;
        Body = transform.Find("Body").GetComponent<Transform>();
        FrontLeftWheel = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("FLW").GetComponent<Transform>();
        FrontRightWheel = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("FRW").GetComponent<Transform>();
        BackRightSkidMark = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("RRW").transform.Find("BackRightSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        BackLeftSkidMark = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("RLW").transform.Find("BackLeftSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        FailVFx = transform.Find("FailFX").GetComponent<ParticleSystem>();
        SuccessVFx = transform.Find("SuccessFX").GetComponent<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();
        if(roadCount%2==0) // 차선이 짝수
        {
            myDirection = roadCount / 2;
            transform.position = new Vector3(0f, 1.2f, -8*myDirection);
        }
        else // 차선이 홀수
        {
            myDirection = roadCount / 2;
            transform.position = new Vector3(0f, 1.2f, -8 * myDirection);
        }
    }
    void GetInputType() // (2) 플레이어의 인풋을 받아오는 함수
    {
        //MobileController();
        PcController();
    }

    public void Booster()
    {
        GameManager.Instance.BoosterGauge.fillAmount = boosterCurrentGauge / boosterMaxGauge;
        if(isBoosting)
        {
            boosterCurrentGauge -= Time.deltaTime*30f;
            if(currentSpeed<=maxSpeed*2f)
            {
                currentSpeed += Time.deltaTime * 80f;
            }
            if(boosterMaxGauge<=0)
            {
                if(currentSpeed >= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
                isBoosting = false;
            }
        }
    }

    void DecreaseRhythmPower()
    {
        rhythmPower -= Time.deltaTime;
        GameManager.Instance.RhythmPower.value = rhythmPower;
        GameManager.Instance.RhythmPower.maxValue = maxRhythmPower;
        if(rhythmPower<=0)
        {
            //게임오버
        }
    }

    void DelayRotate()
    {
        //Body.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,90, transform.eulerAngles.z),3f);
    }
    void PcController()
    {
        //좌우 회전
        if (currentSpeed != 0)
        {
            if(myDirection<roadCount-1&&Input.GetKeyDown(KeyCode.D)) // 오른쪽 이동
            {
                myDirection++;
                Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 15, 0f)), 3f);                
                Invoke("DelayRotate", 0.1f);
            }
            else if(myDirection!=0&&Input.GetKeyDown(KeyCode.A))
            {
                myDirection--;
                Body.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -15, 0f)),3f);
                Invoke("DelayRotate", 0.1f);
            }
        }

        //정답영역에서 호출할 수 있게
        //가속
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(currentSpeed<=maxSpeed)
            {
                if(boosterCurrentGauge<boosterMaxGauge&&isBoosting==false)
                {
                    boosterCurrentGauge += 10;
                }
                else if(boosterCurrentGauge>boosterMaxGauge)
                {
                    boosterCurrentGauge = boosterMaxGauge;
                }
            }
            else
            {
                currentSpeed = maxSpeed;
            }
        }
        //부스터
        if(Input.GetKeyDown(KeyCode.LeftControl)&&boosterCurrentGauge>=boosterMaxGauge)
        {
            isBoosting = true;
        }
    }

    public void MoveLeft()
    {
        if (currentSpeed != 0)
        {
            if (myDirection != 0)
            {
                myDirection--;
                Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -5, 0f)), 3f);
                Invoke("DelayRotate", 0.1f);
            }
        }
    }
    public void MoveRight()
    {
        if (currentSpeed != 0)
        {
            if (myDirection < roadCount - 1)
            {
                myDirection++;
                Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 5, 0f)), 3f);
                Invoke("DelayRotate", 0.1f);
            }

        }
    }

    void Accelerate() // 차량을 가속해주는 함수
    {
        transform.Translate(Vector3.forward*currentSpeed*Time.deltaTime);

    }
    void SteeringWheel()
    {
        for(int i = 0; i<roadCount;i++)
        {
            WayPoint[i] = new Vector3(transform.position.x, transform.position.y, i * -8f);
        }
        transform.position = Vector3.MoveTowards(transform.position, WayPoint[myDirection], 40 * Time.deltaTime);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput, 0f)), 50);
        //바퀴 메쉬 움직임 구현
        FrontLeftWheel.localRotation = Quaternion.Euler(FrontLeftWheel.localRotation.eulerAngles.x, (turnInput * maxSteering), FrontLeftWheel.localRotation.eulerAngles.z);
        FrontRightWheel.localRotation = Quaternion.Euler(FrontRightWheel.localRotation.eulerAngles.x, turnInput * maxSteering, FrontRightWheel.localRotation.eulerAngles.z);

    }

    public void SpeedUP()
    {
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += acceleration;
        }
        else
        {
            currentSpeed = maxSpeed;
        }
    }
    public void GearUp()
    {

    }
    public void GearDown() // 기어를 다운 시키는 함수
    {
        SoundManager.Instance.PlayLoopSFX(SoundManager.ESFX_Loop.CarDriftSound);
        BackLeftSkidMark.emitting = true;
        BackRightSkidMark.emitting = true;
        //딜레이
        BackLeftSkidMark.emitting = false;
        BackRightSkidMark.emitting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            print("닿음");
        }
    }
}