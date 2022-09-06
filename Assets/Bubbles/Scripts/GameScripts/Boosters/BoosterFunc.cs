using System;
using UnityEngine;

namespace Mkey
{
    public class BoosterFunc : MonoBehaviour
    {
        [SerializeField]
        private Sprite GuiObjectImage;
        public FooterBoosterHelper footerBrefab;
        [HideInInspector]
        [SerializeField]
        private new string name;

        #region events
        public Action FooterClickEvent;
        public Action <int> ChangeCountEvent; //count
        public Action <int> LoadEvent; // count
        public Action <BoosterFunc> ActivateEvent;
        public Action DeActivateEvent;
        #endregion events

        #region private
        private SpriteRenderer sRenderer;
        private string SaveKey => "booster_id_" + ID.ToString();
        [HideInInspector]
        [SerializeField]
        private int id = Int32.MinValue;
        #endregion private

        #region properties
        public static BoosterFunc ActiveBooster { get; private set; }
        public string Name { get { return name; } }
        public SpriteRenderer SRenderer { get { return (sRenderer) ? sRenderer : GetComponent<SpriteRenderer>(); } }
        public Sprite ObjectImage { get {  return (SRenderer) ? SRenderer.sprite : null; } }
        public int ID { get { return id; } private set { id = value; } }
        public Sprite GuiImage { get { return (GuiObjectImage) ? GuiObjectImage : ObjectImage; } }
        public Sprite GuiImageHover { get { return GuiImage; } }
        public int Count { get; private set; }
        public bool Use => true;
        public bool IsActive => (ActiveBooster && ID == ActiveBooster.ID); 
        protected BubblesPlayer MPlayer => BubblesPlayer.Instance;
        protected BubblesShooter BS => BubblesShooter.Instance;
        #endregion properties

        #region virtual
        public virtual void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            Debug.Log("base apply booster");
        }

        public virtual CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            Debug.Log("base get shoot area");
            CellsGroup cG = new CellsGroup();
            return cG;
        }
        #endregion virtual
  
        #region count
        public void AddCount(int count)
        {
            SetCount(count + Count);
        }

        public void SetCount(int count)
        {
            count = Mathf.Max(0, count);
            bool changed = (count != Count);
            Count = count;

            if (changed)
            {
                SaveCount();
                ChangeCountEvent?.Invoke(count);
            }
        }
  
        private void LoadCount()
        {
            if (MPlayer && MPlayer.SaveData)
            {
                Count = PlayerPrefs.GetInt(SaveKey, 0);
            }
            else
            {
                Count = 0;
            }
            LoadEvent?.Invoke(Count);
        }

        private void SaveCount()
        {
            if (MPlayer && MPlayer.SaveData)
            {
                PlayerPrefs.SetInt(SaveKey, Count);
            }
        }
        #endregion count

        #region handlers
        public static void ShootEventHandler()
        {
            if (ActiveBooster != null)
            {
                ActiveBooster.AddCount(-1);
                ActiveBooster = null;
            }
        }

        public void FooterClickEventHandler()
        {
            if (!IsActive && Count > 0)     // activate booster
            {
                ActiveBooster?.DeActivateBooster(); // ?. - work
                ActivateBooster();
            }
            else if (IsActive)              // deactivate booster
            {
                DeActivateBooster();
            }
            else if (!IsActive && Count == 0 && ActiveBooster != null) // open shop  
            {
                ActiveBooster.DeActivateBooster();
            }
        }
        #endregion handlers

        #region common
        public void ActivateBooster()
        {
            ActiveBooster = this;
            ActivateEvent?.Invoke(this);
        }

        /// <summary>
        /// Set  ActiveBooster = null, raise DeActivateEvent
        /// </summary>
        public void DeActivateBooster()
        {
            ActiveBooster = null;
            DeActivateEvent?.Invoke();
        }

        public FooterBoosterHelper CreateFooterBooster(RectTransform parent)
        {
            if (!footerBrefab || !parent) return null;
            FooterBoosterHelper footerBooster = Instantiate(footerBrefab, parent);
            footerBooster.booster = this;
            return footerBooster;
        }

        /// <summary>
        /// enumerate and load count by ID
        /// </summary>
        /// <param name="id"></param>
        public void Enumerate(int id)
        {
            this.id = id;
            LoadCount();
        }
        #endregion common

        #region override
        public override string ToString()
        {
            return (Name + " : " + Count.ToString() + " items");
        }
        #endregion override
    }
}