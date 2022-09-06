using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mkey
{
    public class PurchaseInGameProduct : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent GoodPurchaseEvent;
        [SerializeField]
        private UnityEvent FailedPurchaseEvent;

        #region temp vars
        private BubblesPlayer MPlayer => BubblesPlayer.Instance;
        #endregion temp vars

        public void Purchase(int coins)
        {
            coins = Mathf.Abs(coins);
            if (MPlayer.Coins >= coins)
            {
                MPlayer.AddCoins(-coins);
                GoodPurchaseEvent?.Invoke();
            }
            else
            {
                FailedPurchaseEvent?.Invoke();
            }
        }

    }
}