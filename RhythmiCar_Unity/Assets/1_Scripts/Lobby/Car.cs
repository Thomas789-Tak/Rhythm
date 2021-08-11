using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Car
{
    public string name;
    public int currentLevel;
    
    public List<Status> statuses = new List<Status>();

    [System.Serializable]
    public class Status
    {
        public int songEquipCount;

        public int level;
        public float levelUpCost;

        public float acceleration;
        public float boosterMaxGauge;
        public float boosterSpeed;
        public float turnStrength;
        public float brakeForce;
        public float friction;
        public float rhythmPower;
    }
}

/// <summary>
/// 차량의 종류 (이름)
/// </summary>
public enum ECarKind
{
    Red = 0,
    Black = 1,
    Purple = 2,
    Yellow = 3,
}