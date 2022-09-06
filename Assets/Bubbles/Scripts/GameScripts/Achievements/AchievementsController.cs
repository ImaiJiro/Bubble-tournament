using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
	public class AchievementsController : MonoBehaviour
	{
        #region properties
        public IList<Achievement> Achievements => achievements.AsReadOnly();  
        public bool HaveTargetAchieved { get; private set; }
        public static AchievementsController Instance { get; private set; }
        #endregion properties

        #region events
        public Action<bool> HaveTargetAchievedEvent;
        #endregion events

        #region temp vars
        private List<Achievement> achievements;
        #endregion temp vars

        #region regular
        private void Start()
		{
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            achievements = new List<Achievement>(GetComponentsInChildren<Achievement>());

            foreach (var item in achievements)
            {
                item.Load();
                item.ChangeCurrentCountEvent += (c, t) => { CheckState(); };
                item.RewardReceivedEvent += (r) => { CheckState(); };
            }
            CheckState();
        }
		#endregion regular

        private void CheckState()
        {
            bool temp = HaveTargetAchieved;
            HaveTargetAchieved = false;
            foreach (var item in achievements)
            {
                if (item.TargetAchieved && !item.RewardReceived) 
                {
                    HaveTargetAchieved = true;
                    break;
                }
            }

           // if (temp != HaveTargetAchieved)
                HaveTargetAchievedEvent?.Invoke(HaveTargetAchieved);
        }
	}
}
