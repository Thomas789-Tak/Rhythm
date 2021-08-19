using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Auth;
using UniRx;

public class LoginManager : MonoBehaviour
{

    private FirebaseAuth auth;

    public InputField InputFieldEmail;
    public InputField InputFieldPassword;

    public Text TextEmailMessage;
    public Text TextPasswordMessage;
    public Text TextResultMessage;

    private void Start()
    {
        // 파이어베이스 인증 객체 초기화
        auth = FirebaseAuth.DefaultInstance;
        TextEmailMessage.text = "";
        TextPasswordMessage.text = "";
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
                    TextResultMessage.text = "로그인 성공";
                    LoadingSceneManager.LoadScene("LobbyScene");
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
