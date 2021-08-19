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
        // ���̾�̽� ���� ��ü �ʱ�ȭ
        auth = FirebaseAuth.DefaultInstance;
        TextEmailMessage.text = "";
        TextPasswordMessage.text = "";
    }

    public void Login()
    {
        string email = InputFieldEmail.text;
        string password = InputFieldPassword.text;

        // ���� ��ü�� �̿��� �̸��ϰ� ��й�ȣ�� �α����� �����մϴ�.
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("[�α��� ����] ID : " + auth.CurrentUser.UserId);
                    InitIngameManager.auth = auth;
                    Debug.Log(InitIngameManager.auth.CurrentUser.Email);
                    TextResultMessage.text = "�α��� ����";
                    LoadingSceneManager.LoadScene("LobbyScene");
                }
                else
                {
                    Debug.Log("[�α��� ����]");
                    TextEmailMessage.text = "������ �ٽ� Ȯ���ϼ���";
                }

            });
    }

    [ContextMenu("MoveScene")]
    public void MoveScene()
    {
        Debug.Log("MoveScene");
        TextResultMessage.text = "�α��� ����";
        LoadingSceneManager.LoadScene("LobbyScene");
    }
}
