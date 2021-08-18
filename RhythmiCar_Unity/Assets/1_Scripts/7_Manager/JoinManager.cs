using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class JoinManager : MonoBehaviour
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

    bool InputCheckEmail()
    {
        bool result = true;
        string email = InputFieldEmail.text;

        if (email.Length < 11)
        {
            TextEmailMessage.text = "�̸����� 11�� �̻��̿��� �մϴ�.";
            result = false;
        }
        else
        {
            TextEmailMessage.text = "";
        }

        return result;
    }

    bool InputCheckPassword()
    {
        bool result = true;
        string password = InputFieldPassword.text;

        if (password.Length < 8)
        {
            TextPasswordMessage.text = "��й�ȣ�� 8�� �̻��̿��� �մϴ�.";
            result = false;
        }
        else
        {
            TextPasswordMessage.text = "";
        }

        return result;
    }

    public void Check(int num)
    {
        if (num == 0)
            InputCheckEmail();
        else if (num == 1)
            InputCheckPassword();
    }

    public void Join()
    {
        if (!InputCheckEmail() || !InputCheckPassword())
        {
            return;
        }
        else
        {
            string email = InputFieldEmail.text;
            string password = InputFieldPassword.text;

            TextResultMessage.text = "ȸ������ �õ� �� ...";

            // ���� ��ü�� �̿��Ͽ� �÷��̾ �Է��� �̸��ϰ� ��й�ȣ�� ȸ�������� �����մϴ�.
            auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(
                task =>
                {
                    if (!task.IsCanceled && !task.IsFaulted)
                    {
                        Result(true);
                        Debug.Log("ASDF");
                        Debug.Log("ASDF123");
                    }
                    else
                    {
                        Result(false);
                    }
                });
        }
    }

    public void Result(bool result)
    {
        if (result)
        {
            TextResultMessage.text = "ȸ������ ����!";
            Debug.Log("Hi");
        }
        else
        {
            TextResultMessage.text = "�̹� ��� ���̰ų� ������ �ٸ��� �ʽ��ϴ�.";
            Debug.Log("Bye");
        }
    }

}
