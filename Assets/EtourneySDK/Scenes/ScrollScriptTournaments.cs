using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScrollScriptTournaments : MonoBehaviour
{
    public ScrollRect ScrollView;
    public GameObject ScrollContent;
    public ItemTournamentRef[] ScrollItem;

    public GameObject GamePanel;
    
    async Task Start()
    {
        var gameSignInResult = await Etourney.Scripts.EtourneySDK.Game.SignInWithKey(Debug.Log);
        var tournaments = await Etourney.Scripts.EtourneySDK.Game.GetTournaments(Debug.Log);

        GamePanel.transform.Find("GameIcon").gameObject.GetComponent<Image>().overrideSprite =
            Etourney.Scripts.EtourneySDK.CreateSpriteFromBase64(Etourney.Scripts.EtourneySDK.Game.Data.Icon);
        GamePanel.transform.Find("GameTitle").gameObject.GetComponent<Text>().text = Etourney.Scripts.EtourneySDK.Game.Data.Name;
        GamePanel.transform.Find("GameDescription").gameObject.GetComponent<Text>().text = Etourney.Scripts.EtourneySDK.Game.Data.Description;
        GetValance();

        for (int i = 0; i < tournaments.Tournaments.Count; i++)
        {
            string currencyIn = string.Empty;
            string currencyOut = string.Empty;
            string coinIn = string.Empty;
            string coinOut = string.Empty;

            if (tournaments.Tournaments[i].TournamentContributionCurrencies != null &&
                tournaments.Tournaments[i].TournamentContributionCurrencies.Count > 0)
                currencyIn = tournaments.Tournaments[i].TournamentContributionCurrencies[0].Amount.Split('.')[0];

            if (tournaments.Tournaments[i].TournamentWinners[0].TournamentWinnersPayoutsCurrencies != null &&
                tournaments.Tournaments[i].TournamentWinners[0].TournamentWinnersPayoutsCurrencies.Count > 0)
                currencyOut = tournaments.Tournaments[i].TournamentWinners[0].TournamentWinnersPayoutsCurrencies[0].Amount.Split('.')[0];

            if (tournaments.Tournaments[i].TournamentContributionCoins != null &&
                tournaments.Tournaments[i].TournamentContributionCoins.Count > 0)
                coinIn = tournaments.Tournaments[i].TournamentContributionCoins[0].Amount.ToString();

            if (tournaments.Tournaments[i].TournamentWinners[0].TournamentWinnersPayoutsCoins != null &&
                tournaments.Tournaments[i].TournamentWinners[0].TournamentWinnersPayoutsCoins.Count > 0)
                coinOut = tournaments.Tournaments[i].TournamentWinners[0].TournamentWinnersPayoutsCoins[0].Amount.ToString();

            Debug.Log("Here is Tournament number *******" + tournaments.Tournaments[i].PlayersInTournament);
            GenerateItem(tournaments.Tournaments[i].Icon,
                tournaments.Tournaments[i].Name,
                tournaments.Tournaments[i].Description,
                currencyIn,
                currencyOut,
                coinIn,
                coinOut,
                tournaments.Tournaments[i].Id,
                i);
        }

        ScrollView.verticalNormalizedPosition = 1;
    }
    
    void Update()
    {

    }

    public void GenerateItem(string icon, string title, string description,
        string currencyIn, string currencyOut, string coinIn, string coinOut, long id, int number)
    {
        Debug.Log("Here********" + title);
        if (title.Length > 50) {
            title = title.Substring(0, 49);
        }
        int color_num = number % 4;
        GameObject scrollItemObject = Instantiate(ScrollItem[color_num].gameObject);
        scrollItemObject.transform.SetParent(ScrollContent.transform, false);

        var itemTournament = scrollItemObject.GetComponent<ItemTournamentRef>();
        itemTournament.Id = id;
        itemTournament.OnClick += Handler;

        itemTournament.Icon.overrideSprite = Etourney.Scripts.EtourneySDK.CreateSpriteFromBase64(icon);
        itemTournament.Title.text = title;
        itemTournament.Description.text = description;
        itemTournament.CurrencyIn.text = currencyIn;
        itemTournament.CurrencyOut.text = currencyOut;
        itemTournament.CoinIn.text = coinIn;
        itemTournament.CoinOut.text = coinOut;

        if (title.IndexOf("1 vs 1") > -1) {
            itemTournament._1v1.SetActive(true);
            itemTournament._1vs.SetActive(false);
            itemTournament.Player_num.SetActive(true);
            itemTournament._no_limit.SetActive(false);
        }
        else
        {
            itemTournament._1v1.SetActive(false);
            itemTournament._1vs.SetActive(true);
            itemTournament.Player_num.SetActive(false);
            itemTournament._no_limit.SetActive(true);
        }
    }

    public async void Handler(long id)
    {
        Debug.Log("Here123----->" + id);
        //var topUpBalanceResult = await Etourney.Scripts.EtourneySDK.Player.TopUpBalance(id);

        //if (topUpBalanceResult != null)
        //{
        //    Etourney.Scripts.EtourneySDK.OpenPaymentLink(topUpBalanceResult.PaymentUrl);
        //}
    }

    public async void GetValance()
    {
        Debug.Log("GetValanceFunction is called----->");
        var userValanceResult = await Etourney.Scripts.EtourneySDK.Player.GetBalance();
        Debug.Log("GetValanceFunction result is ----->" + userValanceResult.PlayerBalances.ToString());

        var output = JsonUtility.ToJson(userValanceResult, true);
        Debug.Log("bbb---->" + output);

        //if (topUpBalanceResult != null)
        //{
        //    Etourney.Scripts.EtourneySDK.OpenPaymentLink(topUpBalanceResult.PaymentUrl);
        //}
    }
}