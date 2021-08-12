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
    TrailRenderer LeftSkidMark;
    TrailRenderer RightSkidMark;
    public static ParticleSystem SuccessVFx;
    ParticleSystem FailVFx;
    Scrollbar Kick;
    Scrollbar SteeringWheel;

    [Header("���� ������")]
    [SerializeField] int myID;
    [SerializeField] string myVehicleName;

    [Header("���� ����")]
    [SerializeField]
    List<SoundManager.EBGM> mySongEquipList = new List<SoundManager.EBGM>();
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

    void InitRigidBody() // (1) ������ٵ� + ������ݶ��̴� �ʱ�ȭ�ϴ� �Լ�
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
        CheckGroundAndRotation();
        CarEngine();

    }

    void Update()
    {
        GetInput();
        UpdateObjectTransform();
        UpdateCurrentState();
    }
   
    void UpdateObjectTransform()  // (4) ���� ������Ʈ ��ġ�� �Ҵ��ϴ� �Լ�
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

    void UpdateCurrentState() // (6) ���� ���¸� ������Ʈ �ϴ� �Լ�
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

    void SetColliderWhenFly() // (5) ���� �� �ݶ��̴� ��Ȱ��ȭ
    {
        //Invoke("DelaySetCollider", 2f);
        if (transform.position.y<=-19f) // �й����� ����; 
        {
            GameManager.Instance.Retry.gameObject.SetActive(true);
            Time.timeScale = 0;

        }

    }
    void DelaySetCollider()
    {
        CarColliderPos.gameObject.SetActive(true);
    }


    void GetInput() // (2) �÷��̾��� ��ǲ�� �޾ƿ��� �Լ�
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
            if(SoundManager.Instance.sfx.isPlaying==false)
            {
                SoundManager.Instance.PlayOneShotSFX(SoundManager.ESFX.CarDriftSound); //��������
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

    void CarEngine() // ������ ������ �����ϴ� �Լ�
    {
        if (isGrounding&&isDrifting==false) // ��� �� ����
        {       
            MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f,ForceMode.Force);
            MyRigidBody.drag = groundDrag;
            MyRigidBody.angularDrag = groundAngularDrag;
        }
        else if(isDrifting) // �帮��Ʈ ��
        {
            MyRigidBody.drag = 1f;
            MyRigidBody.angularDrag = 1f;
            // ���� MyRigidBody.AddForce(transform.forward * myCurrentSpeed * 1000f*0.01f, ForceMode.Force);
            MyRigidBody.AddForce(transform.forward* myCurrentSpeed * 1000f*0.5f, ForceMode.Force);
        }
    }

    void CheckGroundAndRotation() //�ٴ�üũ �Լ� + ���� ����
    {
        RaycastHit groundInfo;
        if (Physics.Raycast(GroundCheckPos.position, -transform.up, out groundInfo, groundRayLength, GroundLayer))
        {
            isGrounding = true;
            
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, groundInfo.normal) * transform.rotation, HillLerpSpeed); //������ �ִ� �������� ������ ������ �������ִ� �κ�

            if (isDrifting) // �帮��Ʈ �� ȸ��
            {
                print("�帮��Ʈ ��");
                if(turnInput>0) // ������ ȸ�� ��
                {
                    print("������ �帮��Ʈ");
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength*myFriction*0.0006f, 0f)), turnLerpSpeed);
                    MyRigidBody.AddForce(Vector3.Slerp(-transform.right, -transform.right * myFriction * myCurrentSpeed*0.03f * turnInput, 10f), ForceMode.Force);
                    if (myCurrentSpeed > 0)
                    {
                        myCurrentSpeed -= myBrakeForce; // �����ʿ� ���� �ӵ��� ����ؼ� �Ʒ� 3�� ���� �극��ũ�� ���ؼ� ��ȹ������ ���� �ʿ�(���������� �ӵ��� ���ϰ��� ������ �ӵ��� ���ϰ���)
                    }
                    else
                    {
                        myCurrentSpeed = 0f;
                    }
                }
                else if(turnInput==0) // �߸� �� 
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
                else // ���� ȸ�� ��
                {
                    print("���� �帮��Ʈ");
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength*0.0006f*myFriction , 0f)), turnLerpSpeed);
                    MyRigidBody.AddForce(Vector3.Slerp(transform.right, transform.right * myFriction * myCurrentSpeed*0.03f * Mathf.Abs(turnInput), 10f), ForceMode.Force);
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
            else // �帮��Ʈ�� ���� �ʴ� ���� �¿� ȸ��
            {
                print("����");
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength*0.03f, 0f)), turnLerpSpeed);
            }
        }
        else // ������ �ƴҶ� ��Ȳ
        {
            isGrounding = false;
            MyRigidBody.drag = 0f;
            MyRigidBody.angularDrag = 0f;
            MyRigidBody.AddForce(gravityForce * -Vector3.up, ForceMode.Force);
            SetColliderWhenFly();
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
