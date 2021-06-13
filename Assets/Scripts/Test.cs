using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public AudioSource beat;
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    beat.Play();
        //}
    }
    private void FixedUpdate()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("¥Í¿Ω");
        if (collision.CompareTag("Note"))
        {
            print("¥Í¿Ω");
        }
    }
}
