using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class MainObject : GridObject
    {
        public Match match;
        [SerializeField]
        private bool isDestroyable;
        [SerializeField]
        private bool canUseAsShootBubbles;

        public Sprite[] protectionStateImages;
        public GameObject collectAnimPrefab;
        public GameObject hitAnimPrefab;
        public GUIFlyer scoreFlyerPrefab;
        public static int FallDownCount { get; private set; }

        #region properties     
        public bool CanUseAsShootBubbles => canUseAsShootBubbles;
        public bool IsExploidable
        {
            get; internal set;
        }
        public bool IsMatchedById
        {
            get { return (match == Match.ById); }
        }
        public bool IsDestroyable => isDestroyable;
        public int Protection
        {
            get { return (IsDestroyable) ? protectionStateImages.Length + 1 - Hits : 1; }
        }
        public GridCell ParentCell => GetComponentInParent<GridCell>();
        public OverlayObject Overlay => ParentCell ? ParentCell.Overlay : null;
        #endregion properties    

        private static PhysicsMaterial2D physMat;

        /// <summary>
        /// Return true if object IDs is Equal
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(MainObject other)
        {
            return (other && ID == other.ID);
        }

        #region override
        /// <summary>
        /// Create new MainObject for gridcell
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="mData"></param>
        /// <param name="addCollider"></param>
        /// <param name="radius"></param>
        /// <param name="isTrigger"></param>
        /// <returns></returns>
        public override GridObject Create(GridCell parent, Action<int> TargetCollectEvent)
        {
            if (!parent) return null;
            if (parent.Mainobject)
            {
                GameObject old = parent.Mainobject.gameObject;
                DestroyImmediate(old);
            }
            MainObject gridObject = Instantiate(this, parent.transform);
            if (!gridObject) return null;
            gridObject.transform.localScale = Vector3.one;
            gridObject.transform.localPosition = Vector3.zero;
            gridObject.SRenderer = gridObject.GetComponent<SpriteRenderer>();
#if UNITY_EDITOR
            gridObject.name = "main " + parent.ToString();
#endif
            gridObject.TargetCollectEvent = TargetCollectEvent;
            gridObject.SetToFront(false);

            Collider2D c = gridObject.GetComponent<Collider2D>();
            if (c) c.enabled = (GameBoard.gMode == GameMode.Play);
            return gridObject;
        }

        public override void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            collectSequence = new TweenSeq();

            collectSequence.Add((callBack) =>
            {
                TweenExt.DelayAction(gameObject, 0.05f, () =>
               {
                   SetToFront(true);
                   if (collectAnimPrefab)
                   {
                       GameObject gA = Instantiate(collectAnimPrefab);
                       gA.transform.localScale = transform.lossyScale;
                       gA.transform.position = transform.position;
                   }
                   callBack();
               });
            });

            collectSequence.Add((callBack) =>
            {

                GameObject aP = hitAnimPrefab;
                callBack();
            });

            collectSequence.Add((callBack) =>
            {
                if (showPrivateScore && WithScore && scoreFlyerPrefab) InstantiateScoreFlyer(scoreFlyerPrefab, privateScore);
                if (addPrivateScore && WithScore) BubblesPlayer.Instance.AddScore(privateScore);
                GameEvents.CollectGridObject?.Invoke(ID);
                completeCallBack?.Invoke();
                Destroy(gameObject, 0.05f);
                callBack();
            });

            collectSequence.Start();
        }

        public override void ShootHit(Action completeCallBack)
        {
            if (IsDestroyable)
            {
                Debug.Log("hit");
                Hits++;
                Debug.Log("hits: " + Hits);
                if (protectionStateImages.Length > 0)
                {
                    int i = Mathf.Min(Hits - 1, protectionStateImages.Length - 1);
                    SRenderer.sprite = protectionStateImages[i];
                }

                if (hitAnimPrefab)
                {
                    Creator.InstantiateAnimPrefabAtPosition(hitAnimPrefab, transform.parent, transform.position, SortingOrder.MainExplode, true, null);
                }

                if (Protection <= 0)
                {
                    hitDestroySeq = new TweenSeq();

                    SetToFront(true);

                    hitDestroySeq.Add((callBack) =>
                    {
                        SimpleTween.Value(gameObject, 0, 1, 0.050f).AddCompleteCallBack(callBack);
                    });

                    hitDestroySeq.Add((callBack) =>
                    {
                        completeCallBack?.Invoke();
                        Destroy(gameObject);
                        callBack();
                    });

                    hitDestroySeq.Start();
                }
            }
            else
            {
                completeCallBack?.Invoke();
            }
        }

        public override void CancellTweensAndSequences()
        {
            base.CancellTweensAndSequences();
        }

        public override void SetToFront(bool set)
        {
            if (!SRenderer) SRenderer = GetComponent<SpriteRenderer>();
            if (!SRenderer) return;
            if (set)
                SRenderer.sortingOrder = SortingOrder.MainToFront;
            else
                SRenderer.sortingOrder = SortingOrder.Main;
        }

        public override void Remove()
        {
            if (Overlay) ParentCell.Overlay.Remove();
            transform.parent = null;
            DestroyImmediate(gameObject);
        }

        public override string ToString()
        {
            return "MainObject: " + ID;
        }
        #endregion override

        #region fall down
        public void FallDownCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
            SetToFront(true);
            FallDownCount++;
            FallDown
                (
                () =>
                {
                    if (collectAnimPrefab)
                    {
                        GameObject gA = Instantiate(collectAnimPrefab);
                        gA.transform.localScale = transform.lossyScale;
                        gA.transform.position = transform.position;
                    }
                    if (showPrivateScore && WithScore && scoreFlyerPrefab) InstantiateScoreFlyer(scoreFlyerPrefab, privateScore);
                    if (addPrivateScore && WithScore) BubblesPlayer.Instance.AddScore(privateScore);
                    GameEvents.CollectGridObject?.Invoke(ID);
                    completeCallBack?.Invoke();
                }
                );
        }

        protected void FallDown(Action completeCallBack)
        {
            Rigidbody2D rB = gameObject.AddComponent<Rigidbody2D>();
            rB.mass = UnityEngine.Random.Range(1, 2);
            rB.isKinematic = false;
            rB.bodyType = RigidbodyType2D.Dynamic;
            if (!physMat)
            {
                physMat = new PhysicsMaterial2D();
                physMat.bounciness = 1f;
            }
            rB.sharedMaterial = physMat;
            rB.AddForce(UnityEngine.Random.onUnitSphere * 10f, ForceMode2D.Impulse);
            CircleCollider2D cC = gameObject.GetComponent<CircleCollider2D>();
            if (cC) cC.isTrigger = false;
            // rB.drag = 1;
            StartCoroutine(FallDownDestrtoy(completeCallBack));
        }

        private IEnumerator FallDownDestrtoy(Action completeCallBack)
        {
            while (transform.position.y > -10)
            {
                yield return new WaitForEndOfFrame();
            }
            completeCallBack?.Invoke();
            FallDownCount--;
            Destroy(gameObject);
        }
        #endregion fall down
    }
}