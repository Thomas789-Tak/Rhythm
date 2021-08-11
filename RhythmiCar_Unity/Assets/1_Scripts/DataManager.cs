using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;




public class DataManager : MonoBehaviour
{
    #region Data

    //public Dictionary<string, Car> carData;// = new Dictionary<string, Car>();
    public List<Car> carData = new List<Car>();
    public List<Song> songData = new List<Song>();
    public List<Stage> stageData = new List<Stage>();

    #endregion

    void Start()
    {

    }

    public DatabaseReference reference { get; set; }

    [ContextMenu("Test")]
    void test()
    {

    }

    [ContextMenu("ReadData")]
    void ReadData()
    {
        ReadCarData();
        ReadSongData();
        ReadStageData();
    }

    void ReadCarData()
    {
        string dataPath = "Data/CarData";
        List<Dictionary<string, object>> data = CSVReader.Read(dataPath);
        Debug.Log("Load Path : " + dataPath);

        int kindOfCar = System.Enum.GetValues(typeof(ECarKind)).Length;
        int totalLevel = 10;

        carData = new List<Car>();

        for (int i = 0; i < kindOfCar; i++)
        {

            Car newCar = new Car();
            newCar.statuses = new List<Car.Status>();
            newCar.name = ((ECarKind)i).ToString();
            newCar.currentLevel = 0;

            
            for (int lv = 0; lv < totalLevel; lv++)
            {
                Car.Status newStatus = new Car.Status();
                int num = i * totalLevel + lv;

                newStatus.songEquipCount = (int)data[num][nameof(Car.Status.songEquipCount)];
                newStatus.level = (int)data[num][nameof(Car.Status.level)];
                newStatus.levelUpCost = (int)data[num][nameof(Car.Status.levelUpCost)];
                newStatus.acceleration = (float)data[num][nameof(Car.Status.acceleration)];
                newStatus.boosterMaxGauge = (float)data[num][nameof(Car.Status.boosterMaxGauge)];
                newStatus.turnStrength = (float)data[num][nameof(Car.Status.turnStrength)];
                newStatus.brakeForce = (float)data[num][nameof(Car.Status.brakeForce)];
                newStatus.friction = (float)data[num][nameof(Car.Status.friction)];
                newStatus.rhythmPower = (float)data[num][nameof(Car.Status.rhythmPower)];

                newCar.statuses.Add(newStatus);
            }

            carData.Add(newCar);
        }
    }

    void ReadSongData()
    {
        string dataPath = "Data/SongData";
        List<Dictionary<string, object>> data = CSVReader.Read(dataPath);
        Debug.Log("Load Path : " + dataPath);

        int kindOfSong = data.Count;

        songData = new List<Song>();

        for (int i = 0; i < kindOfSong; i++)
        {
            string name = (string)data[i][nameof(Song.name)];

            bool isOpend = bool.Parse((string)data[i][nameof(Song.isOpend)]);
            int openCost = (int)data[i][nameof(Song.openCost)];

            int bpm = (int)data[i][nameof(Song.bpm)];
            int maxSpeed = (int)data[i][nameof(Song.maxSpeed)];

            var newSong = new Song(name, isOpend, openCost, bpm, maxSpeed);

            songData.Add(newSong);
        }

    }

    void ReadStageData()
    {
        string dataPath = "Data/StageData";
        List<Dictionary<string, object>> data = CSVReader.Read(dataPath);
        Debug.Log("Load Path : " + dataPath);

        int totalNum = data.Count;

        stageData = new List<Stage>();

        for (int i = 0; i < totalNum; i++)
        {
            string theme = (string)data[i][nameof(Stage.theme)];
            int level = (int)data[i][nameof(Stage.level)];
            bool isOpend = bool.Parse((string)data[i][nameof(Stage.isOpend)]);

            var newStage = new Stage(theme, level, isOpend);

            stageData.Add(newStage);
        }
    }

    [ContextMenu("CheckData")]
    public void CheckData()
    {
        //Debug.Log(carData[ECarKind.Red.ToString()].statuses[2].songEquipCount);
    }
}