using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Etourney.Enums;
using Etourney.Enums.WebSocket; 
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Models.WebSocket.Procedures.Player.SignUp;
using Etourney.Scripts;
using Etourney.Settings;

public class LoginUIModel : BaseUIModel
{
    private string connected_status = "";
    // Start is called before the first frame update
    void Start()
    {
        if (_loginButton != null) _loginButton.onClick.AddListener(LoginButton);
        if (_createAccountButton != null) _createAccountButton.onClick.AddListener(CreateAccountButton);
        if (_appleLoginButton != null) _appleLoginButton.onClick.AddListener(AppleLoginButton);
        if (_facebookLoginButton != null) _facebookLoginButton.onClick.AddListener(FacebookLoginButton);
        if (_googleLoginButton != null) _googleLoginButton.onClick.AddListener(GoogleLoginButton);
        if (_twitterLoginButton != null) _twitterLoginButton.onClick.AddListener(TwitterLoginButton);

        EtourneySettings.Instance.LocalGameKey = "dJfLAMpcoN+TwyyxTnp7B2+71WxOXlKYGBg86r3CnGRqwjXhQEYC7XghZcx92kc595qbBZcoWBpaUu6xaj6M9etJe0fRmvKQeB8s2j8q/XqEvXtBLVaaG2mjzWkqlW2wjzmVv2lOdJngLFCoKKbvjpZ7rC+fjyODTD9q3UAI1K2DkISZtHxjva7CDs4talxOdMWCNchKDqJct/gJIxPo9cgEbVaYMV+MBaEoLJVDaDYmzwgEdGodAdsO1c8WmcKLmgJrbtwukk+9HmuNPmcTKP6+fb6FRe3KJO3z1coKsRPslbPZdc4Lr6sx8ZyqC6zxJxv4VPVrtRY6rUfSE4w+g9BkhucR+liAu9DQOOMjIU4UAVkpiM3KwbRCh+6a7aiLAFEHpbRPWLMhccbJ28KMlA==";
        EtourneySettings.Instance.GameKey = "dJfLAMpcoN+TwyyxTnp7B2+71WxOXlKYGBg86r3CnGRqwjXhQEYC7XghZcx92kc595qbBZcoWBpaUu6xaj6M9etJe0fRmvKQeB8s2j8q/XqEvXtBLVaaG2mjzWkqlW2wjzmVv2lOdJngLFCoKKbvjpZ7rC+fjyODTD9q3UAI1K2DkISZtHxjva7CDs4talxOdMWCNchKDqJct/gJIxPo9cgEbVaYMV+MBaEoLJVDaDYmzwgEdGodAdsO1c8WmcKLmgJrbtwukk+9HmuNPmcTKP6+fb6FRe3KJO3z1coKsRPslbPZdc4Lr6sx8ZyqC6zxJxv4VPVrtRY6rUfSE4w+g9BkhucR+liAu9DQOOMjIU4UAVkpiM3KwbRCh+6a7aiLAFEHpbRPWLMhccbJ28KMlA==";

    }


    private void ClearInputValues()
    {
        if (_loginInput != null) _loginInput.text = "";
        if (_passwordInput != null) _passwordInput.text = "";
    }

    public override void Show()
    {
        base.Show();
        ClearInputValues();
    }

    private Action action;

    private void LoginButton()
    {
        if (_loginInput.text == "" || _passwordInput.text == "")
        {
            UIController.Instance.PopUpUIModel.Show();
            UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Warnning", "Wrong input", "OK", action);
        }
        else {
            EtourneySDK.OnStatus += status =>
            {
                Debug.Log(status);

                if (status == EtourneyStatus.Connected)
                {
                    UIController.Instance.LoadingPan.Hide();
                    EtourneySDK.Player.SignInWithEmailAndPassword(_loginInput.text, _passwordInput.text, Result);
                }
                else if (status == EtourneyStatus.Connecting)
                {
                    UIController.Instance.LoadingPan.Show();
                    UIController.Instance.LoadingPan.onChangeLoadingTitle("Server Connecting...");
                }
                else
                {
                    Debug.Log("here");
                    UIController.Instance.PopUpUIModel.Show();
                    UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Warnning", "Failed", "OK", action);
                }
            };

            EtourneySDK.WebSocketConnect();
            
        }
        
    }

    private void Result(WebSocketStatus status, object data)
    {
        Debug.Log("***LogIn Result****>>>>" + status);

        if (status == WebSocketStatus.Ok)
        {
            var parseData = (DtoWsProcedureOutPlayerSignIn)data;
            Debug.Log("***Response Data***>>>>>" + parseData);
            
            SceneManager.LoadScene("ListTournaments_portrait");
        }
        else {
            string message_content_text = "";
            if (status.ToString() == "NotFoundPlayer")
            {
                message_content_text = "Player is not exist";
                UIController.Instance.LoadingPan.Hide();
                UIController.Instance.PopUpUIModel.Show();
                UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Warnning", message_content_text, "OK", action);
            }
            else if (status.ToString() == "Already") {
                SceneManager.LoadScene("ListTournaments_portrait");
            }
            else
            {
                UIController.Instance.PopUpUIModel.Show();
                UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Warnning", "Sorry, Server Error Occured.", "OK", action);
            }
        }
    }

    private void CreateAccountButton()
    {
        Hide();
        UIController.Instance.CreateAccountUIModel.Show();
    }

    private void AppleLoginButton()
    {
        Debug.Log("Apple");
    }

    private void FacebookLoginButton()
    {
        Debug.Log("Facebook");
    }

    private void GoogleLoginButton()
    {
        Debug.Log("Google");
    }

    private void TwitterLoginButton()
    {
        Debug.Log("Twitter");
    }

    #region Fields

    [SerializeField, Header("Email input field"), Tooltip("Email Input field reference")]
    private TMP_InputField _loginInput;

    [SerializeField, Header("Password input field"), Tooltip("Password Input field reference")]
    private TMP_InputField _passwordInput;

    [SerializeField, Header("Login button"), Tooltip("Login button reference")]
    private Button _loginButton;

    [SerializeField, Header("Create Account button"), Tooltip("Create Account button reference")]
    private Button _createAccountButton;

    [SerializeField, Header("Apple Login button"), Tooltip("Apple Login button reference")]
    private Button _appleLoginButton;

    [SerializeField, Header("Facebook Login button"), Tooltip("Facebook Login button reference")]
    private Button _facebookLoginButton;

    [SerializeField, Header("Google Login button"), Tooltip("Google Login button reference")]
    private Button _googleLoginButton;

    [SerializeField, Header("Twitter Login button"), Tooltip("Twitter Login button reference")]
    private Button _twitterLoginButton;

    #endregion

    #region Properties



    #endregion
}
