using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICarMove : MonoBehaviour
{
    public Rigidbody rigid;
    public float speed;
    bool takeDown;
    public float rot;
    public int type;

    void Start()
    {
    }


    void Update()
    {
        if (type == 0)
        {
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //other.GetComponent<CarController>().SpeedDown(EBrakeCase.bump);
        }
    }
}
