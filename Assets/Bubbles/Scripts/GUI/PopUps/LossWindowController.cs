using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class LossWindowController : PopUpsController
    {
        public Text missionDescriptionText;

        [SerializeField]
        private bool useAds = true;

        #region temp vars
        private static int failsCounter = 0;
        float volume = 0;
        private GameBoard MBoard { get { return GameBoard.Instance; } }
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private BubblesGuiController MGui { get { return BubblesGuiController.Instance; } }
        private SoundMaster MSound { get { return SoundMaster.Instance; } }
        private AdsControl Ads => AdsControl.Instance;
        #endregion temp vars

        #region regular
        private void Start()
        {
            failsCounter++;
            if (useAds && failsCounter % 2 == 0)
            {
                Debug.Log("show ads");
                if(Ads)  Ads.ShowInterstitial(
                   () =>
                   {
                       MSound.ForceStopMusic();
                   },
                   () =>
                   {
                       MSound.PlayCurrentMusic();
                   });
            }
        }
        #endregion regular

        public override void RefreshWindow()
        {
            string description = (MPlayer.LcSet) ? MPlayer.LcSet.levelMission.Description : "";
            missionDescriptionText.text = description;
            missionDescriptionText.enabled = !string.IsNullOrEmpty(description);
            base.RefreshWindow();
        }

        public void Cancel_Click()
        {
            CloseWindow();
            SceneLoader.Instance.LoadScene(0);
        }

        public void Retry_Click()
        {
            CloseWindow();
            SceneLoader.Instance.LoadScene(0);
        }

    }
}