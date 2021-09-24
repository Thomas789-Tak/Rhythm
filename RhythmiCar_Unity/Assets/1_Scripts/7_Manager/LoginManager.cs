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
        // ���̾�̽� ���� ��ü �ʱ�ȭ
        auth = FirebaseAuth.DefaultInstance;
        TextEmailMessage.text = "";
        TextPasswordMessage.text = "";
    }

    private void Update()
    {

       // �α��� ��ư�� ���� �� Login �Լ����� Login�� �Ϸ�Ǿ��� ���
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

        // ���� ��ü�� �̿��� �̸��ϰ� ��й�ȣ�� �α����� �����մϴ�.
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(
            task =>
            {
                if (task.IsCompleted && !task.IsCanceled && !task.IsFaulted)
                {
                    Debug.Log("[�α��� ����] ID : " + auth.CurrentUser.UserId);
                    InitIngameManager.auth = auth;
                    Debug.Log(InitIngameManager.auth.CurrentUser.Email);
                    isLogined = true;

                    //TextResultMessage.text = "�α��� ����";
                    //SceneMove();
                    //LoadingSceneManager.LoadScene("LobbyScene");
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
