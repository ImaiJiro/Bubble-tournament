using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class DailyRewardLine : MonoBehaviour
    {
        [SerializeField]
        private Image rewardReceivedImage;
        [SerializeField]
        private Image currentRewardImage;
        [SerializeField]
        private Text dayText;
        [SerializeField]
        private Button getButton;
        [SerializeField]
        private int opacityReceived = 50;

        #region temp vars

        #endregion temp vars

        public void SetData(GameReward reward, int day, int rewardDay)
        {
            if (rewardReceivedImage)
            {
                rewardReceivedImage.gameObject.SetActive((day < rewardDay));
            }
            if (currentRewardImage)
            {
                currentRewardImage.gameObject.SetActive(day == rewardDay);
            }
            if (getButton)
            {
                getButton.gameObject.SetActive(day == rewardDay);
            }
            if (dayText && day == rewardDay)
            {
                dayText.text = "GET!!!";
                dayText.enabled = false;
            }

            SetOpacity(day < rewardDay);
        }

        private void SetOpacity(bool received)
        {
            float opacity = (received) ? (float) opacityReceived/100f : 1f;
            Image[] images = GetComponentsInChildren<Image>();
            Text[] texts = GetComponentsInChildren<Text>();

            foreach (var item in images)
            {
                SetOpacity(item, opacity);
            }

            foreach (var item in texts)
            {
                SetOpacity(item, opacity);
            }
        }

        private void SetOpacity(Image image, float opacity)
        {
            if (image)
            {
                Color c = image.color;
                image.color = new Color(c.r, c.g, c.b, opacity);
            }
        }

        private void SetOpacity(Text text, float opacity)
        {
            if (text)
            {
                Color c = text.color;
                text.color = new Color(c.r, c.g, c.b, opacity);
            }
        }
    }
}
