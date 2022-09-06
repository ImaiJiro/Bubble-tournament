using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Mkey
{
	public class UseBoosterAchievement : Achievement
	{
        [SerializeField]
        private BoosterFunc boosterFunc;
        #region events

        #endregion events

        #region temp vars
        private bool dLog = true;
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private BubblesGuiController MGui { get { return BubblesGuiController.Instance; } }
        private SoundMaster MSound { get { return SoundMaster.Instance; } }
        #endregion temp vars

        #region properties

        #endregion properties

        #region regular
        public override void Load()
        {
            LoadRewardReceived();
            LoadCurrentCount();

            GameEvents.ApplyBoosterEvent += UseBoosterEventHandler;
            RewardReceivedEvent +=(r)=> 
            {
                MPlayer.AddCoins(r);
            };

            ChangeCurrentCountEvent += (cc, tc)=>{  };
        }

        private void OnDestroy()
        {
            GameEvents.ApplyBoosterEvent -= UseBoosterEventHandler;
        }
        #endregion regular

        public override string GetUniqueName()
        {
            return "usebooster_" + ((boosterFunc)? boosterFunc.ID.ToString() : "");
        }

        private void UseBoosterEventHandler(int id)
        {
            Debug.Log("use booster event: " + id);
            if (boosterFunc && id == boosterFunc.ID)
            {
                IncCurrentCount();
                Debug.Log("increase count: " + CurrentCount);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UseBoosterAchievement))]
    public class UseBoosterAchievementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            UseBoosterAchievement t = (UseBoosterAchievement)target;
            t.DrawInspector();
        }
    }
#endif
}
