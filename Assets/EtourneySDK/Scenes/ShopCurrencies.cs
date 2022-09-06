using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ShopCurrencies : MonoBehaviour
{
    public ScrollRect ScrollView;
    public GameObject ScrollContent;
    public ItemShopCurrencyRef ScrollItem;

    public GameObject GamePanel;

    async Task Start()
    {
        var gameSignInResult = await Etourney.Scripts.EtourneySDK.Game.SignInWithKey(Debug.Log);
        var gameCurrencyPrices = await Etourney.Scripts.EtourneySDK.Game.GameCurrencyPrices(Debug.Log);

        GamePanel.transform.Find("GameIcon").gameObject.GetComponent<Image>().overrideSprite =
            Etourney.Scripts.EtourneySDK.CreateSpriteFromBase64(Etourney.Scripts.EtourneySDK.Game.Data.Icon);
        GamePanel.transform.Find("GameTitle").gameObject.GetComponent<Text>().text = Etourney.Scripts.EtourneySDK.Game.Data.Name;
        GamePanel.transform.Find("GameDescription").gameObject.GetComponent<Text>().text = Etourney.Scripts.EtourneySDK.Game.Data.Description;

        for (int i = 0; i < gameCurrencyPrices.GameCurrencyPrices.Count; i++)
        {
            GenerateItem(gameCurrencyPrices.GameCurrencyPrices[i].Icon,
                gameCurrencyPrices.GameCurrencyPrices[i].Name,
                gameCurrencyPrices.GameCurrencyPrices[i].Description,
                gameCurrencyPrices.GameCurrencyPrices[i].CostInCurrency.Split('.')[0],
                gameCurrencyPrices.GameCurrencyPrices[i].Currency.Split('.')[0],
                gameCurrencyPrices.GameCurrencyPrices[i].Id);
        }

        ScrollView.verticalNormalizedPosition = 0;
    }
    
    void Update()
    {
        
    }

    public void GenerateItem(string icon, string title, string description, string currencyIn, string currencyOut, long id)
    {
        GameObject scrollItemObject = Instantiate(ScrollItem.gameObject);
        scrollItemObject.transform.SetParent(ScrollContent.transform, false);

        var itemShopCurrency = scrollItemObject.GetComponent<ItemShopCurrencyRef>();
        itemShopCurrency.Id = id;
        itemShopCurrency.Btn_Buy.GetComponent<ButtonController>().OnClick += Handler;

        itemShopCurrency.Icon.overrideSprite = Etourney.Scripts.EtourneySDK.CreateSpriteFromBase64(icon);
        itemShopCurrency.Title.text = title;
        itemShopCurrency.Description.text = description;
        itemShopCurrency.CurrencyIn.text = currencyIn;
        itemShopCurrency.CurrencyOut.text = currencyOut;
    }

    public async void Handler(long id)
    {
        Debug.Log("clicked Buy Btn**********" + id);
        var topUpBalanceResult = await Etourney.Scripts.EtourneySDK.Player.TopUpBalance(id);

        if (topUpBalanceResult != null)
        {
            Etourney.Scripts.EtourneySDK.OpenPaymentLink(topUpBalanceResult.PaymentUrl);
        }
    }
}