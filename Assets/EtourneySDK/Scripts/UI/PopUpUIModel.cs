using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpUIModel : BaseUIModel
{
    // Start is called before the first frame update
    void Start()
    {
        if (_firstButton != null) _firstButton.onClick.AddListener(FirstButton);
        if (_secondButton != null) _secondButton.onClick.AddListener(SecondButton);
    }

    /// <summary>
    /// Shows PopUp with a single button.
    /// </summary>
    /// <param name="title">Title of the PopUp</param>
    /// <param name="mainText">Main text of the PopUp</param>
    /// <param name="buttonTitle">Single button title</param>
    /// <param name="action">Action of the button. PopUp will close oon button click.</param>
    public void ShowSingleButtonPopUp(string title, string mainText, string buttonTitle, Action action)
    {
        _titleText.text = title;
        _mainText.text = mainText;
        _firstButtonTitleText.text = buttonTitle;
        _firstAction = action;
        _firstButton.gameObject.SetActive(true);
        _secondButton.gameObject.SetActive(false);
        Show();
    }

    /// <summary>
    /// Shows PopUp with two buttons.
    /// </summary>
    /// <param name="title">Title of the PopUp</param>
    /// <param name="mainText">Main text of the PopUp</param>
    /// <param name="firstButtonTitle">First/Left button title</param>
    /// <param name="firstButtonAction">Action of the first/left button. PopUp will close oon button click.</param>
    /// <param name="secondButtonTitle">Second/Right button title</param>
    /// <param name="secondButtonAction">Action of the second/right button. PopUp will close oon button click.</param>
    public void ShowDoubleButtonPopUp(string title, string mainText, string firstButtonTitle, Action firstButtonAction,
        string secondButtonTitle, Action secondButtonAction)
    {
        _titleText.text = title;
        _mainText.text = mainText;
        _firstButtonTitleText.text = firstButtonTitle;
        _firstAction = firstButtonAction;
        _secondButtonTitleTextText.text = secondButtonTitle;
        _secondAction = secondButtonAction;
        _firstButton.gameObject.SetActive(true);
        _secondButton.gameObject.SetActive(true);
        Show();
    }

    private void FirstButton()
    {
        Hide();
        _firstAction?.Invoke();
        _firstAction = null;
        _secondAction = null;
    }

    private void SecondButton()
    {
        Hide();
        _secondAction?.Invoke();
        _firstAction = null;
        _secondAction = null;
    }

    #region Fields

    [SerializeField, Header("Title text"), Tooltip("Title text reference")]
    private TextMeshProUGUI _titleText;

    [SerializeField, Header("Main text"), Tooltip("Main text reference")]
    private TextMeshProUGUI _mainText;
    
    [SerializeField, Header("First button"), Tooltip("First button reference")]
    private Button _firstButton;
    
    [SerializeField, Header("First Button Title text"), Tooltip("First Button  text reference")]
    private TextMeshProUGUI _firstButtonTitleText;
    
    [SerializeField, Header("Second button"), Tooltip("Second button reference")]
    private Button _secondButton;

    [SerializeField, Header("Second Button Title text"), Tooltip("Second Button Title text reference")]
    private TextMeshProUGUI _secondButtonTitleTextText;

    private Action _firstAction;
    private Action _secondAction;

    #endregion

    #region Properties

    #endregion
}