using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public InputField InputFieldCheckDistance;
    public bool isTouch;
    private static InputController instance = null;
    public static InputController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InputController>();

                if (instance == null)
                {
                    Debug.LogError("SoundManager singelton Error");
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    //public TextMeshProUGUI TextInfor;
    private Vector2 a;
    public float checkDistance = 10f;

    public UnityEvent aaa;

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("Pointer Down : " + eventData.position);
        //TextInfor.text = "Down \n" + eventData.position;
        a = eventData.position;
        isTouch = true;
        print(isTouch);
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
        isTouch = false;
        print(isTouch);
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
                GameManager.Instance.car.MoveLeft();
            }    
            else
            {
                Debug.Log("Move To Right");
                GameManager.Instance.car.MoveRight();
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

    public void SetCheckDistance()
    {
        float.TryParse(InputFieldCheckDistance.text, out checkDistance);
    }

}
