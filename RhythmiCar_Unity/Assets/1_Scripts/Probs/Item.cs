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
        if(enumType.Equals(EnumItemVO.EItemType.booster)|| enumType.Equals(EnumItemVO.EItemType.rhythmEnergy))
        {
            transform.rotation = Quaternion.Euler(0, 0, 30);
        }
    }
    private void Update()
    {
        print("È¸Àü");
        transform.Rotate(Vector3.up * 100f * Time.deltaTime,Space.World);
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
