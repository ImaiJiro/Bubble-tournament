using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class FooterBoosterHelper : MonoBehaviour
    {
        public BoosterFunc booster;
        public Text boosterCounter;
        public Image boosterImage;
        public PopUpsController boosterShop;

        #region regular
        private IEnumerator Start()
        {
            while (booster == null) yield return new WaitForEndOfFrame();
            if (booster != null)
            {
                booster.ChangeCountEvent += ChangeCountEventHandler;
                booster.FooterClickEvent += FooterClickEventHandler;
                booster.ActivateEvent += ShowActive;
            }
            RefreshFooterGui();
        }

        private void OnDestroy()
        {
            if(gameObject)  SimpleTween.Cancel(gameObject, true);
            if (booster != null)
            {
                booster.ChangeCountEvent -= ChangeCountEventHandler;
                booster.FooterClickEvent -= FooterClickEventHandler;
                booster.ActivateEvent -= ShowActive;
            }
        }
        #endregion regular

        /// <summary>
        /// Refresh booster count and booster visibilty
        /// </summary>
        private void RefreshFooterGui()
        {
            //Debug.Log("refresh");
            if (booster)
            {
                if (boosterCounter) boosterCounter.text = booster.Count.ToString();
                gameObject.SetActive(booster.Use);
            }
        }

        /// <summary>
        /// Show active footer booster with another color
        /// </summary>
        /// <param name="active"></param>
        private void ShowActive(BoosterFunc b)
        {
            if (gameObject) SimpleTween.Cancel(gameObject, true);
            if (boosterImage)
            {
                Color c = boosterImage.color;
                boosterImage.color = new Color(1, 1, 1, 1);
                if (booster.IsActive)
                {
                    SimpleTween.Value(gameObject, 1.0f, 0.5f, 0.5f).SetEase(EaseAnim.EaseLinear).
                                SetOnUpdate((float val) =>
                                {
                                    if (booster.IsActive) boosterImage.color = new Color(1, val, val, 1);
                                    else
                                    {
                                        boosterImage.color = new Color(1, 1, 1, 1);
                                        SimpleTween.Cancel(gameObject, true);
                                    }

                                }).SetCycled();
                }
            }
        }

        #region handlers
        public void ChangeCountEventHandler(int count)
        {
            RefreshFooterGui();
        }

        public void FooterClickEventHandler()
        {
            ShowActive(booster);
        }

        public void Booster_Click()
        {
            booster.FooterClickEventHandler();
            if (booster.Count == 0) BubblesGuiController.Instance.ShowPopUp(boosterShop);
            ShowActive(booster);
        }
        #endregion handlers
    }
}