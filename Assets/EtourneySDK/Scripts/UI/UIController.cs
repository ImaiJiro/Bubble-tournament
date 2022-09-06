using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UIController : MonoBehaviour
{
    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoginUIModel.Show();
        PopUpUIModel.gameObject.transform.SetAsLastSibling();
    }

    #region Singleton

    private static UIController _instance;

    public static UIController Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                string message = "UIController is not attached to a gameObject or the gameObject is not active.\nMake also sure to set 'Instance = this;' in your Awake() function!";

                Debug.LogError(message);
                throw new NullReferenceException(message); 
            }
        }
        set
        {
            _instance = value;
        }
    }
    
    public static bool IsInitialized => _instance != null;

    protected virtual void OnApplicationQuit()
    {
        _instance = default;
    }

    #endregion

    #region Fields

    [SerializeField, Header("Login UI Model"), Tooltip("Login UI Model script reference")]
    private LoginUIModel _loginUIModel;

    [SerializeField, Header("Create Account UI Model"), Tooltip("Create Account UI Model script reference")]
    private CreateAccountUIModel _createAccountUIModel;

    [SerializeField, Header("Pop Up UI Model"), Tooltip("Pop Up UI Model script reference")]
    private PopUpUIModel _popUpUIModel;

    [SerializeField, Header("Loading Pan UI Model"), Tooltip("Loading Pan UI Model script reference")]
    private LoadingPan _loadingPan;

    #endregion

    #region Properties

    public LoginUIModel LoginUIModel => _loginUIModel;

    public CreateAccountUIModel CreateAccountUIModel => _createAccountUIModel;

    public PopUpUIModel PopUpUIModel => _popUpUIModel;

    public LoadingPan LoadingPan => _loadingPan;

    #endregion
}
