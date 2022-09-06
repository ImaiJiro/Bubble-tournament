using System;
using System.Collections;
using UnityEngine;

namespace Mkey
{
    public class GridObject : MonoBehaviour
    {
        [Tooltip("Picture that is used in GUI")]
        [SerializeField]
        private Sprite GuiObjectImage;
        [SerializeField]
        private bool withScore;

        [HideInInspector]
        [SerializeField]
        private int id = Int32.MinValue;

        #region construct object properties
        public bool canUseAsTarget;
        #endregion construct object properties

        #region protected temp vars
        protected TweenSeq collectSequence;
        protected TweenSeq hitDestroySeq;
        protected SoundMaster MSound { get { return SoundMaster.Instance; } }
        private static Canvas parentCanvas;
        #endregion protected temp vars

        #region properties
        public int ID { get { return id; } private set { id = value; } }
        protected SpriteRenderer SRenderer { get; set; }
        public Sprite ObjectImage { get { SpriteRenderer sr = GetComponent<SpriteRenderer>(); return (sr) ? sr.sprite : null; } }
        public Sprite GuiImage { get { return (GuiObjectImage) ? GuiObjectImage : ObjectImage; } }
        public Sprite GuiImageHover { get { return GuiImage; } }
        public int Hits { get; set; }
        public bool WithScore => withScore;
        #endregion properties

        #region events
        public Action<int> TargetCollectEvent;
        #endregion events

        #region regular
        void OnDestroy()
        {
            CancellTweensAndSequences();
        }
        #endregion regular

        #region common
        internal bool ReferenceEquals(GridObject other)
        {
            return System.Object.ReferenceEquals(this, other);//Determines whether the specified Object instances are the same instance.
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        internal void SetLocalScale(float scale)
        {
            transform.localScale = (transform.parent) ? transform.parent.localScale * scale : new Vector3(scale, scale, scale);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        internal void SetLocalScaleX(float scale)
        {
            Vector3 parLS = (transform.parent) ? transform.parent.localScale : Vector3.one;
            float ns = parLS.x * scale;
            transform.localScale = new Vector3(ns, parLS.y, parLS.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scale"></param>
        internal void SetLocalScaleY(float scale)
        {
            Vector3 parLS = (transform.parent) ? transform.parent.localScale : Vector3.one;
            float ns = parLS.y * scale;
            transform.localScale = new Vector3(parLS.x, ns, parLS.z);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alpha"></param>
        internal void SetAlpha(float alpha)
        {
            if (!SRenderer) GetComponent<SpriteRenderer>();
            if (SRenderer)
            {
                Color c = SRenderer.color;
                Color newColor = new Color(c.r, c.g, c.b, alpha);
                SRenderer.color = newColor;
            }
        }

        internal void InstantiateScoreFlyer(GUIFlyer scoreFlyerPrefab, int score)
        {
            if (!scoreFlyerPrefab) return;
            if (!parentCanvas)
            {
                Debug.Log("no canvas");
                GameObject gC = GameObject.Find("CanvasMain");
                if (gC) parentCanvas = gC.GetComponent<Canvas>();
                if (!parentCanvas) parentCanvas = FindObjectOfType<Canvas>();
            }

            GUIFlyer flyer = scoreFlyerPrefab.CreateFlyer(parentCanvas, score.ToString());
            if (flyer)
            {
                flyer.transform.localScale = transform.lossyScale;
                flyer.transform.position = transform.position;
            }
        }
        #endregion common

        #region virtual
        /// <summary>
        /// Collect object with shootarea
        /// </summary>
        /// <param name="completeCallBack"></param>
        /// <param name="showPrivateScore"></param>
        /// <param name="addPrivateScore"></param>
        /// <param name="decProtection"></param>
        /// <param name="privateScore"></param>
        public virtual void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {

        }

        /// <summary>
        /// Hit object from shoot bubble
        /// </summary>
        /// <param name="completeCallBack"></param>
        public virtual void ShootHit( Action completeCallBack)
        {

        }

        /// <summary>
        /// Cancel all tweens and sequences
        /// </summary>
        public virtual void CancellTweensAndSequences()
        {
            if (collectSequence != null) collectSequence.Break();
            if (hitDestroySeq != null) hitDestroySeq.Break();
            SimpleTween.Cancel(gameObject, false);
        }

        public virtual void SetToFront(bool set)
        {

        }

        public virtual GridObject Create(GridCell parent, Action<int> TargetCollectEvent)
        {
            return parent ? Instantiate(this, parent.transform) : Instantiate(this);
        }

        public virtual GridObject Create(GridCell parent, int hitsCount, Action<int> TargetCollectEvent)
        {
            return parent ? Instantiate(this, parent.transform) : Instantiate(this);
        }

        public virtual Sprite[] GetProtectionStateImages()
        {
            return null;
        }

        public virtual void Remove()
        {
            transform.parent = null;
            DestroyImmediate(gameObject);
        }
        #endregion virtual

        public void Enumerate(int id)
        {
            this.id = id;
        }
    }

    [Serializable]
    public class GridObjectState
    {
        public int id;
        public int hits;

        public GridObjectState(int id, int hits)
        {
            this.id = id;
            this.hits = hits;
        }
    }
}
