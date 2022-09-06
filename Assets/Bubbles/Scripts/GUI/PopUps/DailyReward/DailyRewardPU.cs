using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
	public class DailyRewardPU : PopUpsController
	{
        [SerializeField]
        private DailyRewardLine[] dailyRewardLines;

        #region temp vars
        private DailyRewardController DRC { get { return DailyRewardController.Instance; } }
        private GameReward reward;
        private int rewDay = -1;
        #endregion temp vars


        public override void RefreshWindow()
        {
            List<GameReward> rewards = new List<GameReward>(DRC.Rewards);
            rewDay = DRC.RewardDay;
            if (rewDay < 0) return;
            int rewDayCl = DRC.RepeatingReward ? rewDay % rewards.Count : Mathf.Clamp(rewDay, 0, rewards.Count - 1);
            reward = rewards[rewDayCl];

            for (int i = 0; i < dailyRewardLines.Length; i++)
            {
                int rI = DRC.RepeatingReward ? i % rewards.Count : Mathf.Clamp(i, 0, rewards.Count - 1);
                var item = rewards[rI];
                dailyRewardLines[i].SetData(item, i, rewDayCl);
            }
            base.RefreshWindow();
        }

        public void Apply_Click()
        {
            if (rewDay >= 0) DRC.ApplyReward(reward);
            SetControlActivity(false);
            TweenExt.DelayAction(gameObject, 1.5f, ()=> { CloseWindow(); }); 
        }
    }
}
