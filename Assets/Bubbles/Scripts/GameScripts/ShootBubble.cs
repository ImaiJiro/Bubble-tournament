using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class ShootBubble : TouchPadMessageTarget
    {
        #region prorerties
        public Sprite sprite { get; private set; }
        private GridObject GObject { get { return (gridObject) ? gridObject : gridObject = GetComponent<GridObject>(); } }        
        private BoosterFunc Booster { get { return (booster) ? booster : booster = GetComponent<BoosterFunc>(); } }
        public int ID
        {
            get
            {
                if (GObject) return GObject.ID;
                if (Booster) return Booster.ID;
                return 0;
            }
        }
        #endregion prorerties

        #region  properties show selectors
        [SerializeField]
        private bool showHitTargetSelector = true;
        [SerializeField]
        private bool showFreeTargetSelector = true;
        [SerializeField]
        private bool showShootAreaSelector = true;
        #endregion show selectors

        #region  properties show selectors
        /// <summary>
        /// If has boosterfunc return boosterunc ShowHitTargetSelector else private
        /// </summary>
        public bool ShowHitTargetSelector { get {return showHitTargetSelector; } }
        /// <summary>
        /// If has boosterfunc return boosterunc ShowFreeTargetSelector else private
        /// </summary>
        public bool ShowFreeTargetSelector { get { return  showFreeTargetSelector; } }
        /// <summary>
        /// If has boosterfunc return boosterunc ShowShootAreaSelector else private
        /// </summary>
        public bool ShowShootAreaSelector { get { return showShootAreaSelector; } }
        #endregion properties

        #region events
        public Action<ShootBubble> SBPointerDownEvent;
        #endregion events

        #region temp vars
        private bool debug = false;
        private GridObject gridObject;
        private BoosterFunc booster;
        #endregion temp vars

        internal void SetData(int sortingOrder, bool enableCollider, Action onClick)
        {
            SpriteRenderer shootBubbleSR = GetComponent<SpriteRenderer>();
            sprite = shootBubbleSR.sprite;
            shootBubbleSR.sortingOrder = sortingOrder;
            CircleCollider2D cC = gameObject.GetOrAddComponent<CircleCollider2D>();
            cC.enabled = enableCollider;
            cC.isTrigger = true; // avoid bouncing
            PointerDownEvent = PointerDownEventHandler;
            SBPointerDownEvent += (tpe)=>
            { 
                onClick?.Invoke();
            };
        }

        internal void ActivateCollider(bool activate)
        {
            GetComponent<CircleCollider2D>().enabled = activate;
        }

        public  void ApplyToTarget(GridCell hitGCell, GridCell freeGCell, CellsGroup group)
        {
           if(debug) Debug.Log("apply");
        }

        /// <summary>
        /// Get shoot area around target free grid cell
        /// </summary>
        /// <param name="hitGCell"></param>
        /// <param name="freeGCell"></param>
        /// <param name="grid"></param>
        /// <returns></returns>
        public CellsGroup GetShootArea(GridCell hitGCell, GridCell freeGCell, BubbleGrid grid)
        {
            if (debug) Debug.Log("shootbubble get shoot area");
            BoosterFunc bF = GetComponent<BoosterFunc>();
            if (bF)
            {
                return bF.GetShootArea(hitGCell, freeGCell, grid);
            }
            else if (GObject)
            {
                return grid.GetIdArea(freeGCell, GObject.ID);
            }
            return null;
        }

        public void ApplyShootBubbleToGrid(GridCell hitGridCell, GridCell freeGridCell, CellsGroup shootGridCellsArea)
        {
            if (debug) Debug.Log("apply shoot bubble");

            if (GObject) // regular object shoot bubble, set new mainobject and destroy shootbubble
            {
                if (shootGridCellsArea.Length < 2) freeGridCell.SetObject(GObject.ID);
                DestroyImmediate(gameObject);
            }
            else // possible booster
            {
                BoosterFunc bF = GetComponent<BoosterFunc>();
                if (bF)
                {
                    bF.ApplyBooster(hitGridCell, freeGridCell, shootGridCellsArea);
                }
            }

            if (hitGridCell) hitGridCell.ShootHit(null);
        }

        #region touch
        public void PointerDownEventHandler(TouchPadEventArgs tpea)
        {
            Debug.Log("pointer down: " + ToString());
            SBPointerDownEvent?.Invoke(this);
        }
        #endregion touch
    }
}
