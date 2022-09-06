using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Etourney.Enums;
using Etourney.Enums.WebSocket;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Models.WebSocket.Procedures.Player.SignUp;
using Etourney.Scripts;

public class CreateAccountUIModel : BaseUIModel
{
    private Action action;

    public string fromEmail = "kobernikihor@gmail.com";
    public string toEmail = "WhomYouWantToSendEmail";
    public string subject = "Congratulation! - Ciroccostudios";
    public string body = "You sign up successfully.";
    public string password = "makemoney2001";

    // Start is called before the first frame update
    void Start()
    {
        if (_createAccountButton != null) _createAccountButton.onClick.AddListener(CreateAccountButton);
        if (_cancelButton != null) _cancelButton.onClick.AddListener(CancelButton);
    }

    private void ClearInputValues()
    {
        if (_nameInput != null) _nameInput.text = "";
        if (_surnameInput != null) _surnameInput.text = "";
        if (_nickInput != null) _nickInput.text = "";
        if (_emailInput != null) _emailInput.text = "";
        if (_passwordInput != null) _passwordInput.text = "";
        if (_repeatPasswordInput != null) _repeatPasswordInput.text = "";
    }

    public override void Show()
    {
        base.Show();
        ClearInputValues();
    }

    private void CreateAccountButton()
    {
        Debug.Log("Create Account");

        if (_passwordInput.text != _repeatPasswordInput.text)
            UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Error", "Passwords do not match!", "OK", null);

        else {
            EtourneySDK.OnStatus += status =>
            {
                Debug.Log(status);

                if (status == EtourneyStatus.Connected)
                {
                    toEmail = _emailInput.text;
                    EtourneySDK.Player.SignUpWithEmailAndPassword(_emailInput.text, _passwordInput.text, _nameInput.text, "", Result);
                }
                else if (status == EtourneyStatus.Connecting)
                {
                    UIController.Instance.LoadingPan.Show();
                    UIController.Instance.LoadingPan.onChangeLoadingTitle("Server Connecting...");
                }
                else
                {
                    Debug.Log("here");
                }
            };

            EtourneySDK.WebSocketConnect();
        }
    }

    private void Result(WebSocketStatus status, object data)
    {
        Debug.Log("***SignUp Result****>>>>" + status);

        if (status == WebSocketStatus.Ok)
        {
            //var parseData = (DtoWsProcedureOutPlayerSignUp) data;
            UIController.Instance.PopUpUIModel.Show();
            UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Success", "Registration is success", "OK", action);
            UIController.Instance.LoadingPan.Hide();
            UIController.Instance.CreateAccountUIModel.Hide();
            UIController.Instance.LoginUIModel.Show();
            EmailSending();

        }
        else if (status.ToString() == "BusyEmail") {
            UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Success", "Already signed up", "OK", action);
        }
        else
        {
            UIController.Instance.PopUpUIModel.Show();
            UIController.Instance.PopUpUIModel.ShowSingleButtonPopUp("Warning", "Registration is Failed", "OK", action);
            UIController.Instance.CreateAccountUIModel.Hide();
            UIController.Instance.LoginUIModel.Show();
        }
    }

    void EmailSending()
    {

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = subject;
        mail.Body = body;
        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com", 587);
        //smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(fromEmail, password) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);

    }

    private void CancelButton()
    {
        Hide();
        UIController.Instance.LoginUIModel.Show();
    }

    #region Fields

    [SerializeField, Header("Name input field"), Tooltip("Name Input field reference")]
    private TMP_InputField _nameInput;

    [SerializeField, Header("Surname input field"), Tooltip("Surname Input field reference")]
    private TMP_InputField _surnameInput;

    [SerializeField, Header("Nick input field"), Tooltip("Nick Input field reference")]
    private TMP_InputField _nickInput;

    [SerializeField, Header("Email input field"), Tooltip("Email Input field reference")]
    private TMP_InputField _emailInput;

    [SerializeField, Header("Password input field"), Tooltip("Password Input field reference")]
    private TMP_InputField _passwordInput;

    [SerializeField, Header("Repeat Password input field"), Tooltip("Repeat Password Input field reference")]
    private TMP_InputField _repeatPasswordInput;

    [SerializeField, Header("Create Account button"), Tooltip("Create Account button reference")]
    private Button _createAccountButton;

    [SerializeField, Header("Cancel button"), Tooltip("Cancel button reference")]
    private Button _cancelButton;

    #endregion

    #region Properties

    #endregion
}