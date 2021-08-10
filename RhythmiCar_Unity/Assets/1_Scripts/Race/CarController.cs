using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
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

    [Header("���� ������")]
    [SerializeField] int myID;
    [SerializeField] string myVehicleName;

    [Header("���� ����")]
    int myMusicEquipCount;
    [SerializeField] [Tooltip("���ӵ�")] [Range(1, 100)] float myAcceleration;
    [SerializeField] [Tooltip("�ִ� �ν��Ͱ�������")] [Range(1, 300)] float myBoosterMaxGauge;
    [SerializeField] [Tooltip("�ν��ͼӵ�")] [Range(1, 300)] float myBoosterSpeed;
    [SerializeField] [Tooltip("ȸ���ӵ�")] [Range(1, 300)] float myTurnStrength =30f;
    [SerializeField] [Tooltip("������")] [Range(1, 300)] float myBrakeForce = 50f;
    [SerializeField] [Tooltip("����������")] [Range(1, 300)] float myFriction =40f;
    float myRhythmPower;
    public float myCurrentSpeed;
    float myMaxSteering=45;
    float myBoosterCurrentGauge;
    public float maxSpeed;

    float gravityForce =2000f; // �߷�
    float groundRayLength = 1.5f; // �ٴ�üũ ���� ����
    float groundDrag = 3f; //���󿡼��� ��������
    float groundAngularDrag = 2f;
    float turnInput; // �¿� ����Ű �Է°�
    float turnLerpSpeed=160f; // ���� �� �ε巯�� ����
    float HillLerpSpeed=10f; // ����ö� �� �ε巯�� ����
    float driftTime;

    bool isGrounding; //�������� üũ
    bool isDrifting; // �帮��Ʈ������ üũ
    bool isBoosting; // �ν��������� üũ
    bool isTouchWheel;


    int GroundLayer;
    int hillLayer;

    void Awake()
    {
        InitRigidBody(); // (1)
        Kick = GameObject.Find("Kick").GetComponent<Scrollbar>();
        SteeringWheel = GameObject.Find("SteeringWheel").GetComponent<Scrollbar>();
    }

    // (1) ������ٵ� + ������ݶ��̴� �ʱ�ȭ�ϴ� �Լ�
    void InitRigidBody()
    {
        /// �ʱ�ȭ �Ҷ� mass ������ ������ ���������� ���� ���� awake �� ȣ������ �ʰ� ���� 
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
        Profiler.BeginSample("�÷��̾� ��ǲ");
        GetInput(); // (2)
        Profiler.EndSample();
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

    // (5) ���� �� �ݶ��̴� ��Ȱ��ȭ
    void SetColliderWhenFly()
    {
        //Invoke("DelaySetCollider", 2f);
        if(transform.position.y<=-19f)
        {
            GameManager.Instance.Retry.gameObject.SetActive(true);
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

    // (2) �÷��̾��� ��ǲ�� �޾ƿ��� �Լ�
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

        //���� ��ġ => ������ٵ� ��ġ�� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            myCurrentSpeed += myAcceleration;
        }

        //�帮��Ʈ
        if(driftTime>=0.2f && isGrounding&&myCurrentSpeed>30f)
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

    // (3) ������ ������ �����ϴ� �Լ�
    void CarEngine()
    {
        //�ٴ�üũ �Լ�
        if (isGrounding&&isDrifting==false)
        {       
            //����
            MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f,ForceMode.Force);
            MyRigidBody.drag = groundDrag;
        }
        else if(isDrifting)
        {
            MyRigidBody.drag = 0f;
            MyRigidBody.angularDrag = 0f;
            MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f*0.01f, ForceMode.Force);
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
            float referenceDrift=myCurrentSpeed*0.4f;
            //������
            if (Input.GetAxis("Horizontal") > 0)
            {
                //���� �ӵ��� �����ؼ� ����  
                /// ����ǲ �ޱ�
                MyRigidBody.AddForce(Vector3.Slerp(-transform.right, -transform.right * myFriction*referenceDrift*turnInput, 500f * Time.deltaTime), ForceMode.Force);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 2f*turnInput * myTurnStrength * 0.5f * Time.deltaTime, 0f)), turnLerpSpeed * Time.deltaTime);
                if (myCurrentSpeed > 0)
                {
                    myCurrentSpeed -= myBrakeForce * referenceDrift * Time.deltaTime*0.5f;
                }
            }
            else if(Input.GetAxis("Horizontal")==0)
            {
                if(myCurrentSpeed > 0)
                {
                    myCurrentSpeed -= myBrakeForce * referenceDrift * Time.deltaTime;
                }
            }
            //����
            else
            {
                if (myCurrentSpeed > 0)
                {
                    MyRigidBody.AddForce(Vector3.Slerp(transform.right, transform.right * myFriction * referenceDrift * Mathf.Abs(turnInput), 500f * Time.deltaTime), ForceMode.Force);
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 2f * turnInput * myTurnStrength * 0.5f * Time.deltaTime, 0f)), turnLerpSpeed * Time.deltaTime);
                    myCurrentSpeed -= myBrakeForce * referenceDrift * Time.deltaTime*0.5f;
                }


            }

            //��ƼŬ �־��ֱ�

        }
    }

    //�ٴ�üũ �Լ� + ���� ����
    void CheckGround()
    {
        RaycastHit groundInfo;
        if (Physics.Raycast(GroundCheckPos.position, -transform.up, out groundInfo, groundRayLength, GroundLayer))
        {
            isGrounding = true;
            //������ �ִ� �������� ������ ������ �������ִ� �κ�
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, groundInfo.normal) * transform.rotation, Time.deltaTime * HillLerpSpeed);
            //�¿� �̵�
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
