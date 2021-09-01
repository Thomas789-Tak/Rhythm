using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarMove : MonoBehaviour
{
    public Rigidbody rigid;
    public float speed;
    bool takeDown;
    public float rot;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();    
    }


    void Update()
    {
        transform.Translate(Vector3.forward *speed* Time.deltaTime);
        if(takeDown)
        {
            rot += 10*Time.deltaTime;
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z+rot);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            rigid.AddForce(Vector3.up * 10f+Vector3.right*30f,ForceMode.VelocityChange);
            takeDown = true;
            print("¥Í¿Ω");
        }
    }
}
