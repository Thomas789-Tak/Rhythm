using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class Player : MonoBehaviour
{
    [SerializeField] private int money = 0;

    [SerializeField] private List<Car> carData = new List<Car>();
    [SerializeField] private List<Song> songData = new List<Song>();
    [SerializeField] private List<Stage> stageData = new List<Stage>();

    [SerializeField] private Car curCar;
    [SerializeField] private List<Song> curSongs;
    [SerializeField] private Stage curStage;


    public DatabaseReference reference { get; set; }
    public FirebaseAuth auth;

    [ContextMenu("FirebaseDatabase")]
    void aa()
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