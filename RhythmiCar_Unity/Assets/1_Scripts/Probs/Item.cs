using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    [SerializeField] EnumItemVO.EItemType enumType;
    [SerializeField] int goldAmount;
    [SerializeField] int noteAmount;
    [SerializeField] int starAmount;
    [SerializeField] float rhythmEnergyAmount;

    private void Start()
    {
        transform.DORotate(Vector3.up*180, 1f).SetLoops(-1).SetEase(Ease.Linear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<CarController>().GetItem(enumType,goldAmount,noteAmount,starAmount,rhythmEnergyAmount);

            gameObject.SetActive(false);
        }
    }
}
