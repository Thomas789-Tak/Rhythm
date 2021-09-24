using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using UniRx;
using UnityEditor;
using System.Threading.Tasks;

public class LoginManager : MonoBehaviour
{
    private FirebaseAuth auth;
    public GameObject GameData;

    public InputField InputFieldEmail;
    public InputField InputFieldPassword;

    public Text TextEmailMessage;
    public Text TextPasswordMessage;
    public Text TextResultMessage;

    private bool isLogined = false;

    private void Start()
    {
        // 파이어베이스 인증 객체 초기화
        auth = FirebaseAuth.DefaultInstance;
        TextEmailMessage.text = "";
        TextPasswordMessage.text = "";
    }

    private void Update()
    {

       // 로그인 버튼을 누른 후 Login 함수에서 Login이 완료되었을 경우
       if (isLogined)
        {
            isLogined = false;
            Debug.Log("MoveScene");
            LoadingSceneManager.LoadScene("LobbyScene");
        }
    }

    public void Login()
    {
        string email = InputFieldEmail.text;
        string password = InputFieldPassword.text;

        // 인증 객체를 이용해 이메일과 비밀번호로 로그인을 수행합니다.
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("[로그인 성공] ID : " + auth.CurrentUser.UserId);
                    InitIngameManager.auth = auth;
                    Debug.Log(InitIngameManager.auth.CurrentUser.Email);
                    isLogined = true;

                    //TextResultMessage.text = "로그인 성공";
                    //SceneMove();
                    //LoadingSceneManager.LoadScene("LobbyScene");
                }
                else
                {
                    Debug.Log("[로그인 실패]");
                    TextEmailMessage.text = "계정을 다시 확인하세요";
                }
            });
    }

    [ContextMenu("MoveScene")]
    public void MoveScene()
    {
        Debug.Log("MoveScene");
        TextResultMessage.text = "로그인 성공";
        LoadingSceneManager.LoadScene("LobbyScene");
    }
}
