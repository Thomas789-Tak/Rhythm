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
        // 파이어베이스 인증 객체 초기화
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
            TextEmailMessage.text = "이메일은 11자 이상이여야 합니다.";
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
            TextPasswordMessage.text = "비밀번호는 8자 이상이여야 합니다.";
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

            TextResultMessage.text = "회원가입 시도 중 ...";

            // 인증 객체를 이용하여 플레이어가 입력한 이메일과 비밀번호로 회원가입을 수행합니다.
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
            TextResultMessage.text = "회원가입 성공!";
            Debug.Log("Hi");
        }
        else
        {
            TextResultMessage.text = "이미 사용 중이거나 형식이 바르지 않습니다.";
            Debug.Log("Bye");
        }
    }

}
