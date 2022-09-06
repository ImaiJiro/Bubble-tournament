using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Mkey
{
    public class GridCell : TouchPadMessageTarget
    {
        #region debug
        private bool debug = false;
        #endregion debug

        #region row column
        public Row<GridCell> GRow { get; private set; }
        public int Row { get; private set; }
        public int Column { get; private set; }
        public List<Row<GridCell>> Rows { get; private set; }

        public PFCell pfCell;
        public bool IsEvenRow { get; private set; }
        #endregion row column

        #region objects
        [SerializeField]
        public MainObject Mainobject => GetComponentInChildren<MainObject>();
        public OverlayObject Overlay => GetComponentInChildren<OverlayObject>();
        #endregion objects

        #region cache fields
        private CircleCollider2D coll2D;
        private SpriteRenderer sRenderer;
        private GameObject targetSelector;
        #endregion cache fields

        #region events
        public Action<GridCell> GCPointerDownEvent;
        public Action<GridCell> GCDoubleClickEvent;
        public Action<GridCell> GCDragEnterEvent;
        #endregion events

        #region properties 
        /// <summary>
        /// Return true if mainobject and mainobject IsMatchedById || IsMatchedWithAny
        /// </summary>
        /// <returns></returns>
        public bool IsMatchable
        {
            get { return (Mainobject && Mainobject.IsMatchedById); }
        }

        /// <summary>
        /// Return true if mainobject == null
        /// </summary>
        public bool IsEmpty
        {
            get { return !Mainobject; }
        }

        /// <summary>
        /// Return true if gridcell row==0
        /// </summary>
        public bool IsTopGridcell
        {
            get { return Row == 0; }
        }

        /// <summary>
        /// Return true if gridcell row is top used for gameobects (not service)
        /// </summary>
        public bool IsTopObjectGridcell
        {
            get { return GRow.isTopObjectRow; }
        }

        /// <summary>
        /// Return true if MO IsExploidable  and !FullProtected
        /// </summary>
        /// <returns></returns>
        public bool IsExploidable
        {
            get
            {
                if (Mainobject && Mainobject.IsExploidable) return true;
                return false;
            }
        }

        public NeighBors Neighbors;//{ get; private set; }
        #endregion properties 

        #region temp vars
        private GameBoard MBoard => GameBoard.Instance; 
        private BubblesPlayer MPlayer => BubblesPlayer.Instance;
        private BubblesGuiController MGui => BubblesGuiController.Instance;
        private SoundMaster MSound => SoundMaster.Instance;
        private GameConstructSet GcSet =>  MPlayer.gcSet;
        private GameObjectsSet GoSet => GcSet.GOSet;
        private LevelConstructSet LcSet => MPlayer.LcSet;
        #endregion temp vars

        #region touchbehavior only for construct
        public void PointerDownEventHandler(TouchPadEventArgs tpea)
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                GCPointerDownEvent?.Invoke(this);
            }
        }

        public void DragEventHandler(TouchPadEventArgs tpea)
        {

        }

        public void DragBeginEventHandler(TouchPadEventArgs tpea)
        {

        }

        public void DragDropEventHandler(TouchPadEventArgs tpea)
        {

        }

        public void DragEnterEventHandler(TouchPadEventArgs tpea)
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                Debug.Log("drag enter " + ToString());
                GCDragEnterEvent?.Invoke(this);
            }
        }

        public void DragExitEventHandler(TouchPadEventArgs tpea)
        {

        }

        public void PointerUpEventHandler(TouchPadEventArgs tpea)
        {

        }
        #endregion touchbehavior only for construct

        #region set get objects
        internal void SetObject(int ID)
        {
            GridObject prefab = GoSet.GetObject(ID);
            if (prefab)
            {
                prefab.Hits = 0;
                SetObject(prefab);
            }
        }

        internal void SetObject(int ID, int hits)
        {
            GridObject prefab = GoSet.GetObject(ID);
            if (prefab)
            {
                prefab.Hits = hits;
                SetObject(prefab);
            }
        }

        internal GridObject SetObject(GridObject prefab)
        {
            if (prefab == null) return null;
            GridObject gO = prefab.Create(this, null); 
            return gO;
        }

        public GridObject[] GetGridObjects()
        {
            return GetComponentsInChildren<GridObject>();
        }

        public List<GridObjectState> GetGridObjectsStates()
        {
            GridObject[] gOs = GetGridObjects();
            List<GridObjectState> res = new List<GridObjectState>();
            foreach (var item in gOs)
            {
                res.Add(new GridObjectState(item.ID, item.Hits));
            }
            return res;
        }

        public List<int> GetGridObjectsIDs()
        {
            List<int> res = new List<int>();
            foreach (var item in GetGridObjects())
            {
                res.Add(item.ID);
            }
            return res;
        }

        public void RemoveObject(int id)
        {
            sRenderer.enabled = true;
            GridObject[] gOs = GetComponentsInChildren<GridObject>();
            foreach (var gO in gOs)
            {
                if (gO && gO.ID == id) { gO.Remove(); }
            }
        }

        public T GetObject<T>() where T : GridObject
        {
            return GetComponentInChildren<T>();
        }
        #endregion set get objects

        #region grid objects behavior
        /// <summary>
        /// Side hit from shoot bubble, it worked with destroayble mainobject
        /// </summary>
        internal void ShootHit(Action completeCallBack)
        {
            if (!Mainobject)
            {
                completeCallBack?.Invoke();
                return;
            }
            Debug.Log("shoot hit: " + Mainobject);
            Mainobject.ShootHit(completeCallBack);
            if (Mainobject && Mainobject.Protection <= 0) Mainobject.transform.parent = null;

            if (Overlay)
            {
                Overlay.ShootHit(null);
            }
        }

        /// <summary>
        /// Set grid cell main object to null, run CollectDelegate (if set) and completeCallBack
        /// </summary>
        /// <param name="completeCallBack"></param>
        internal void CollectShootAreaObject(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            if (!Mainobject)
            {
                completeCallBack?.Invoke();
                return;
            }
            Mainobject.ShootAreaCollect(completeCallBack, showPrivateScore, addPrivateScore, decProtection, privateScore);
            Mainobject.transform.parent = null;
            if (IsTopObjectGridcell) coll2D.enabled = true;
            if (Overlay)
            {
                Overlay.ShootAreaCollect(null, showPrivateScore, addPrivateScore, decProtection, privateScore);
                Overlay.transform.parent = null;
            }
        }

        internal void CollectFalledDown(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, int privateScore)
        {
            if (!Mainobject)
            {
                completeCallBack?.Invoke();
                return;
            }
            Mainobject.gameObject.layer = 2; // ignore shoot raycasting
            Mainobject.FallDownCollect(completeCallBack, showPrivateScore, addPrivateScore, privateScore);
            Mainobject.transform.parent = null;
            if (IsTopObjectGridcell) coll2D.enabled = true;
            if (Overlay)
            {
                Overlay.gameObject.layer = 2; // ignore shoot raycasting
                Overlay.ShootAreaCollect(null, showPrivateScore, addPrivateScore, true, privateScore);
                Overlay.transform.parent = null;
            }
        }
        #endregion grid objects behavior

        /// <summary>
        ///  used by instancing for cache data
        /// </summary>
        internal void Init(int cellRow, int cellColumn, List<Row<GridCell>> rows, GameMode gMode)
        {
            Row = cellRow;
            Column = cellColumn;
            GRow = rows[cellRow];
            IsEvenRow = (cellRow % 2 == 0);
            Rows = rows;

#if UNITY_EDITOR
            name = ToString();
#endif
            sRenderer = GetComponent<SpriteRenderer>();
            if(sRenderer) sRenderer.sortingOrder = SortingOrder.Base;
            coll2D = GetComponent<CircleCollider2D>();
            if (gMode == GameMode.Play && !IsTopObjectGridcell) DestroyImmediate(GetComponent<CircleCollider2D>());
            if (gMode == GameMode.Play) DestroyImmediate(GetComponent<SpriteRenderer>()); // 

            PointerDownEvent    = PointerDownEventHandler;
            DragBeginEvent      = DragBeginEventHandler;
            DragEnterEvent      = DragEnterEventHandler;
            DragExitEvent       = DragExitEventHandler;
            DragDropEvent       = DragDropEventHandler;
            PointerUpEvent      = PointerUpEventHandler;
            DragEvent           = DragEventHandler;
        }

        /// <summary>
        ///  return true if main MainObjects of two cells are equal
        /// </summary>
        internal bool IsMainObjectEquals(GridCell other)
        {
            if (other == null) return false;
            if (other.Mainobject == null) return false;
            if (Mainobject == null) return false;

            return Mainobject.Equals(other.Mainobject);//Check whether the MainObject properties are equal. 
        }

        /// <summary>
        ///  cancel any tween on main MainObject object
        /// </summary>
        internal void CancelTween()
        {
            GridObject[] gridObjects = GetGridObjects();
            foreach (var item in gridObjects)
            {
                item.CancellTweensAndSequences();
            }
        }

        /// <summary>
        /// DestroyImeediate MainObject, OverlayProtector, UnderlayProtector
        /// </summary>
        internal void DestroyGridObjects()
        {
            GridObject[] gridObjects = GetGridObjects();
            foreach (var item in gridObjects)
            {
                DestroyImmediate(item.gameObject);
            }
        }
    
        internal void ShowTargetSelector(bool show, Sprite sprite)
        {
            if (targetSelector && show) // need and also exist
            {
                return;
            }
            else if(!targetSelector && show && sprite) // need but not exist - create new 
            {
                targetSelector = Creator.CreateSpriteAtPosition(transform, sprite, transform.position, SortingOrder.TargetSelector).gameObject;
            }
            else if (targetSelector && !show) // not need but exist
            {
                DestroyImmediate(targetSelector);
            }
        }

        internal void ShowTargetSelector(bool show)
        {
            ShowTargetSelector(show, GoSet.selector);
        }

        public bool HaveObjectWithID(int id)
        {
            GridObject[] gOs = GetGridObjects();
            foreach (var gO in gOs)
            {
                if (gO && gO.ID == id) return true;
            }
            return false;
        }

        #region override 
        public override string ToString()
        {
            return "cell : [ row: " + Row + " , col: " + Column + "]";
        }
        #endregion override

    }

  
}