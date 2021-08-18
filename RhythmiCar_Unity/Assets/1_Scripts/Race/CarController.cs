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

    [Header("���� ������")]
    [SerializeField] int myID;
    [SerializeField] string myVehicleName;

    [Header("���� ����")]
    [SerializeField]
    List<SoundManager.EBGM> mySongEquipList = new List<SoundManager.EBGM>();
    [SerializeField] [Tooltip("���ӵ�")] [Range(1, 100)] float myAcceleration;
    [SerializeField] [Tooltip("�ִ� �ν��Ͱ�������")] [Range(1, 300)] float myBoosterMaxGauge;
    [SerializeField] [Tooltip("�ν��ͼӵ�")] [Range(1, 300)] float myBoosterSpeed;
    [SerializeField] [Tooltip("ȸ���ӵ�")] [Range(1, 300)] float myTurnStrength;
    [SerializeField] [Tooltip("������")] [Range(1, 300)] float myBrakeForce;
    [SerializeField] [Tooltip("����������")] [Range(1, 300)] float myFriction;
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
    float optimizeRayCheckTime;

    bool isGrounding; //�������� üũ
    bool isDrifting; // �帮��Ʈ������ üũ
    public bool isBoosting { get; set; } // �ν��������� üũ


    int GroundLayer;
    int hillLayer;

    void Awake()
    {
        InitRigidBody(); // (1)
        SoundManager.Instance.PlayOneShotSFX(SoundManager.ESFX.EngineStartSound);
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

    void CheckRoadType() // ����ĳ��Ʈ üũ�ֱ⸦ �����Ͽ� ����ȭ
    {
        optimizeRayCheckTime += Time.deltaTime;
        if(optimizeRayCheckTime>=0.3f)
        {
            RaycastHit groundInfo;
            if (Physics.Raycast(GroundCheckPos.position, -transform.up, out groundInfo, groundRayLength, GroundLayer))
            {
                isGrounding = true;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(transform.up, groundInfo.normal) * transform.rotation, HillLerpSpeed); //������ �ִ� �������� ������ ������ �������ִ� �κ�
            }
            else
            {
                isGrounding = false;
            }
            optimizeRayCheckTime = 0f;
        }

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

    void PcController()
    {
        //�¿� ȸ��
        if (myCurrentSpeed != 0)
        {
            turnInput = Input.GetAxis("Horizontal");
        }
        else
        {
            turnInput = 0;
        }

        //�帮��Ʈ
        if (Input.GetKey(KeyCode.LeftShift))
        {
            driftTime += Time.deltaTime;
        }
        else
        {
            driftTime = 0;
        }
        //����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(myCurrentSpeed<=maxSpeed)
            {
                myCurrentSpeed += myAcceleration;
                if(myBoosterCurrentGauge<myBoosterMaxGauge)  // ����ϵ� ���� �ν��� ������ ���� ��
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
        //�ν���
        if(Input.GetKeyDown(KeyCode.LeftControl)&&myBoosterCurrentGauge>=myBoosterMaxGauge)
        {
            isBoosting = true;
        }
    }

    void MobileController()
    {
        //�¿� ȸ��
        if(myCurrentSpeed!=0)
        {
            turnInput = (GameManager.Instance.WheelScrollBar.value-0.5f)*2f;
        }
        else
        {
            turnInput = 0;
        }

        // ��ġ�� ���� ��� �߸� ��ġ�� ����
        if (GameManager.Instance.isTouchWheelUI == false)
        {
            GameManager.Instance.WheelScrollBar.value = 0.5f;
        }
        //�帮��Ʈ
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
    void UpdateCurrentState() // (6) ���� ���¸� ������Ʈ �ϴ� �Լ�
    {     
        //�帮��Ʈ
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

    void SetColliderWhenFly() // (5) ���� �� �ݶ��̴� ��Ȱ��ȭ
    {
        //Invoke("DelaySetCollider", 2f);
        if (transform.position.y<=-19f) // �й����� ����; 
        {
            GameManager.Instance.RetryButton.gameObject.SetActive(true);
            Time.timeScale = 0;

        }

    }
    void DelaySetCollider()
    {
        CarColliderPos.gameObject.SetActive(true);
    }


    void GetInput() // (2) �÷��̾��� ��ǲ�� �޾ƿ��� �Լ�
    {
        //MobileController();
        PcController();   
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
        if (isGrounding)
        {
            if (isDrifting) // �帮��Ʈ �� ȸ��
            {
                if(turnInput>0) // ������ ȸ�� ��
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
            else // �帮��Ʈ�� ���� �ʴ� ���� �¿� ȸ��
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * myTurnStrength*0.03f, 0f)), turnLerpSpeed);
            }
        }
        else // ������ �ƴҶ� ��Ȳ
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
