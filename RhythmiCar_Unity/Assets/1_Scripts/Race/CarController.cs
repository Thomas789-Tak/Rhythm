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
    public float currentSpeed;
    public float maxSpeed;
    float maxSteering=45;
    float turnInput; // �¿� ����Ű �Է°�
    public bool isBoosting { get; set; }

    int myDirection;
    int roadCount;
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
    void InitReference() // ���� ������ �ϴ� �Լ�
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
        if(roadCount%2==0) // ������ ¦��
        {
            myDirection = roadCount / 2;
            transform.position = new Vector3(0f, 1.2f, -8*myDirection);
        }
        else // ������ Ȧ��
        {
            myDirection = roadCount / 2;
            transform.position = new Vector3(0f, 1.2f, -8 * myDirection);
        }
    }
    void GetInputType() // (2) �÷��̾��� ��ǲ�� �޾ƿ��� �Լ�
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
            //���ӿ���
        }
    }

    void DelayRotate()
    {
        //Body.rotation = Quaternion.Euler(transform.eulerAngles.x, 90, transform.eulerAngles.z);
        Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,90, transform.eulerAngles.z),3f);
    }
    void PcController()
    {
        //�¿� ȸ��
        if (currentSpeed != 0)
        {
            if(myDirection<roadCount-1&&Input.GetKeyDown(KeyCode.D)) // ������ �̵�
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

        //���俵������ ȣ���� �� �ְ�
        //����
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
        //�ν���
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

    void Accelerate() // ������ �������ִ� �Լ�
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
        else
        {
            currentSpeed = maxSpeed;
        }
    }
    public void GearUp()
    {

    }
    public void GearDown() // �� �ٿ� ��Ű�� �Լ�
    {
        SoundManager.Instance.PlayLoopSFX(SoundManager.ESFX_Loop.CarDriftSound);
        BackLeftSkidMark.emitting = true;
        BackRightSkidMark.emitting = true;
        //������
        BackLeftSkidMark.emitting = false;
        BackRightSkidMark.emitting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            print("����");
        }
    }
}