using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Image LImage;
    public Image RImage;
    float a;
    private void Update()
    {
        print(a);
    }
    private void FixedUpdate()
    {
        if(LImage.rectTransform.position.x>=0)
        {
            LImage.rectTransform.position = new Vector2(LImage.rectTransform.position.x, LImage.rectTransform.position.y);
            a += Time.deltaTime;
        }
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
