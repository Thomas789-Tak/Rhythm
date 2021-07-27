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
    Vector3 lastPosition;
    ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;
    public MusicController music;
    Scrollbar Kick;
    Scrollbar SteeringWheel;
    public Image GearReadyUI;

    [Header("���� ������")]
    [SerializeField] int myID;
    [SerializeField] string myVehicleName;
    [SerializeField] int myMusicEquipCount;

    [Header("���� ����")]
    [SerializeField] [Tooltip("���ӵ�")][Range(1,100)] float myAcceleration;
    [SerializeField] [Tooltip("�ִ� �ν��Ͱ�������")] [Range(1, 300)] float myBoosterMaxGauge;
    [SerializeField] [Tooltip("�ν��ͼӵ�")] [Range(1, 300)] float myBoosterSpeed;
    [SerializeField] [Tooltip("�ִ�ȸ������")] [Range(1, 300)] float myMaxSteering=45;
    [SerializeField] [Tooltip("ȸ���ӵ�")] [Range(1, 300)] float myTurnStrength =30f;
    [SerializeField] [Tooltip("������")] [Range(1, 300)] float myBrakeForce = 50f;
    [SerializeField] [Tooltip("����������")] [Range(1, 300)] float myFriction =40f;
    [SerializeField] [Tooltip("������")] [Range(1, 3)] public int myCurrentGear;
    [SerializeField] [Tooltip("����ν��Ͱ�����")] [Range(1, 300)] float myBoosterCurrentGauge;
    [SerializeField] [Tooltip("����ӵ�")] [Range(1, 300)] public float myCurrentSpeed;
    public float maxSpeed;
    float gearUpSpeed;
    float gearDownSpeed;
    float gearDefaultSpeed;

    float gravityForce =2000f; // �߷�
    float groundRayLength = 1.5f; // �ٴ�üũ ���� ����
    float groundDrag = 3f; //���󿡼��� ��������
    float groundAngularDrag = 2f;
    float turnInput; // �¿� ����Ű �Է°�
    float turnLerpSpeed=160f; // ���� �� �ε巯�� ����
    float HillLerpSpeed=10f; // ����ö� �� �ε巯�� ����
    public float driftTime;

    bool isGrounding; //�������� üũ
    bool isDrifting; // �帮��Ʈ������ üũ
    bool isBoosting; // �ν��������� üũ
    bool isTouchWheel;
    public bool isGearReady;
    public bool isGearChanging;


    int GroundLayer;
    int hillLayer;

    void Awake()
    {
        InitRigidBody(); // (1)
        Kick = GameObject.Find("Kick").GetComponent<Scrollbar>();
        GearReadyUI = Kick.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        SteeringWheel = GameObject.Find("SteeringWheel").GetComponent<Scrollbar>();
    }

    // (1) ������ٵ� + ������ݶ��̴� �ʱ�ȭ�ϴ� �Լ�
    void InitRigidBody()
    {
        myCurrentGear = 1;
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
        CarEngine(); // (3)
        SetColliderWhenFly(); // (5)        
    }

    void Update()
    {
        GetInput(); // (2)
        UpdateObjectTransform(); // (4)
        UpdateCurrentState(); // (6)
    }
    // (4) ���� ������Ʈ ��ġ�� �Ҵ��ϴ� �Լ�
    void UpdateObjectTransform()
    {
        //������ �ٵ� Ŀ���� ���� y���� �����ϸ� �ȴ�
        transform.position = new Vector3(MyRigidBody.transform.position.x, MyRigidBody.transform.position.y-0.3f, MyRigidBody.transform.position.z);
        //���� ���� => ������ٵ� ��ġ�� ����
        GroundCheckPos.position = MyRigidBody.transform.position;
        //���� �ݶ��̴� ��ġ ����
        CarColliderPos.position = transform.position;
        CarColliderPos.rotation = transform.rotation;
        //���� �޽� ������ ����
        FrontLeftWheel.localRotation = Quaternion.Euler(FrontLeftWheel.localRotation.eulerAngles.x, (turnInput * myMaxSteering), FrontLeftWheel.localRotation.eulerAngles.z);
        FrontRightWheel.localRotation = Quaternion.Euler(FrontRightWheel.localRotation.eulerAngles.x, turnInput * myMaxSteering, FrontRightWheel.localRotation.eulerAngles.z);

    }
    // (6) ���� ���¸� ������Ʈ �ϴ� �Լ�
    void UpdateCurrentState()
    {
        gearDownSpeed = maxSpeed-60;
        gearUpSpeed = maxSpeed-20;
        gearDefaultSpeed = maxSpeed;
        if(myCurrentSpeed>=gearUpSpeed&&myCurrentGear!=3)
        {
            isGearReady = true;
            GearReadyUI.gameObject.SetActive(true);
            if(isGearReady&&Kick.value>=0.7f)
            {
                if(myCurrentGear!=2)
                {
                    music.GearUp(myCurrentGear);
                    MusicManager.Instance.GearChange.Play();

                }
                myCurrentGear++;
            }
        }
        else
        {
            isGearReady = false;
            GearReadyUI.gameObject.SetActive(false);
            Kick.value = 0;
        }
        if(myCurrentSpeed<=gearDownSpeed&&myCurrentGear!=1)
        {
            if(myCurrentGear!=3)
            {
                music.GearDown(myCurrentGear);
                MusicManager.Instance.GearChange.Play();

            }
            myCurrentGear--;
        }
        if(isTouchWheel==false)
        {
            SteeringWheel.value = 0.5f;
        }
        if(music.isKickButtonDown)
        {
            driftTime += Time.deltaTime;
        }
        if(MusicManager.Instance.GearChange.isPlaying)
        {
            isGearChanging = true;
        }
        else
        {
            isGearChanging = false;
        }
    }

    // (5) ���� �� �ݶ��̴� ��Ȱ��ȭ
    void SetColliderWhenFly()
    {
        //�����밡 ���� �� �Ͻ������� ���� �ݶ��̴� ����
        RaycastHit hillInfo;
        float fowardLength=3f;
        if (Physics.Raycast(GroundCheckPos.position, transform.forward, out hillInfo, fowardLength, hillLayer))
        {
            CarColliderPos.gameObject.SetActive(false);
        }
        else if (isGrounding == false)
        {        
            Invoke("DelaySetCollider", 2f);
            if(transform.position.y<=-19f)
            {
                GameManager.Instance.Retry.gameObject.SetActive(true);
                MusicManager.Instance.ifI.Pause();
                MusicManager.Instance.wakeUp.Pause();
                Time.timeScale = 0;

            }
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

    // (2) �÷��̾��� ��ǲ�� �޾ƿ��� �Լ�
    void GetInput()
    {

        turnInput = (SteeringWheel.value-0.5f)*2f;
        //���� ��ġ => ������ٵ� ��ġ�� ����

        //�帮��Ʈ
        if(driftTime>=0.2f && isGrounding) //�ӵ��� �������Ϸ� �������� �帮��Ʈ �ȵǰ� ����  , ,�ӵ��� ���� ���� �ʿ�
        {
            isDrifting = true;
            if (myCurrentSpeed > 30f)
            {
                if (MusicManager.Instance.Skid.isPlaying == false)
                {
                    MusicManager.Instance.Skid.Play();
                }
                LeftSkidMark.emitting = true;
                RightSkidMark.emitting = true;
            }
            else
            {
                LeftSkidMark.emitting = false;
                RightSkidMark.emitting = false;

            }

        }
        else
        {
            isDrifting = false;
            LeftSkidMark.emitting = false;
            RightSkidMark.emitting = false;
        }
    }

    // (3) ������ ������ �����ϴ� �Լ�
    void CarEngine()
    {
        //�ٴ�üũ �Լ�
        if (isGrounding)
        {       
            //����
            MyRigidBody.AddForce(gravityForce * -Vector3.up + transform.forward * myCurrentSpeed * 1000f,ForceMode.Force);
            MyRigidBody.drag = groundDrag;
        }
        else
        {
            //���߿� ���� �� ó��
            MyRigidBody.drag = 0f;
            MyRigidBody.angularDrag = 0f;
            MyRigidBody.AddForce(gravityForce * -Vector3.up,ForceMode.Force);
        }

        //�帮��Ʈ
        if (isDrifting)
        {
            float referenceDrift=myCurrentSpeed*0.1f;
            //����
            if (Input.GetAxis("Horizontal") > 0)
            {
                //���� �ӵ��� �����ؼ� ����  
                /// ����ǲ �ޱ�
                MyRigidBody.AddForce(Vector3.Slerp(-transform.right, -transform.right * myFriction*referenceDrift, 500f * Time.deltaTime), ForceMode.Impulse);
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
            //������
            else
            {
                if (myCurrentSpeed > 0)
                {
                    MyRigidBody.AddForce(Vector3.Slerp(transform.right, transform.right * myFriction * referenceDrift, 500f * Time.deltaTime), ForceMode.Impulse);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength * 0.5f * Time.deltaTime, 0f)), turnLerpSpeed * Time.deltaTime);
                    myCurrentSpeed -= myBrakeForce * referenceDrift * Time.deltaTime;
                }


            }

            //��ƼŬ �־��ֱ�

        }
        else
        {
            //isDrifting = false;
        }
        CheckGround();
    }

    //�ٴ�üũ �Լ� + ���� ����
    void CheckGround()
    {
        isGrounding = false;
        RaycastHit groundInfo;
        if (Physics.Raycast(GroundCheckPos.position, -transform.up, out groundInfo, groundRayLength, GroundLayer))
        {
            isGrounding = true;
            //������ �ִ� �������� ������ ������ �������ִ� �κ�
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, groundInfo.normal) * transform.rotation, Time.deltaTime * HillLerpSpeed);
            //�¿� �̵�
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength * Time.deltaTime, 0f)), turnLerpSpeed*Time.deltaTime);
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
