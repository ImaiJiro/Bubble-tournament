using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
	public class LifeGUIController : MonoBehaviour
	{
        [SerializeField]
        private GameObject lifesGroup;
        [SerializeField]
        private Text lifesText;
        [SerializeField]
        private Image infiniteIcon;
        [SerializeField]
        private Text timerText;

        #region temp vars
        private float restHours = 0;
        private float restMinutes = 0;
        private float restSeconds = 0;
        private BubblesGuiController MGui { get { return BubblesGuiController.Instance; } }
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private GameConstructSet GCSet { get { return GameConstructSet.Instance; } } 
        private bool unLimited = false;
        #endregion temp vars

        public static LifeGUIController Instance;

        #region regular
        private IEnumerator Start()
        {
            while (!MPlayer)
            {
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForEndOfFrame();

            unLimited = GCSet.UnLimited;
            if (lifesGroup) lifesGroup.SetActive(!unLimited);
            if (!unLimited)
            {
                MPlayer.ChangeLifeEvent += RefreshLife;
                MPlayer.StartInfiniteLifeEvent += RefreshInfiniteLife;
                MPlayer.EndInfiniteLifeEvent += RefreshInfiniteLife;
                if (timerText) timerText.text = restMinutes.ToString("00") + ":" + restSeconds.ToString("00");
            }
            Refresh();
        }

        void OnGUI()
        {
            if (!unLimited) RefreshTimerText();
        }

        private void OnDestroy()
        {
            if (MPlayer)
            {
                if (!unLimited)
                {
                    MPlayer.ChangeLifeEvent -= RefreshLife;
                    MPlayer.StartInfiniteLifeEvent -= RefreshInfiniteLife;
                    MPlayer.EndInfiniteLifeEvent -= RefreshInfiniteLife;
                }
            }
        }
        #endregion regular

        private void RefreshTimerText()
        {
            LifeIncTimer lifeIncTimer = LifeIncTimer.Instance;
            InfiniteLifeTimer infiniteLifeTimer = InfiniteLifeTimer.Instance;
            if (timerText)
            {
                if (infiniteLifeTimer && infiniteLifeTimer.IsWork)
                {
                    if (restHours != infiniteLifeTimer.RestHours || restMinutes != infiniteLifeTimer.RestMinutes || restSeconds != infiniteLifeTimer.RestSeconds)
                    {
                        restHours = infiniteLifeTimer.RestHours;
                        restMinutes = infiniteLifeTimer.RestMinutes;
                        restSeconds = infiniteLifeTimer.RestSeconds;
                        timerText.text = restHours.ToString("00") + ":" + restMinutes.ToString("00"); // + ":" + restSeconds.ToString("00");
                    }
                    if (lifesText && lifesText.gameObject.activeSelf) lifesText.gameObject.SetActive(false);
                    if (infiniteIcon && !infiniteIcon.gameObject.activeSelf) infiniteIcon.gameObject.SetActive(true);
                    return;
                }

                if (lifeIncTimer)
                {
                    if (restMinutes != lifeIncTimer.RestMinutes || restSeconds != lifeIncTimer.RestSeconds)
                    {
                        restMinutes = lifeIncTimer.RestMinutes;
                        restSeconds = lifeIncTimer.RestSeconds;
                        timerText.text = restMinutes.ToString("00") + ":" + restSeconds.ToString("00");
                    }
                    if (lifesText && !lifesText.gameObject.activeSelf) lifesText.gameObject.SetActive(true);
                    if (infiniteIcon && infiniteIcon.gameObject.activeSelf) infiniteIcon.gameObject.SetActive(false);
                }
            }
        }

        private void Refresh()
        {
            if (!unLimited)
            {
                RefreshLife(MPlayer.Life);
                RefreshTimerText();
            }
        }

        private void RefreshLife(int life)
        {
            if(lifesText) lifesText.text = life.ToString();
        }

        private void RefreshInfiniteLife()
        {
            if (infiniteIcon) infiniteIcon.gameObject.SetActive(MPlayer.HasInfiniteLife());
            if (lifesText) lifesText.enabled = !MPlayer.HasInfiniteLife();
        }
    }
}
