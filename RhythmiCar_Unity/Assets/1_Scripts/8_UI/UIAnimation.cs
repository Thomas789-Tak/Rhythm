using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIAnimation : MonoBehaviour
{
    public Transform trans;
    public RectTransform movedRect;
    public RectTransform sizingRext;

    public float aniTime = 0.5f;
    public Ease ease;

    public bool isOn;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlideUI()
    {
        if (!isActive)
        {
            isActive = true;

            if (!isOn)
                movedRect.DOMoveX(sizingRext.rect.width, aniTime)
                    .SetRelative(true)
                    .SetEase(ease)
                    .onComplete += () =>
                    {
                        isActive = false;
                        isOn = true;
                        Debug.Log("On");
                    };
            else
                movedRect.DOMoveX(-sizingRext.rect.width, aniTime)
                    .SetRelative(true)
                    .SetEase(ease)
                    .onComplete += () =>
                    {
                        isActive = false;
                        isOn = false;
                        Debug.Log("Off");
                    };
        }

    }
}
