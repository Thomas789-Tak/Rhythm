using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    float rotation;

    void Update()
    {
        rotation += Time.deltaTime * 60f;
        transform.DORotate(Vector3.up*rotation, 1f).SetLoops(-1).SetEase(Ease.Linear);
    }
}
