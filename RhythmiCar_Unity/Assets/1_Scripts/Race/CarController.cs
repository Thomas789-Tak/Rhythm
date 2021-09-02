using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    //구현해야할 내용 : 부스터 중일 때 리듬실패 처리 X  ,
    [Header("유저 스탯")]
    [SerializeField]
    List<SoundManager.EBGM> songEquipList = new List<SoundManager.EBGM>();
    [SerializeField] [Tooltip("가속도")] [Range(1, 100)] float acceleration;
    [SerializeField] [Tooltip("최대 부스터게이지량")] [Range(1, 300)] float boosterMaxGauge;
    float boosterCurrentGauge;
    [SerializeField] float boosterSpeed;
    [SerializeField] float currentRhythmEnergy;
    [SerializeField] float maxRhythmEnergy;
    [SerializeField] float currentSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] float turnSpeed;
    public bool isBoosting { get; set; }

    int currentDirection;
    int roadCount;
    int combo;
    List<float> gearRatio = new List<float>();
    [SerializeField]int currentGear;
    //참조 영역
    Transform Body;
    TrailRenderer BackLeftSkidMark;
    TrailRenderer BackRightSkidMark;
    [SerializeField] ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;
    Rigidbody rigid;
    Vector3[] WayPoint = new Vector3[10];
    IEnumerator Co_Booster;
    void Awake()
    {
        gearRatio.Add(30);
        gearRatio.Add(60);
        gearRatio.Add(90);
        gearRatio.Add(120);
        print(maxSpeed + "" + currentGear);
        InitReference();
        SoundManager.Instance.PlayOneShotSFX(SoundManager.ESFX.EngineStartSound);
    }
    private void Start()
    {
        InputManager.Instance.touchEvent.AddListener(Judge);
        InputManager.Instance.verticalEvent.AddListener(SetGear);
        InputManager.Instance.horizontalEvent.AddListener(MoveDirection);
        UIManager.Instance.SetMaxRhythmEnergy(maxRhythmEnergy);
        UIManager.Instance.SetCurrentRhythmEnergy(currentRhythmEnergy);
        GameManager.Instance.car = GetComponent<CarController>();
    }
    //--------------------------------------------------update----------------------------------------------------------------------
    void Update()
    {
        DecreaseRhythmEnergy();
        Accelerate();
        SteeringWheel();
        ObserveGearState();
        //GameManager.Instance.text.text = currentSpeed.ToString("N0") + "Km/h"+ " 현재기어 ["+currentGear.ToString()+"]";
        UIManager.Instance.SetCarInformation(currentSpeed, currentGear);
        UIManager.Instance.SetCurrentRhythmEnergy(currentRhythmEnergy);
    }
    //----------------------------------------------------update--------------------------------------------------------------------

    void InitReference() // 각종 참조를 하는 함수
    {
        //나중에 여기서 인게임초기화 VO 받아서 하자
        currentGear = 1;
        maxSpeed = gearRatio[currentGear - 1];
        roadCount = 6; //로드카운트 게임매니저에서 받아오자
        //maxRhythmPower = 10;
        currentRhythmEnergy = maxRhythmEnergy;
        //boosterMaxGauge = 7;
        //boosterSpeed = 10f;
        Body = transform.Find("Body").GetComponent<Transform>();
        BackRightSkidMark = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("RRW").transform.Find("BackRightSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        BackLeftSkidMark = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("RLW").transform.Find("BackLeftSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        FailVFx = transform.Find("FailFX").GetComponent<ParticleSystem>();
        SuccessVFx = transform.Find("SuccessFX").GetComponent<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();
        if(roadCount%2==0) // 차선이 짝수
        {
            currentDirection = roadCount / 2;
            transform.position = new Vector3(0f, 0f, -8 * currentDirection);
        }
        else // 차선이 홀수
        {
            currentDirection = roadCount / 2; 
            transform.position = new Vector3(0f, 0f, -8 * currentDirection);
        }
        Co_Booster = Booster();
    }


    public void Judge(EJudge judgement)
    {
        if(isBoosting==false)
        {
            switch (judgement)
            {
                case EJudge.Perfect:
                    SpeedUP();
                    
                    break;

                case EJudge.Good:
                    SpeedUP();
                    break;

                case EJudge.Bad:
                    SpeedDown();
                    break;

                case EJudge.Miss:
                    SpeedDown();
                    break;
            }

        }
        UIManager.Instance.SetNoteCombo(combo);
        
    }

    public void GetItem(EnumItemVO.EItemType itemType, int goldAmount=0 , int noteAmount=0, int starAmount=0, float rhythmEnergyAmount=0 )
    {
        //연출넣자 파티클 같은거
        switch (itemType)
        {
            case EnumItemVO.EItemType.gold:

                GameManager.Instance.Gold += goldAmount;

                break;

            case EnumItemVO.EItemType.note:

                GameManager.Instance.Note += noteAmount;

                break;

            case EnumItemVO.EItemType.rhythmEnergy:
                
                if(currentRhythmEnergy<maxRhythmEnergy)
                {
                    currentRhythmEnergy += rhythmEnergyAmount;
                }
                if(currentRhythmEnergy>maxRhythmEnergy)
                {
                    currentRhythmEnergy = maxRhythmEnergy;
                }

                break;

            case EnumItemVO.EItemType.star:
                
                GameManager.Instance.Star += starAmount;

                break;

            case EnumItemVO.EItemType.booster:

                isBoosting = true;
                boosterCurrentGauge = boosterMaxGauge;
                StartCoroutine(Co_Booster);

                break;
        }
    }

    public IEnumerator Booster() // 부스터를 활성화 하는 함수
    {
        while (isBoosting)
        {
            print("부스터중");
            boosterCurrentGauge -= Time.deltaTime;
            if(currentGear<gearRatio.Count)
            {
                if (currentSpeed <= gearRatio[currentGear])
                {
                    currentSpeed += boosterSpeed * Time.deltaTime;
                }
            }
            else
            {
                if (currentSpeed <= maxSpeed*1.5f)
                {
                    currentSpeed += boosterSpeed * Time.deltaTime;
                }
            }

            if(boosterCurrentGauge<=0)
            {
                if(currentSpeed >= maxSpeed)
                {
                    currentSpeed = maxSpeed;
                }
                isBoosting = false;
            }
            yield return null;
        }
    }

    void DecreaseRhythmEnergy() // 자동적으로 연료를 닳게 하는 함수
    {
        currentRhythmEnergy -= Time.deltaTime;
        if(currentRhythmEnergy<=0)
        {
            //게임오버
        }
    }
    void ObserveGearState() // 현재속도가 낮아 기어를 낮추는 조건을 탐색하는 함수 
    {
        if(currentGear>2) // 2단 기어보다 높을 때 
        {
            if (currentSpeed <= gearRatio[currentGear - 2] - 10f)
            {
                currentSpeed = (gearRatio[currentGear - 2] + gearRatio[currentGear - 3]) * 0.5f;
                currentGear--;
            }
        }
        else if(currentGear==2)
        {
            if (currentSpeed <= gearRatio[currentGear - 2] - 10f)
            {
                currentSpeed = gearRatio[currentGear - 2] * 0.5f;
                currentGear--;
            }

        }
    }

    void DelayRotate() // 회전 뒤 딜레이를 줘서 차량을 정면으로 유지시키는 함수
    {
        //Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,90, transform.eulerAngles.z),3f);
        Body.transform.DOLocalRotate(Vector3.zero, 0.3f);
    }

    public void MoveDirection(int direction)
    {
        if(direction==1) // 오른쪽
        {
            if (currentSpeed != 0)
            {
                if (currentDirection < roadCount - 1)
                {
                    currentDirection++;
                    Body.transform.DOLocalRotate(new Vector3(0, 5f, 0), 0.3f);

                    Invoke("DelayRotate", 0.3f);
                }

            }
        }
        else //왼쪽
        {
            if (currentSpeed != 0)
            {
                if (currentDirection != 0)
                {
                    currentDirection--;
                    Body.transform.DOLocalRotate(new Vector3(0, -5f, 0), 0.3f);

                    Invoke("DelayRotate", 0.3f);
                }
            }
        }
    }

    void Accelerate() // 차량의 현재속도를 받아와 지속적으로 움직이게 하는 함수
    {
        transform.Translate(Vector3.forward*currentSpeed*Time.deltaTime);
    }
    void SteeringWheel() // 좌우 이동 키 입력을 받아 해당하는 지점으로 이동하는 함수
    {
        for (int i = 0; i < roadCount; i++) //이동할 지점들 위치 업데이트
        {
            WayPoint[i] = new Vector3(transform.position.x, 0f, i * -8f);
        }
        //transform.position = Vector3.MoveTowards(transform.position, WayPoint[currentDirection], turnSpeed * Time.deltaTime);
        transform.DOMoveZ(WayPoint[currentDirection].z, turnSpeed, false).SetEase(Ease.OutExpo);
    }

    public void SpeedUP() // 노트 성공 시 속도를 올려주는 함수 --- 이 부분을 판정 결과와 관련된 함수로 바꾸자
    {
        SuccessVFx.Play();
        if (currentSpeed <= maxSpeed)
        {
            currentSpeed += acceleration;

        }
        if(currentSpeed>maxSpeed)
        {
            if(isBoosting==false)
            {
                currentSpeed = maxSpeed;
            }
        }
    }


    public void SpeedDown() // 노트 실패 시 속도를 내려주는 함수
    {
        FailVFx.Play();
        if (currentSpeed>=0)
        {

            currentSpeed -= 5f;
        }
        if(currentSpeed<0)
        {
            currentSpeed = 0;
        }
    }

    public void Bump()  // 다른 오브젝트 부딪힐 때 피해 X, 다른 오브젝트랑 부딪칠 때 리듬 에너지 감소
    {
        if(isBoosting)
        {
            
        }
        else
        {
            //카메라 쉐이크
            //차량 회전하는 연출
            // 이펙트 발생
            //속도 감소
            // 리듬 에너지 감소
        }
    }

    public void SetGear(int direction) // 기어를 올리거나 내리는 함수
    {
        if(direction>0) // 위쪽 && 기어업
        {
            print("기어업");
            if(currentGear < gearRatio.Count&&currentSpeed>=maxSpeed)
            {
                currentGear++;                
            }
            maxSpeed = gearRatio[currentGear-1];
        }
        else // 아래쪽 && 기어다운
        {
            if(currentGear!=1)
            {
                SoundManager.Instance.PlayLoopSFX(SoundManager.ESFX_Loop.CarDriftSound);
                BackLeftSkidMark.emitting = true;
                BackRightSkidMark.emitting = true;
                Invoke("DelayDriftFX", 0.3f);
                maxSpeed = gearRatio[currentGear - 2];
                if(currentGear==2)
                {
                    currentSpeed = maxSpeed * 0.5f;
                }
                else
                {
                    currentSpeed = (maxSpeed+gearRatio[currentGear-3])*0.5f;
                }
                currentGear--;
            }
        }

    }
    void DelayDriftFX() // 드리프트 FX를 딜레이를 준 뒤 꺼주는 함수
    {
        BackLeftSkidMark.emitting = false;
        BackRightSkidMark.emitting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            // 승리 처리
        }
    }
}