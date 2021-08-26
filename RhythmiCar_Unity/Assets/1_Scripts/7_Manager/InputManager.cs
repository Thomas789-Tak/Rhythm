using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    [SerializeField] public UnityEvent<EJudge> touchEvent;
    [SerializeField] public UnityEvent<int> verticalEvent;
    [SerializeField] public UnityEvent<int> horizontalEvent;

    private void Start()
    {
        touchEvent.AddListener((EJudge) => Debug.Log(EJudge));
        verticalEvent.AddListener((inta) => Debug.Log("Vertical + " + inta));
        horizontalEvent.AddListener((inta) => Debug.Log("Vertical + " + inta));

    }
}


public enum EJudge
{
    Miss = 0,
    Bad,
    Good,
    Perfect
}