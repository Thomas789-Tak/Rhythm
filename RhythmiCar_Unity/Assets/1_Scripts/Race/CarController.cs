using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{

    [Header("���� ����")]
    [SerializeField]
    List<SoundManager.EBGM> songEquipList = new List<SoundManager.EBGM>();
    [SerializeField] [Tooltip("���ӵ�")] [Range(1, 100)] float acceleration;
    [SerializeField] [Tooltip("�ִ� �ν��Ͱ�������")] [Range(1, 300)] float boosterMaxGauge;
    float boosterCurrentGauge;
    float boosterSpeed;
    float rhythmPower;
    float maxRhythmPower;
    [SerializeField] float currentSpeed;
    [SerializeField] float maxSpeed;
    float maxSteering=45;
    float turnInput; // �¿� ����Ű �Է°�
    public bool isBoosting { get; set; }

    int currentDirection;
    int roadCount;
    List<float> gearRatio = new List<float>();
    [SerializeField]int currentGear;
    //���� ����
    Transform FrontLeftWheel;
    Transform FrontRightWheel;
    Transform Body;
    TrailRenderer BackLeftSkidMark;
    TrailRenderer BackRightSkidMark;
    public static ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;
    Rigidbody rigid;
    Vector3[] WayPoint = new Vector3[10];

    bool isCrossing;
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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            SetGear(1);
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            SetGear(0);
        }
        DecreaseRhythmPower();
        Accelerate();
        SteeringWheel();
    }
    void InitReference() // ���� ������ �ϴ� �Լ�
    {
        //���߿� ���⼭ �ΰ����ʱ�ȭ VO �޾Ƽ� ����
        currentGear = 1;
        maxSpeed = gearRatio[currentGear - 1];
        roadCount = 6;
        maxRhythmPower = 100;
        rhythmPower = maxRhythmPower;
        boosterMaxGauge = 100;
        boosterSpeed = 100f;
        Body = transform.Find("Body").GetComponent<Transform>();
        FrontLeftWheel = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("FLW").GetComponent<Transform>();
        FrontRightWheel = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("FRW").GetComponent<Transform>();
        BackRightSkidMark = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("RRW").transform.Find("BackRightSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        BackLeftSkidMark = transform.Find("Body").Find("Wheels").transform.Find("Meshes").transform.Find("RLW").transform.Find("BackLeftSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        FailVFx = transform.Find("FailFX").GetComponent<ParticleSystem>();
        SuccessVFx = transform.Find("SuccessFX").GetComponent<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();
        if(roadCount%2==0) // ������ ¦��
        {
            currentDirection = roadCount / 2;
            transform.position = new Vector3(0f, 1.2f, -8 * currentDirection);
        }
        else // ������ Ȧ��
        {
            currentDirection = roadCount / 2; 
            transform.position = new Vector3(0f, 1.2f, -8 * currentDirection);
        }
    }

    IEnumerator Booster()
    {
        while(isBoosting)
        {
            print("�ν�����");
            boosterCurrentGauge -= Time.deltaTime*10f;  // �ν��Ͱ������� 100�̶�� 10�� �� 0�ȴ�
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

    void DecreaseRhythmPower()
    {
        rhythmPower -= Time.deltaTime;
        GameManager.Instance.RhythmPower.value = rhythmPower;
        GameManager.Instance.RhythmPower.maxValue = maxRhythmPower;
        if(rhythmPower<=0)
        {
            //���ӿ���
        }
    }

    void DelayRotate()
    {
        //Body.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,90, transform.eulerAngles.z),3f);
    }
    public void MoveLeft()
    {
        if (currentSpeed != 0)
        {
            if (currentDirection != 0)
            {
                currentDirection--;
                Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -1, 0f)), 10f);
                Invoke("DelayRotate", 0.1f);
            }
        }
    }
    public void MoveRight()
    {
        if (currentSpeed != 0)
        {
            if (currentDirection < roadCount - 1)
            {
                currentDirection++;
                Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 1, 0f)), 10f);
                Invoke("DelayRotate", 0.1f);
            }

        }
    }

    void Accelerate() // ������ �������ִ� �Լ�
    {
        transform.Translate(Vector3.forward*currentSpeed*Time.deltaTime);
    }
    void SteeringWheel()
    {
        for (int i = 0; i < roadCount; i++) //�̵��� ������ ��ġ ������Ʈ
        {
            WayPoint[i] = new Vector3(transform.position.x, transform.position.y, i * -8f);
        }
        transform.position = Vector3.MoveTowards(transform.position, WayPoint[currentDirection], 50 * Time.deltaTime);

        //���� �޽� ������ ����
        FrontLeftWheel.localRotation = Quaternion.Euler(FrontLeftWheel.localRotation.eulerAngles.x, (turnInput * maxSteering), FrontLeftWheel.localRotation.eulerAngles.z);
        FrontRightWheel.localRotation = Quaternion.Euler(FrontRightWheel.localRotation.eulerAngles.x, turnInput * maxSteering, FrontRightWheel.localRotation.eulerAngles.z);
    }

    public void SpeedUP()
    {
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

    public void SetGear(int direction)
    {
        if(direction>0) // ���� && ����
        {
            if(currentGear < gearRatio.Count&&currentSpeed>=maxSpeed)
            {
                currentGear++;                
            }
            maxSpeed = gearRatio[currentGear-1];
        }
        else // �Ʒ��� && ���ٿ�
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
    void DelayDriftFX()
    {
        BackLeftSkidMark.emitting = false;
        BackRightSkidMark.emitting = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Booster"))
        {
            isBoosting = true;
            boosterCurrentGauge = boosterMaxGauge;
            StartCoroutine(Booster());
        }
    }
}