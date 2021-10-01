using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Car
{
    [SerializeField] private string carName;
    [SerializeField] private ESelectCar kind;
    [SerializeField] private int currentLevel;

    [SerializeField] private List<Status> statuses = new List<Status>();

    [System.Serializable]
    public class Status
    {
        [SerializeField] private int songEquipCount;

        [SerializeField] private int level;
        [SerializeField] private float levelUpCost;

        [SerializeField] private float acceleration;
        [SerializeField] private float boosterMaxGauge;
        [SerializeField] private float boosterSpeed;
        [SerializeField] private float turnStrength;
        [SerializeField] private float brakeForce;
        [SerializeField] private float friction;
        [SerializeField] protected float rhythmPower;

        public Status(int songEquipCount, int level, float levelUpCost, float acceleration, float boosterMaxGauge, float boosterSpeed, float turnStrength, float brakeForce, float friction, float rhythmPower)
        {
            this.songEquipCount = songEquipCount;
            this.level = level;
            this.levelUpCost = levelUpCost;
            this.acceleration = acceleration;
            this.boosterMaxGauge = boosterMaxGauge;
            this.boosterSpeed = boosterSpeed;
            this.turnStrength = turnStrength;
            this.brakeForce = brakeForce;
            this.friction = friction;
            this.rhythmPower = rhythmPower;
        }

        public int SongEquipCount { get => songEquipCount; }
        public int Level { get => level; }
        public float LevelUpCost { get => levelUpCost; }
        public float Acceleration { get => acceleration; }
        public float BoosterMaxGauge { get => boosterMaxGauge; }
        public float BoosterSpeed { get => boosterSpeed; }
        public float TurnStrength { get => turnStrength; }
        public float BrakeForce { get => brakeForce; }
        public float Friction { get => friction; }
        public float RhythmPower { get => rhythmPower; }
    }

    public Car(string name, int currentLevel, List<Status> statuses)
    {
        this.carName = name;
        this.currentLevel = currentLevel;
        this.statuses = statuses;
    }

    public string CarName { get => carName; }
    public int CurrentLevel { get => currentLevel; }
    public int SongEquipCount { get => statuses[currentLevel].SongEquipCount; }
    public int Level { get => statuses[currentLevel].Level; }
    public float LevelUpCost { get => statuses[currentLevel].LevelUpCost; }
    public float Acceleration { get => statuses[currentLevel].Acceleration; }
    public float BoosterMaxGauge { get => statuses[currentLevel].BoosterMaxGauge; }
    public float BoosterSpeed { get => statuses[currentLevel].BoosterSpeed; }
    public float TurnStrength { get => statuses[currentLevel].TurnStrength; }
    public float BrakeForce { get => statuses[currentLevel].BrakeForce; }
    public float Friction { get => statuses[currentLevel].Friction; }
    public float RhythmPower { get => statuses[currentLevel].RhythmPower; }



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
}