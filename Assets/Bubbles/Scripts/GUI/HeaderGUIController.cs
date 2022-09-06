using System;
using UnityEngine;
using UnityEngine.UI;

namespace Mkey
{
    public class HeaderGUIController : MonoBehaviour
    {
        [Header("Stars")]
        [SerializeField]
        private Image star1;
        [SerializeField]
        private Image star2;
        [SerializeField]
        private Image star3;
        [SerializeField]
        private GameObject starPrefab;


        [Space(8)]
        [Header("Score strip")]
        [SerializeField]
        private ProgressSlider ScoreSlider;
        [SerializeField]
        private Text ScoreCount;

        [Space(8)]
        [Header("Targets")]
        [SerializeField]
        private GUIObjectTargetHelper targetFish;
        [SerializeField]
        private GUIObjectTargetHelper targetLopTopRow;
        [SerializeField]
        private GUIObjectTargetHelper targetRaiseAcnhor;
        [SerializeField]
        private GUIObjectTargetHelper targetClock;

        #region temp vars
        private GameObject fullStar_1;
        private GameObject fullStar_2;
        private GameObject fullStar_3;
        private int oldCount = 0;

        private GameBoard MBoard { get { return GameBoard.Instance; } }
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private SoundMaster MSound { get { return SoundMaster.Instance; } }
        #endregion temp vars

        public static HeaderGUIController Instance;

        #region regular
        private void Awake()
        {
            if (Instance) Destroy(Instance.gameObject);
            Instance = this;
        }

        private void Start()
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                gameObject.SetActive(false);
                return;
            }
            Refresh();

            //set delegates
            if (MPlayer)
            {
                MPlayer.ChangeScoreEvent += RefreshScoreStrip;
                MPlayer.ChangeStarsEvent += RefreshStars;
            }
        }

        private void OnDestroy()
        {
            if (MPlayer)
            {
                MPlayer.ChangeScoreEvent -= RefreshScoreStrip;
                MPlayer.ChangeStarsEvent -= RefreshStars;
            }
            if (ScoreCount) SimpleTween.Cancel(ScoreCount.gameObject, false);
            if (ScoreSlider) SimpleTween.Cancel(ScoreSlider.gameObject, false);
        }
        #endregion regular

        public void Refresh()
        {
            RefreshTargets();
            RefreshStars(MPlayer.StarCount);
            RefreshScoreStrip(MPlayer.LevelScore, MPlayer.AverageScore);
        }

        public void SetControlActivity(bool activity)
        {
            Button[] buttons = GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        #region stars
        private void RefreshStars(int count)
        {
            SetStars(count, null);
        }

        /// <summary>
        /// Instantiate stars objects with any animation
        /// </summary>
        /// <param name="starCount"></param>
        /// <param name="completeCallBack"></param>
        private void SetStars(int starCount, Action completeCallBack)
        {
            if (fullStar_1 && starCount < 1) Destroy(fullStar_1);
            if (fullStar_2 && starCount < 2) Destroy(fullStar_2);
            if (fullStar_3 && starCount < 3) Destroy(fullStar_3);

            TweenSeq ts = new TweenSeq();

            if (!fullStar_1 && starCount > 0)
            {
                ts.Add((callBack) =>
                {
                    fullStar_1 =  InstantiateNewStar(starPrefab,  star1.transform);
                    AnimateNewStar(fullStar_1, callBack);
                });
              
            }

            if (!fullStar_2 && starCount > 1)
            {
                ts.Add((callBack) =>
                {
                    fullStar_2 = InstantiateNewStar(starPrefab,  star2.transform);
                    AnimateNewStar(fullStar_2, callBack);
                });
            }

            if (!fullStar_3 && starCount > 2)
            {
                ts.Add((callBack) =>
                {
                    fullStar_3 = InstantiateNewStar(starPrefab, star3.transform);
                    AnimateNewStar(fullStar_3, callBack);
                });
            }

            ts.Add((callBack) =>
            {
                completeCallBack?.Invoke();
                callBack();
            });

            ts.Start();
        }

        private GameObject InstantiateNewStar(GameObject prefab, Transform target)
        {
            if (!prefab || !target) return null;
            GameObject  star = Instantiate(prefab, target.position, Quaternion.identity);
            star.transform.localScale = target.lossyScale;
            star.transform.parent = target;
            return star;
        }

        private void AnimateNewStar(GameObject star, Action completeCallBack)
        {
            if (!star)
            {
                completeCallBack?.Invoke();
                return;
            }

            SimpleTween.Value(star, 0, 1, 0.5f).SetOnUpdate((val) =>
            {
              if(star) star.transform.localScale = new Vector3(val, val, val);
            }).AddCompleteCallBack(() =>
            {
                completeCallBack?.Invoke();
            }).SetEase(EaseAnim.EaseOutBounce);
        }

        public void RefreshTargets()
        {
            if (GameBoard.Instance && GameBoard.Instance.WController != null)
            {
                int c = 0;
                int tc = 0;
                LevelType lType = GameBoard.Instance.WController.GameLevelType;
                GameBoard.Instance.WController.GetCurrTarget(out c, out tc);
                targetFish.gameObject.SetActive(lType == LevelType.FishLevel);
                targetLopTopRow.gameObject.SetActive(lType == LevelType.LoopTopRowLevel);
                targetRaiseAcnhor.gameObject.SetActive(lType == LevelType.AnchorLevel);
                targetClock.gameObject.SetActive(lType == LevelType.TimeLevel);

                switch (lType)
                {
                    case LevelType.LoopTopRowLevel:
                        targetLopTopRow.SetData(c, tc);
                        break;
                    case LevelType.TimeLevel:
                        //targetLopTopRow.SetData(c, tc);
                        break;
                    case LevelType.AnchorLevel:
                        targetRaiseAcnhor.SetData(c, tc);
                        break;
                    case LevelType.FishLevel:
                        targetFish.SetData(c, tc);
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion stars

        #region score scorestrip  
        private void RefreshScoreStrip(int score, int averageScore)
        {
            // refresh score text
            if (ScoreCount)
            {
                int newCount = score;
                SimpleTween.Cancel(ScoreCount.gameObject, false);
                SimpleTween.Value(ScoreCount.gameObject, oldCount, newCount, 0.5f).SetOnUpdate((float val) =>
                {
                    oldCount = (int)val;
                    if (ScoreCount) ScoreCount.text = oldCount.ToString();
                });
            }

            //Refresh score strip
            if (!ScoreSlider) return;
            SimpleTween.Cancel(ScoreSlider.gameObject, false);
            float amount = (averageScore > 0) ? (float)score / (float)(averageScore) : 0;

            SimpleTween.Value(ScoreSlider.gameObject, ScoreSlider.FillAmount, amount, 0.3f).SetOnUpdate((float val) =>
            {
              if(ScoreSlider)  ScoreSlider.SetFillAmount( val);

            }).SetEase(EaseAnim.EaseOutCubic);
        }
        #endregion score scorestrip
    }
}