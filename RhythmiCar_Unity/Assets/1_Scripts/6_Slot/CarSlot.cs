using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CarSlot : Slot<Car>
{
    public Text TextCarName;
    public Text TextCarOpend;
    

    public override void SetContent(Car car)
    {
        base.SetContent(car);

        TextCarName.text = car.Name;
    }
    
    
}