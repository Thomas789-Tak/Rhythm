using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //public TextMeshProUGUI TextInfor;
    private Vector2 a;
    public float checkDistance = 0f;


    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Pointer Down : " + eventData.position);
        //TextInfor.text = "Down \n" + eventData.position;
        a = eventData.position;

    }



    public void OnPointerUp(PointerEventData eventData)
    {
        a -= eventData.position;
        Debug.Log(a);
        //Debug.Log
        //    ("Pointer UP : " + eventData.position + "\n" +
        //    "Distance = " + b);
        //TextInfor.text = "Up \n" + eventData.position;
        DragCheck();
    }

    public void DragCheck()
    {
        var x = Mathf.Pow(a.x, 2);
        var y = Mathf.Pow(a.y, 2);

        if (x < checkDistance && y < checkDistance)
        {
            Debug.Log("Just Touch");
        }
        // X축 드래그 판정
        else if (x >= y)    
        {
            if (a.x > 0)
            {
                Debug.Log("Move To Left");
            }    
            else
            {
                Debug.Log("Move To Right");
            }
        }
        // Y축 드래그 판정
        else
        {
            if (a.y < 0)
            {
                Debug.Log("Move To up");
            }
            else
            {
                Debug.Log("Move To Down");
            }
        }


    }

}
