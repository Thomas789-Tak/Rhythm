using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;


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
    public FirebaseAuth auth;

    public class Rank
    {
        public string name;
        public int score;
        public int timestamp;

        public Rank(string name, int score, int timestamp)
        {
            this.name = name;
            this.score = score;
            this.timestamp = timestamp;
        }
    }


    [ContextMenu("FirebaseDatabase")]
    void FirebaseDatabase()
    {
        WriteDatabase();
        ReadDatabase();
    }

    void WriteDatabase()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://rhythmicar-default-rtdb.firebaseio.com/");
        reference = Firebase.Database.FirebaseDatabase.DefaultInstance.RootReference;

        Rank rank = new Rank("Sittan", 96, 1413523370);

        string json = JsonUtility.ToJson(rank);

        string key = reference.Child("rank").Push().Key;

        reference.Child("rank").Child(key).SetRawJsonValueAsync(json);

    }

    void ReadDatabase()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new System.Uri("https://rhythmicar-default-rtdb.firebaseio.com/");
        reference = Firebase.Database.FirebaseDatabase.DefaultInstance.GetReference("rank");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach(DataSnapshot data in snapshot.Children)
                {
                    IDictionary rank = (IDictionary)data.Value;
                    Debug.Log("이름: " + rank["name"] + " / 점수: " + rank["score"]);
                }
            }
        });
    }

    [ContextMenu("FirebaseAuth")]
    void Auth()
    {
        auth = FirebaseAuth.DefaultInstance;
        Join("hang5050@naver.com", "shqk1492");
        Login("hang5050@naver.com", "shqk1492");
    }

    void Join(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if(!task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("[회원가입 성공] 이메일 :" + email);
                }
                else
                {
                    Debug.Log("[회원가입 실패]");
                }

            });
    }

    void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    FirebaseUser user = task.Result;
                    Debug.Log("[로그인 성공] " + user.Email);
                }
                else
                {
                    Debug.Log("[로그인 실패]");
                }
            });
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

}