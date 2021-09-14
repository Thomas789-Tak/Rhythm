using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using UniRx;

[System.Serializable]
public class Player : MonoBehaviour
{
    [SerializeField] private int money = 0;

    [SerializeField] private List<Car> carData = new List<Car>();
    [SerializeField] private List<Song> songData = new List<Song>();
    [SerializeField] private List<Stage> stageData = new List<Stage>();

    [SerializeField] private Car curCar;
    [SerializeField] private List<Song> curSongs;
    [SerializeField] private Stage curStage;

    private Subject<Car> carSubject;
    private Subject<List<Song>> songsSubject;
    private Subject<Stage> stageSubject;

    public DatabaseReference reference { get; set; }
    public int Money { get => money; set => money = value; }

    public FirebaseAuth auth;

    public Player() { }
    public Player(int money, List<Car> carData, List<Song> songData, List<Stage> stageData)
    {
        this.money = money;
        this.carData = carData;
        this.songData = songData;
        this.stageData = stageData;
    }

    [ContextMenu("Get Path")]
    public void GETPATH()
    {

    }

    public void SetCar(Car newCar)
    {
        curCar = carData.Find(car => car.CarName == newCar.CarName);
    }

    public void SetSong(int slotNum, SoundManager.EBGM newBGM)
    {
        curSongs[slotNum] = songData.Find(song => song.EBGM == newBGM);
    }

    public void SetStage(Stage newStage)
    {
        curStage = stageData.FindAll(stage => stage.Theme == newStage.Theme)
            .Find(stage => stage.Level == newStage.Level);
    }


    [ContextMenu("FirebaseDatabase")]
    void WriteAndReadDatabase()
    {
        WriteDatabase();
        ReadDatabase();
    }

    void WriteDatabase()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = DataManager.dbUrl;
        reference = FirebaseDatabase.DefaultInstance.RootReference;

    }

    void ReadDatabase()
    {
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = DataManager.dbUrl;
        reference = Firebase.Database.FirebaseDatabase.DefaultInstance.GetReference("rank");

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
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
                if (!task.IsCanceled && !task.IsFaulted)
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



}