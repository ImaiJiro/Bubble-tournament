using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
	public class AchievementsLine : MonoBehaviour
	{
        [SerializeField]
        private Text countText;
        [SerializeField]
        private Text rewardCountText;
        [SerializeField]
        private GameObject rewardGroup;
        [SerializeField]
        private Button getButton;

        private Achievement achievement;
        #region temp vars

        #endregion temp vars


        #region regular
        private void Start()
        {
            achievement.RewardReceivedEvent += RewardReceivedEventHandler;
            achievement.ChangeCurrentCountEvent += RefreshCount;
            achievement.ResetReceivedEvent += RefreshRewardGroup;
        }
		
		private void OnDestroy()
        {
            if (achievement)
            {
                achievement.RewardReceivedEvent -= RewardReceivedEventHandler;
                achievement.ChangeCurrentCountEvent -= RefreshCount;
                achievement.ResetReceivedEvent -= RefreshRewardGroup;
            }
        }
		#endregion regular

        public AchievementsLine CreateInstance(RectTransform parent, Achievement achievement)
        {
            if (!parent || !achievement) return null;
          
            AchievementsLine achievementsLine = Instantiate(this, parent);
            achievementsLine.transform.localScale = Vector3.one;
            achievementsLine.achievement = achievement;

            achievementsLine.RefreshCount(achievement.CurrentCount, achievement.TargetCount);
            achievementsLine.RefreshRewardGroup();

            return achievementsLine;
        }

        private void RefreshCount(int currentCount, int targetCount)
        {
            if (countText)
            {
                countText.text = currentCount + "/" + targetCount;
            }
            RefreshRewardGroup();
        }

        private void RefreshRewardGroup()
        {
            if (getButton) getButton.gameObject.SetActive(achievement.TargetAchieved && !achievement.RewardReceived);
            if (rewardGroup) rewardGroup.SetActive(!achievement.TargetAchieved && !achievement.RewardReceived);
            if (rewardCountText) rewardCountText.text = achievement.AchReward.ToString();
        }

        public void GetButton_Click()
        {
            if (achievement.RewardReceived)
            {
                Debug.Log("reward received");
                return;
            }
            if (achievement.TargetAchieved) achievement.OnGetRewardEvent();
        }

        #region event handlers
        private void RewardReceivedEventHandler(int achReward)
        {
            RefreshRewardGroup();
        }
        #endregion event handlers
    }
}
