using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemShopCurrencyRef : MonoBehaviour, IPointerClickHandler
{
    public long Id;

    public Image Icon;
    public Text Title;
    public Text Description;
    public Text CurrencyIn;
    public Text CurrencyOut;
    public Image Btn_Buy;

    public event Action<long> OnClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (OnClick != null)
            OnClick(Id);

        Debug.Log(Id.ToString());
    }
}