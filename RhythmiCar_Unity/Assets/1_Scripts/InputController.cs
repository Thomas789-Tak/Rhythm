using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class InputController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private InputManager InputManager;
    private RhythmJudge RhythmJudge;
    private Vector2 preVec;

    public float checkDistance = 9000f;
    public InputField InputFieldCheckDistance;

    private void Start()
    {
        InputManager = InputManager.Instance;
        RhythmJudge = GetComponent<RhythmJudge>();
    }

    private void Update()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        preVec = eventData.position;
        RhythmJudge.Judge();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        preVec -= eventData.position;
        DragCheck();
    }

    public void DragCheck()
    {
        var x = Mathf.Pow(preVec.x, 2);
        var y = Mathf.Pow(preVec.y, 2);

        if (x < checkDistance && y < checkDistance)
        {
            //Debug.Log("Just Touch");
            //RhythmJudge.Judge();
            return;
        }
        // X�� �巡�� ����
        else if (x >= y)    
        {
            if (preVec.x > 0)
            {
                // �·� �����̵�
                InputManager.horizontalEvent.Invoke(-1);
            }    
            else
            {
                // ��� �����̵�
                InputManager.horizontalEvent.Invoke(1);
            }
        }
        // Y�� �巡�� ����
        else
        {
            if (preVec.y < 0)
            {
                // ���� �����̵�
                InputManager.verticalEvent.Invoke(1);
            }
            else
            {
                // �Ʒ��� �����̵�
                InputManager.verticalEvent.Invoke(-1);
            }
        }
    }

    public void SetCheckDistance()
    {
        float.TryParse(InputFieldCheckDistance.text, out checkDistance);
    }
}
