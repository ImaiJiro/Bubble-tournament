using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Etourney.Enums;
using Etourney.Enums.WebSocket;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Models.WebSocket.Procedures.Player.SignUp;
using Etourney.Scripts;
using TMPro;

using UnityEngine.SceneManagement;

public class ItemTournamentRef : MonoBehaviour, IPointerClickHandler
{
    public long Id;

    public Image Icon;
    public Text Title;
    public Text Description;
    public Text CurrencyIn;
    public Text CurrencyOut;
    public Text CoinIn;
    public Text CoinOut;
    public GameObject Player_num;
    public GameObject _1v1;
    public GameObject _1vs;
    public GameObject _no_limit;

    public GameObject str_fee;

    public event Action<long> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
            OnClick(Id);

        Debug.Log(Id.ToString());
        //EtourneySDK.Tournaments.JoinToTournament(Id, Result);
    }

    public void OnClickJoinBtn() {
        SceneManager.LoadScene("0_VerticalMap");
        GameObject.Find("LoginScreen").SetActive(false);
    }

    private void Result(WebSocketStatus status, object data)
    {
        Debug.Log("***LogIn Result****>>>>" + status);

    }
}