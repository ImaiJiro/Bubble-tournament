using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Mkey
{
	public class StarChestController : MonoBehaviour
	{
        [SerializeField]
        private int targetStarsCount = 20;
        [SerializeField]
        private List <GameReward> rewards;

        #region properties
        public int StarsInChest { get; private set; }
        public int StarsTarget { get { return targetStarsCount; } }
        public bool TargetAchieved { get { return targetStarsCount <= StarsInChest; } }
        public bool Started { get; private set; }
        internal IEnumerable<GameReward> Rewards { get { return rewards.AsReadOnly(); } }
        #endregion properties

        #region temp vars
        private string starsCommitSaveKey = "cheststarsforest";
        private BubblesGuiController MGui { get { return BubblesGuiController.Instance; } }
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }

        #endregion temp vars

        #region events
        public Action<int, int> LoadStarsEvent;
        public Action<int, int> ChangeStarsEvent;
        #endregion events

        public static StarChestController Instance;

        #region regular
        void Awake()
        {
            if (Instance) Destroy(gameObject);
            else
            {
                Instance = this;
            }
        }

        private IEnumerator Start()
		{
            Validate();
            LoadStarsInChest();
            while (!MPlayer) yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            MPlayer.PassLevelEvent += PassLevelEventHandler;
            Started = true;
        }

        private void OnDestroy()
        {
            if(MPlayer) MPlayer.PassLevelEvent -= PassLevelEventHandler;
        }

        private void LoadStarsInChest()
        {
            StarsInChest = PlayerPrefs.GetInt(starsCommitSaveKey, 0);
            LoadStarsEvent?.Invoke(StarsInChest, StarsTarget);
        }

        private void OnValidate()
        {
            Validate();
        }
        #endregion regular

        internal void AddLevelStarsInChest(int stars)
        {
            StarsInChest += stars;
            StarsInChest = Mathf.Clamp(StarsInChest, 0, targetStarsCount);
            PlayerPrefs.SetInt(starsCommitSaveKey, StarsInChest);
            ChangeStarsEvent?.Invoke(StarsInChest, StarsTarget);
        }

        private void PassLevelEventHandler()
        {
            AddLevelStarsInChest(MPlayer.StarCount);
        }

        private void Validate()
        {
            targetStarsCount = Mathf.Max(targetStarsCount, 3);
        }

        public void ResetData()
        {
            StarsInChest = 0;
            PlayerPrefs.SetInt(starsCommitSaveKey, StarsInChest);
            ChangeStarsEvent?.Invoke(StarsInChest, StarsTarget);
        }

        public void AddCoins(int count)
        {
            MPlayer.AddCoins(count);
        }

        public void AddLifes(int count)
        {
            MPlayer.AddLifes(count);
        }

        public void AddBoosterPack(BoosterPack boosterPack)
        {
            if (!boosterPack || !boosterPack.boosterFunc) return;
            boosterPack.boosterFunc.AddCount(boosterPack.count);
        }

        public void ApplyChestReward(GameReward gameReward)
        {
            gameReward.ApplyRewardEvent?.Invoke();
            ResetData();
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(StarChestController))]
    public class StarChestControllerEditor : Editor
    {
        private bool test = true;
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            StarChestController t = (StarChestController)target;

            if (!EditorApplication.isPlaying)
            {
                if (test = EditorGUILayout.Foldout(test, "Test"))
                {
                    EditorGUILayout.BeginHorizontal("box");
                    if (GUILayout.Button("Reset chest"))
                    {
                        t.ResetData();
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                if (test = EditorGUILayout.Foldout(test, "Test"))
                {
                    EditorGUILayout.BeginHorizontal("box");
                    if (GUILayout.Button("Add star in chest"))
                    {
                        t.AddLevelStarsInChest(1);
                    }
                    if (GUILayout.Button("Remove star from chest"))
                    {
                        t.AddLevelStarsInChest(-1);
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
#endif
}
