using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region DataDictionary

    public Dictionary<string, Car> carData;// = new Dictionary<string, Car>();
    


    #endregion

    void Start()
    {
        List<Dictionary<string, object>> data = CSVReader.Read("Data/cardata");

        //print(data[0]["id"]);
    }

    [ContextMenu("ReadData")]
    void ReadCarData()
    {
        string carDataPath = "Data/CarData";
        int kindOfCar = System.Enum.GetValues(typeof(ECarKind)).Length;
        int totalLevel = 10;

        List<Dictionary<string, object>> data = CSVReader.Read(carDataPath);
        carData = new Dictionary<string, Car>();

        for (int i = 0; i < kindOfCar; i++)
        {

            Car newCar = new Car();
            newCar.statuses = new List<Car.CarStatus>();
            newCar.name = ((ECarKind)i).ToString();
            newCar.currentLevel = 0;

            
            for (int lv = 0; lv < totalLevel; lv++)
            {
                Car.CarStatus newCarStatus = new Car.CarStatus();
                int num = i * totalLevel + lv;

                newCarStatus.songEquipCount = (int)data[num][nameof(Car.CarStatus.songEquipCount)];
                newCarStatus.level = (int)data[num][nameof(Car.CarStatus.level)];
                newCarStatus.levelUpCost = (int)data[num][nameof(Car.CarStatus.levelUpCost)];
                newCarStatus.acceleration = (float)data[num][nameof(Car.CarStatus.acceleration)];
                newCarStatus.boosterMaxGauge = (float)data[num][nameof(Car.CarStatus.boosterMaxGauge)];
                newCarStatus.turnStrength = (float)data[num][nameof(Car.CarStatus.turnStrength)];
                newCarStatus.brakeForce = (float)data[num][nameof(Car.CarStatus.brakeForce)];
                newCarStatus.friction = (float)data[num][nameof(Car.CarStatus.friction)];
                newCarStatus.rhythmPower = (float)data[num][nameof(Car.CarStatus.rhythmPower)];

                newCar.statuses.Add(newCarStatus);
            }

            carData.Add(newCar.name, newCar);
        }

    }

    [ContextMenu("LoadData")]
    public void LoadData()
    {
        Debug.Log(carData[ECarKind.Red.ToString()].statuses[2].songEquipCount);
    }


}
