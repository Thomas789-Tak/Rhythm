using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;


public class DataManager : MonoBehaviour
{
    public static System.Uri dbUrl = new System.Uri("https://rhythmicar-default-rtdb.firebaseio.com/");

    #region Original Data

    [SerializeField] private List<Car> carData = new List<Car>();
    [SerializeField] private List<Song> songData = new List<Song>();
    [SerializeField] private List<Stage> stageData = new List<Stage>();

    #endregion

    #region Firebase

    public DatabaseReference reference { get; set; }

    #endregion

    [ContextMenu("ReadDataFromCSV")]
    void ReadDataFromCSV()
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

        int kindOfCar = System.Enum.GetValues(typeof(Car.ECarKind)).Length;
        int totalLevel = 10;

        carData = new List<Car>();

        for (int i = 0; i < kindOfCar; i++)
        {

            List<Car.Status> newStatuses = new List<Car.Status>();
            for (int lv = 0; lv < totalLevel; lv++)
            {
                int num = i * totalLevel + lv;

                int songEquipCount = (int)data[num][nameof(Car.Status.SongEquipCount)];
                int level = (int)data[num][nameof(Car.Status.Level)];
                int levelUpCost = (int)data[num][nameof(Car.Status.LevelUpCost)];
                float acceleration = (float)data[num][nameof(Car.Status.Acceleration)];
                float boosterMaxGauge = (float)data[num][nameof(Car.Status.BoosterMaxGauge)];
                float boosterSpeed = (float)data[num][nameof(Car.Status.BoosterSpeed)];
                float turnStrength = (float)data[num][nameof(Car.Status.TurnStrength)];
                float brakeForce = (float)data[num][nameof(Car.Status.BrakeForce)];
                float friction = (float)data[num][nameof(Car.Status.Friction)];
                float rhythmPower = (float)data[num][nameof(Car.Status.RhythmPower)];

                Car.Status newStatus = new Car.Status(songEquipCount, level, levelUpCost, acceleration,
                    boosterMaxGauge, boosterSpeed, turnStrength, brakeForce, friction, rhythmPower);

                newStatuses.Add(newStatus);
            }

            string name = ((Car.ECarKind)i).ToString();
            int currentLevel = 0;
            
            Car newCar = new Car(name, currentLevel, newStatuses);

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
            var eBGM = (SoundManager.EBGM)System.Enum
                .Parse(typeof(SoundManager.EBGM), (string)data[i][nameof(Song.EBGM)]);

            string songName = (string)data[i][nameof(Song.SongName)];
            string writerName = (string)data[i][nameof(Song.WriterName)];

            bool isOpend = bool.Parse((string)data[i][nameof(Song.IsOpend)]);
            int openCost = (int)data[i][nameof(Song.OpenCost)];

            int bpm = (int)data[i][nameof(Song.Bpm)];
            int maxSpeed = (int)data[i][nameof(Song.MaxSpeed)];

            var newSong = new Song(eBGM, songName, writerName, isOpend, openCost, bpm, maxSpeed);

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
            string theme = (string)data[i][nameof(Stage.Theme)];
            int level = (int)data[i][nameof(Stage.Level)];
            bool isOpend = bool.Parse((string)data[i][nameof(Stage.IsOpend)]);

            var newStage = new Stage(theme, level, isOpend);

            stageData.Add(newStage);
        }
    }

    [ContextMenu("SaveDataToFirebase")]
    void SaveOriginalDataToFirebase()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = dbUrl;
        reference = FirebaseDatabase.DefaultInstance.RootReference.Child("originalData");

        reference.RemoveValueAsync();

        carData.ForEach(data => 
        {
            string dataToJson = JsonUtility.ToJson(data);
            string key = data.Name;

            reference.Child("carData").Child(key).SetRawJsonValueAsync(dataToJson);
        });

        songData.ForEach(data =>
        {
            string dataToJson = JsonUtility.ToJson(data);
            string key = data.SongName;

            reference.Child("songData").Child(key).SetRawJsonValueAsync(dataToJson);
        });

        stageData.ForEach(data =>
        {
            string dataToJson = JsonUtility.ToJson(data);
            string key = data.Theme + " lv" + data.Level;

            reference.Child("stageData").Child(key).SetRawJsonValueAsync(dataToJson);
            //.ContinueWith(task =>
            //{
            //    if (task.IsCompleted) Debug.Log("Data Upload Complete");
            //});
        });

        //string key = reference.Child("rank").Push().Key;

        //reference.Child("rank").Child(key).SetRawJsonValueAsync(json);
    }

}