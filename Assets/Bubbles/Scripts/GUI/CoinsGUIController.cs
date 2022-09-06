﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class CoinsGUIController : MonoBehaviour
    {
        [SerializeField]
        private Text balanceAmountText;

        #region temp vars
        private TweenIntValue balanceTween;
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private BubblesGuiController MGui { get { return BubblesGuiController.Instance; } }
        #endregion temp vars

        #region regular
        private IEnumerator Start()
        {
            while (!MPlayer)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();

            // set player event handlers
            MPlayer.ChangeCoinsEvent += ChangeBalanceHandler;
            MPlayer.LoadCoinsEvent += LoadBalanceHandler;
            if (balanceAmountText) balanceTween = new TweenIntValue(balanceAmountText.gameObject, MPlayer.Coins, 1, 3, true, (b) => { if (this && balanceAmountText) balanceAmountText.text = (b > 0) ? b.ToString() : "0"; });
            RefreshBalance();
        }

        private void OnDestroy()
        {
            if (MPlayer)
            {
                // remove player event handlers
                MPlayer.ChangeCoinsEvent -= ChangeBalanceHandler;
                MPlayer.LoadCoinsEvent -= LoadBalanceHandler;
            }
        }
        #endregion regular

        /// <summary>
        /// Refresh gui balance
        /// </summary>
        private void RefreshBalance()
        {
            if (balanceAmountText && MPlayer) balanceAmountText.text = (MPlayer.Coins > 0) ? MPlayer.Coins.ToString() : "0"; // MPlayer.Coins.ToString("# ### ### ### ###") 
        }

        #region eventhandlers
        private void ChangeBalanceHandler(int newBalance)
        {
            if (balanceTween != null) balanceTween.Tween(newBalance, 100);
            else
            {
                if (balanceAmountText) balanceAmountText.text = (newBalance > 0) ? newBalance.ToString() : "0";
            }
        }

        private void LoadBalanceHandler(int newBalance)
        {
            if (balanceAmountText) balanceAmountText.text = (newBalance > 0) ? newBalance.ToString() : "0";
        }
        #endregion eventhandlers
    }
}
