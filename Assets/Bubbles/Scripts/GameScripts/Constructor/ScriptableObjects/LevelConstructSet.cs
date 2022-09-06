using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace Mkey
{
   // [CreateAssetMenu]
    public class LevelConstructSet : BaseScriptable
    {
        [HideInInspector]
        [SerializeField]
        private PopUpsController levelStartStoryPage;
        [HideInInspector]
        [SerializeField]
        private PopUpsController levelWinStoryPage;

        [SerializeField]
        private int vertSize = 15;
        [SerializeField]
        private int horSize = 11;
        [HideInInspector]
        [SerializeField]
        public float distX = 0.15f;
        [HideInInspector]
        [SerializeField]
        public float distY = 0.15f;
        [HideInInspector]
        [SerializeField]
        public float scale = 1.0f;
        [SerializeField]
        public int backGroundNumber = 0;
        [SerializeField]
        public List<GCellObects> cells;
        public MissionConstruct levelMission;

        #region properties
        public PopUpsController LevelWinStoryPage =>levelWinStoryPage; 

        public PopUpsController LevelStartStoryPage => levelStartStoryPage; 

        public int BackGround => backGroundNumber; 

        public int VertSize
        {
            get { return vertSize; }
            set
            {
                if (value < 1) value = 1;
                vertSize = value;
                SetAsDirty();
            }
        }

        public int HorSize
        {
            get { return horSize; }
            set
            {
                if (value < 1) value = 1;
                horSize = value;
                SetAsDirty();
            }
        }

        public float DistX
        {
            get { return distX; }
            set
            {
                distX = RoundToFloat(value, 0.05f);
                SetAsDirty();
            }
        }

        public float DistY
        {
            get { return distY; }
            set
            {
                distY = RoundToFloat(value, 0.05f);
                SetAsDirty();
            }
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                if (value < 0) value = 0;
                scale = RoundToFloat(value, 0.05f);
                SetAsDirty();
            }
        }
        #endregion properties

        #region regular
        void OnEnable()
        {
           // Debug.Log("onenable " + ToString());
            if (levelMission == null) levelMission = new MissionConstruct();
            levelMission.SaveEvent = SetAsDirty;

        }
        #endregion regular

        public void Clean(GameObjectsSet gOS)
        {
            if (cells == null) cells = new List<GCellObects>();
            cells.RemoveAll((c) => { return (!BubbleGrid.ok(c.row, c.column, vertSize, horSize)); });
            if (gOS)
            {
                foreach (var item in cells)
                {
                    if (item.gridObjects != null)
                    {
                        item.gridObjects.RemoveAll((o) => { return !gOS.ContainID(o.id); });
                    }
                }
            }

            SetAsDirty();
        }

        #region background
        public void IncBackGround(int length)
        {
            backGroundNumber++;
            backGroundNumber = (int)Mathf.Repeat(backGroundNumber, length);
            Save();
        }

        public void DecBackGround(int length)
        {
            backGroundNumber--;
            backGroundNumber = (int)Mathf.Repeat(backGroundNumber, length);
            Save();
        }
        #endregion background

        #region utils
        private float RoundToFloat(float val, float delta)
        {
            int vi = Mathf.RoundToInt(val / delta);
            return (float)vi * delta;
        }

        private void RemoveCellData(List<CellData> cdl, CellData cd)
        {
            if (cdl != null) cdl.RemoveAll((c) => { return ((cd.Column == c.Column) && (cd.Row == c.Row)); });
        }

        private bool ContainCellData(List<CellData> lcd, CellData cd)
        {
            if (lcd == null || cd == null) return false;
            foreach (var item in lcd)
            {
                if ((item.Row == cd.Row) && (item.Column == cd.Column)) return true;
            }
            return false;
        }
        #endregion utils

        internal void SaveObjects(GridCell gC)
        {
            cells.RemoveAll((c) => { return ((c.row == gC.Row) && (c.column == gC.Column)); });
            List<GridObjectState> gOSs = gC.GetGridObjectsStates();
            if (gOSs.Count > 0) cells.Add(new GCellObects(gC.Row, gC.Column, gOSs));

            SetAsDirty();
        }

        internal void Scan(BubbleGrid mGrid)
        {
            foreach (var item in mGrid.Cells)
            {
                SaveObjects(item);
            }
        }

        internal List<MainObject> GetMatchObjects(GameObjectsSet goSet)
        {
            return new List<MainObject>(goSet.MainObjects);
        }
    }

    [Serializable]
    public class GCellObects
    {
        public int row;
        public int column;
        public List<GridObjectState> gridObjects;

        public GCellObects(int row, int column, List<GridObjectState> gridObjects)
        {
            this.row = row;
            this.column = column;
            this.gridObjects = new List<GridObjectState>(gridObjects);
        }
    }
}
