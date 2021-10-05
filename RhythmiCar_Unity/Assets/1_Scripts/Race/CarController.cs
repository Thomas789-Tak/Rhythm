using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.UI;
using DG.Tweening;

public class CarController : MonoBehaviour
{
    //�����ؾ��� ���� : �ν��� ���� �� ������� ó�� X  ,
    [Header("���� ����")]
    [SerializeField]
    List<SoundManager.EBGM> songEquipList = new List<SoundManager.EBGM>();
    ///�뷱�� ���� Range ���� �����
    [SerializeField] [Tooltip("���ӵ�")] [Range(1, 30)] float acceleration;
    [SerializeField] [Tooltip("�ν��ͼӵ�")] [Range(10, 50)] float boosterSpeed;
    [SerializeField] [Tooltip("���뿡����")] [Range(30, 300)] float maxRhythmEnergy;
    [SerializeField] [Tooltip("�¿� �̵��ӵ�")] [Range(1, 6)] float turnSpeed;

    /// -------------���� ����   ��!
    [SerializeField] [Tooltip("�ִ� �ν��Ͱ�������")] const float boosterMaxGauge=5f;

    float boosterCurrentGauge;
    [SerializeField] float currentRhythmEnergy;
    [SerializeField] float currentSpeed;
    [SerializeField] float maxSpeed;
    public bool isBoosting { get; set; }

    int currentDirection;
    int roadCount;
    int combo;
    List<float> gearRatio = new List<float>();
    [SerializeField]int currentGear;
    //���� ����
    public Transform Body;
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
        //GameManager.Instance.car = GetComponent<CarController>();
    }
    //--------------------------------------------------update----------------------------------------------------------------------
    void Update()
    {
        DecreaseRhythmEnergy();
        Accelerate();
        SteeringWheel();
        ObserveGearState();
        //GameManager.Instance.text.text = currentSpeed.ToString("N0") + "Km/h"+ " ������ ["+currentGear.ToString()+"]";
        UIManager.Instance.SetCarInformation(currentSpeed, currentGear);
        UIManager.Instance.SetCurrentRhythmEnergy(currentRhythmEnergy);
    }
    //----------------------------------------------------update--------------------------------------------------------------------

    void InitReference() // ���� ������ �ϴ� �Լ�
    {
        ///���߿� ���⼭ �ΰ����ʱ�ȭ VO �޾Ƽ� ���� �����
       
        //maxRhythmPower = 10;
        //boosterMaxGauge = 7;
        //boosterSpeed = 10f;

        /// -----------------�������   





        roadCount = 6; //�ε�ī��Ʈ �����Ŵ������� ����
        currentRhythmEnergy = maxRhythmEnergy;
        currentGear = 1;
        maxSpeed = gearRatio[currentGear - 1];
        Body = GameObject.FindWithTag("Car").GetComponent<Transform>();
        BackRightSkidMark = transform.Find("BackRightSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        BackLeftSkidMark = transform.Find("BackLeftSkid").GetComponent<Transform>().GetComponent<TrailRenderer>();
        FailVFx = transform.Find("FailFX").GetComponent<ParticleSystem>();
        SuccessVFx = transform.Find("SuccessFX").GetComponent<ParticleSystem>();
        rigid = GetComponent<Rigidbody>();
        if(roadCount%2==0) // ������ ¦��
        {
            currentDirection = roadCount / 2;
            transform.position = new Vector3(0f, 0f, -8 * currentDirection);
        }
        else // ������ Ȧ��
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
                    combo++;
                    //SpeedUP();                    
                    break;

                case EJudge.Good:
                    combo++;
                    //SpeedUP();
                    break;

                case EJudge.Bad:
                    combo = 0;
                    //SpeedDown(EBrakeCase.bad);
                    break;

                case EJudge.Miss:
                    combo =0;
                    //SpeedDown(EBrakeCase.miss);
                    break;
            }

        }
        UIManager.Instance.SetNoteCombo(combo);
        
    }

    public void GetItem(EnumItemVO.EItemType itemType, int goldAmount=0 , int noteAmount=0, int starAmount=0, float rhythmEnergyAmount=0 )
    {
        //������� ��ƼŬ ������
        switch (itemType)
        {
            case EnumItemVO.EItemType.gold:

                GameManager.Instance.Gold += goldAmount;

                break;

            case EnumItemVO.EItemType.note:

                GameManager.Instance.Note += noteAmount;

                break;

            case EnumItemVO.EItemType.rhythmEnergy:

                if (currentRhythmEnergy < maxRhythmEnergy)
                {
                    currentRhythmEnergy += rhythmEnergyAmount;
                }
                if (currentRhythmEnergy > maxRhythmEnergy)
                {
                    currentRhythmEnergy = maxRhythmEnergy;
                }

                break;

            case EnumItemVO.EItemType.star:

                GameManager.Instance.Star += starAmount;

                break;

            case EnumItemVO.EItemType.booster:
                
                boosterCurrentGauge = boosterMaxGauge;
                if (isBoosting==false)
                {
                    StartCoroutine(Co_Booster);
                }

                break;
        }
    }

    public IEnumerator Booster() // �ν��͸� Ȱ��ȭ �ϴ� �Լ�
    {
        isBoosting = true;
        while (isBoosting)
        {
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

    void DecreaseRhythmEnergy() // �ڵ������� ���Ḧ ��� �ϴ� �Լ�
    {
        if(currentRhythmEnergy<=0)
        {
            //���ӿ���
            currentRhythmEnergy = 0f;
        }
        else
        {
            currentRhythmEnergy -= Time.deltaTime;
        }
    }
    void ObserveGearState() // ����ӵ��� ���� �� ���ߴ� ������ Ž���ϴ� �Լ� 
    {
        if(currentGear>2) // 2�� ���� ���� �� 
        {
            if (currentSpeed <= gearRatio[currentGear - 2] - 10f)
            {
                currentSpeed = (gearRatio[currentGear - 2] + gearRatio[currentGear - 3]) * 0.5f;
                currentGear--;
                maxSpeed = gearRatio[currentGear-1];
            }
        }
        else if(currentGear==2)
        {
            if (currentSpeed <= gearRatio[currentGear - 2] - 10f)
            {
                currentSpeed = gearRatio[currentGear - 2] * 0.5f;
                currentGear--;
                maxSpeed = gearRatio[currentGear-1];
            }

        }
    }

    void DelayRotate() // ȸ�� �� �����̸� �༭ ������ �������� ������Ű�� �Լ�
    {
        //Body.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles.x,90, transform.eulerAngles.z),3f);
        Body.transform.DOLocalRotate(Vector3.zero, 0.3f);
    }

    public void MoveDirection(int direction)
    {
        if(direction==1) // ������
        {
            if (currentDirection < roadCount - 1)
            {
                currentDirection++;
                Body.transform.DOLocalRotate(new Vector3(0, 5f, 0), 0.3f);

                Invoke("DelayRotate", 0.3f);
            }
        }
        else //����
        {
            if (currentDirection != 0)
            {
                currentDirection--;
                Body.transform.DOLocalRotate(new Vector3(0, -5f, 0), 0.3f);

                Invoke("DelayRotate", 0.3f);
            }
        }
    }

    void Accelerate() // ������ ����ӵ��� �޾ƿ� ���������� �����̰� �ϴ� �Լ�
    {
        transform.Translate(Vector3.forward*currentSpeed*Time.deltaTime);
    }
    void SteeringWheel() // �¿� �̵� Ű �Է��� �޾� �ش��ϴ� �������� �̵��ϴ� �Լ�
    {
        for (int i = 0; i < roadCount; i++) //�̵��� ������ ��ġ ������Ʈ
        {
            WayPoint[i] = new Vector3(transform.position.x, 0f, i * -8f);
        }
        //transform.position = Vector3.MoveTowards(transform.position, WayPoint[currentDirection], turnSpeed * Time.deltaTime);
        transform.DOMoveZ(WayPoint[currentDirection].z, turnSpeed, false).SetEase(Ease.OutExpo);
        
    }

    public void SpeedUP() // ��Ʈ ���� �� �ӵ��� �÷��ִ� �Լ� --- �� �κ��� ���� ����� ���õ� �Լ��� �ٲ���
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


    public void SpeedDown(EBrakeCase result) // ��Ʈ ���� �� �ӵ��� �����ִ� �Լ�
    {
        if(isBoosting==false)
        {
            FailVFx.Play();
            if (currentSpeed >= 0 && result == EBrakeCase.bad)
            {
                currentSpeed -= 5f;
            }
            else if (currentSpeed >= 0 && result == EBrakeCase.miss)
            {
                currentSpeed -= 10f;
            }
            else if (currentSpeed >= 0 && result == EBrakeCase.bump)
            {
                currentSpeed -= 30f;

            }
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }
        }

    }

    public void Bump()  // �ٸ� ������Ʈ �ε��� �� ���� X, �ٸ� ������Ʈ�� �ε�ĥ �� ���� ������ ����
    {
        if(isBoosting)
        {
            
        }
        else
        {
            //ī�޶� ����ũ
            //���� ȸ���ϴ� ����
            // ����Ʈ �߻�
            //�ӵ� ����
            // ���� ������ ����
        }
    }

    public void SetGear(int direction) // �� �ø��ų� ������ �Լ�
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
    void DelayDriftFX() // �帮��Ʈ FX�� �����̸� �� �� ���ִ� �Լ�
    {
        BackLeftSkidMark.emitting = false;
        BackRightSkidMark.emitting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            print("�¸�");
        }
    }
}