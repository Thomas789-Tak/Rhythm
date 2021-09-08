using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="CarDataContainer", menuName = "Scriptable Object/Car Data Container")]
[System.Serializable]
public class CarDataContainer : ScriptableObject
{
    public string carName;
    public Sprite ImageCar;
    public int level;
}