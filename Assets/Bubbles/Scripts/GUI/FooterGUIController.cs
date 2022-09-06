using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Mkey
{
    public class FooterGUIController : MonoBehaviour
    {
        //[SerializeField]
        //private FooterBoosterHelper footerBoosterPrefab;
        [SerializeField]
        private RectTransform BoostersParent;
        [SerializeField]
        private GameObject FooterContent;
        [SerializeField]
        private Text MovesCountText;

        #region temp vars
        private GameBoard MBoard { get { return GameBoard.Instance; } }
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private BubblesGuiController MGui { get { return BubblesGuiController.Instance; } }
        private GameConstructSet GCSet { get { return MPlayer.gcSet; } }
        private GameObjectsSet GOSet { get { return GCSet.GOSet; } }
        private LevelConstructSet LCSet { get { return MPlayer.LcSet; } }
        #endregion temp vars
        public static FooterGUIController Instance;

        #region regular
        void Awake()
        {
            if (Instance) Destroy(Instance.gameObject);
            Instance = this;
        }

        private IEnumerator Start()
        {
            while (!GameBoard.Instance) yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            if (GameBoard.gMode == GameMode.Edit)
            {
                gameObject.SetActive(false);
            }
            else
            {
                CreateBoostersPanel();
            }
        }
        #endregion regular

        private void CreateBoostersPanel()
        {
            FooterBoosterHelper[] fBH = BoostersParent.GetComponentsInChildren<FooterBoosterHelper>();
            foreach (FooterBoosterHelper item in fBH)
            {
                DestroyImmediate(item.gameObject);
            }
            foreach (var item in GOSet.BoosterObjects)
            {
                item.CreateFooterBooster(BoostersParent); 
            }
        }
        
        /// <summary>
        /// Set all interactable as activity
        /// </summary>
        /// <param name="activity"></param>
        public void SetControlActivity(bool activity)
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }
    }
}