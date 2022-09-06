using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class BubbleGrid
    {
        #region properties
        public List<GridCell> Cells { get; private set; }
        public List<Row<GridCell>> Rows { get; private set; }
        /// <summary>
        ///  //circle collider radius
        /// </summary>
        public float BubleRadius { get; private set; }
        public bool ErrFlag { get; private set; }
        public static BubbleGrid Instance { get; private set; }
        public Row<GridCell> TopObjectRow { get { if (Rows == null || Rows.Count <= serviceRowsCount + 1) return null;  return Rows[serviceRowsCount]; } }
        public Transform CellsParent { get; private set; }
        public LevelConstructSet LcSet { get; private set; }
        #endregion properties

        #region temp vars
        private GameObjectsSet goSet;
        private GameObject prefab;
        private int vertSize; // rows count
        private int horSize; // max row length (min row.length = max_row.length - 1)
        private int visibleRowsCount = 11;
        private float yStart; // Camera.main.orthographicSize - radius
        private float yStep;
        private float xStep;
        private float cOffset;
        private int yOffset;
        private GameMode gMode;
        private int sortingOrder;
        private const int serviceRowsCount = 2;
        #endregion temp vars

        public BubbleGrid(LevelConstructSet lcSet, GameObjectsSet goSet, Transform cellsParent, int sortingOrder, GameMode gMode)
        {
            Debug.Log("new grid ");
            LcSet = lcSet;
            CellsParent = cellsParent;
            this.gMode = gMode;
            this.sortingOrder = sortingOrder;

            vertSize = lcSet.VertSize;
            horSize = lcSet.HorSize;
            this.goSet = goSet;
            prefab = this.goSet.gridCellPrefab;

            Cells = new List<GridCell>(vertSize * horSize);
            Rows = new List<Row<GridCell>>(vertSize);

            BubleRadius = prefab.GetComponent<CircleCollider2D>().radius;
            yStart = Camera.main.orthographicSize - BubleRadius;

            xStep = BubleRadius * 2.0f; //
            yStep = xStep * Mathf.Sin(60.0f * Mathf.Deg2Rad);
            cOffset = (1 - horSize) * BubleRadius; // offset from center

            for (int i = 0; i > -vertSize; i--) 
            {
                AddRow();
            }

            for (int i = 0; i < Rows.Count; i++)
            {
                for (int j = 0; j < Rows[i].Length; j++)
                {
                    Rows[i][j].Init(i, j, Rows, gMode);
                }
            }

            SetObjectsData();
            Debug.Log("create cells: " + Cells.Count);
            Instance = this;
        }

        public void Rebuild(GameObjectsSet goSet, int sortingOrder, GameMode gMode)
        {
            Debug.Log("rebuild grid");

            vertSize = LcSet.VertSize;
            horSize = LcSet.HorSize;
            this.goSet = goSet;
            Cells.ForEach((c) => { c.DestroyGridObjects(); });

            List<GridCell> tempCells = Cells;
            Cells = new List<GridCell>(vertSize * horSize +  horSize);
            Rows = new List<Row<GridCell>>(vertSize);
            float cOffset = (1 - horSize) * BubleRadius; // offfset from center

            // create rows 
            GridCell cell;
            Row<GridCell> row;
            int cellCounter = 0;
            bool isEven = false;
            int ri = 0;

            for (int i = 0; i > - vertSize; i--)
            {
                isEven = (ri % 2 == 0);
                row = new Row<GridCell>((isEven) ? horSize : horSize - 1);
                float eo = (isEven) ? 0 : BubleRadius;
                for (int j = 0; j < row.Length; j++)
                {
                    Vector3 pos = new Vector3(j * xStep + eo + cOffset, yStart + i * yStep, 0);
                    if (tempCells != null && cellCounter < tempCells.Count)
                    {
                        cell = tempCells[cellCounter];
                        cell.transform.localPosition = pos;
                        cellCounter++;
                    }
                    else
                    {
                        cell = UnityEngine.Object.Instantiate(this.goSet.gridCellPrefab).GetComponent<GridCell>();
                        cell.transform.parent = CellsParent;
                        cell.transform.localPosition = pos;
                        cell.transform.localScale = Vector3.one;
                    }
                    Cells.Add(cell);
                    row[j] = cell;
                    cell.GetComponent<SpriteRenderer>().enabled = (i > -vertSize);
                }
                Rows.Add(row);
                ri++;
            }

            // destroy not used cells
            if (cellCounter < tempCells.Count)
            {
                for (int i = cellCounter; i < tempCells.Count; i++)
                {
                    UnityEngine.Object.Destroy(tempCells[i].gameObject);
                }
            }

            for (int i = 0; i < Rows.Count; i++)
            {
                bool isService = (i < serviceRowsCount);
                bool isTopBjectRow = (i == serviceRowsCount);
                Rows[i].IsSeviceRow = isService;
                Rows[i].isTopObjectRow = isTopBjectRow;

                for (int j = 0; j < Rows[i].Length; j++)
                {
                    Rows[i][j].Init(i, j, Rows, gMode);
                }
            }

            SetObjectsData();
            Debug.Log("rebuild cells: " + Cells.Count);
        }

        public GridCell this[int row, int column]
        {
            get { if (ok(row, column)) { ErrFlag = false; return Rows[row][column]; } else { ErrFlag = true; return null; } }
            set { if (ok(row, column)) { ErrFlag = false; Rows[row][column] = value; } else { ErrFlag = true; } }
        }

        public bool ok(int row, int column)
        {
            int hSize = (row % 2 == 0) ? horSize : horSize - 1;
            return (row >= 0 && row < vertSize && column >= 0 && column < hSize);
        }

        public static bool ok(int row, int column, int vertGridSize, int horGridSize)
        {
            int hSize = (row % 2 == 0) ? horGridSize : horGridSize - 1;
            return (row >= 0 && row < vertGridSize && column >= 0 && column < hSize);
        }

        private void SetObjectsData()
        {
            if (LcSet.cells != null)
                foreach (var c in LcSet.cells)
                {
                    if (c != null && c.gridObjects != null)
                    {
                        foreach (var o in c.gridObjects)
                        {
                            if (ok(c.row, c.column)) Rows[c.row][c.column].SetObject(o.id, o.hits);
                        }
                    }
                }
        }

        /// <summary>
        /// Add row to grid
        /// </summary>
        private void AddRow()
        {
            GridCell cell;
            int i = Rows.Count;
            bool isService = (i < serviceRowsCount);
            bool isTopBjectRow = (i == serviceRowsCount);

            bool isEven = (i % 2 == 0);
            Row<GridCell> row = new Row<GridCell>((isEven) ? horSize : horSize - 1);
            float eOffset = (isEven) ? 0 : BubleRadius; // even offset
            for (int j = 0; j < row.Length; j++)
            {
                Vector3 pos = new Vector3(j * xStep + eOffset + cOffset, yStart + yOffset * yStep, 0);
                cell = UnityEngine.Object.Instantiate(goSet.gridCellPrefab, CellsParent).GetComponent<GridCell>();
                cell.transform.localPosition = pos;
                cell.transform.localScale = Vector3.one;
                Cells.Add(cell);
                row[j] = cell;
                cell.GetComponent<SpriteRenderer>().enabled = (i < vertSize);
            }
            //set flags
            row.IsSeviceRow = isService;
            row.isTopObjectRow = isTopBjectRow;

            Rows.Add(row);
            for (int j = 0; j < Rows[i].Length; j++)
            {
                Rows[i][j].Init(i, j, Rows, gMode);
            }

            yOffset--;
        }

        /// <summary>
        /// if (emptyRows count==0) AddRow;
        /// </summary>
        public void AddEmptyRow()
        {
            if (GetEmptyRowsCount() < 1) // need for path finder
            {
                AddRow();
            }

            if (GetEmptyRowsCount() < 2) // need for path finder
            {
                AddRow();
            }
        }

        #region  get data from grid
        /// <summary>
        /// Return first empty row number at top or -1 if not have empty (exclude sevice rows)
        /// </summary>
        /// <returns></returns>
        private int GetFirstEmptyRowNumber()
        {
            for (int i = serviceRowsCount; i < Rows.Count; i++)
            {
                if (Rows[i].RowIsEmpty()) return i;
            }
            return -1;
        }

        /// <summary>
        /// Return empty rows count from bottom
        /// </summary>
        /// <returns></returns>
        public int GetEmptyRowsCount()
        {
            int result = 0;
            for (int i = Rows.Count - 1; i >= serviceRowsCount; i--)
            {
                if (Rows[i].RowIsEmpty()) result++;
                else break;
            }
            return result;
        }

        /// <summary>
        /// Return gridcells group with id matched bubbles around free gridcell
        /// </summary>
        /// <param name="freeGCell"></param>
        /// <returns></returns>
        public MatchGroup GetIdArea(GridCell freeGCell, int id)
        {
            MatchGroup res = new MatchGroup();
            MatchGroup equalNeigh = new MatchGroup();
            MatchGroup neighTemp;

            if (freeGCell)
            {
                NeighBors nCells = new NeighBors(freeGCell, id);   // res.Add(freeGCell);
                equalNeigh.AddRange(nCells.EqualIDCells); //equalNeigh.AddRange(gCell.EqualNeighBornCells());
                while (equalNeigh.Length > 0)
                {
                    res.AddRange(equalNeigh.cells);
                    neighTemp = new MatchGroup();
                    foreach (var item in equalNeigh.cells)
                    {
                        nCells = new NeighBors(item, id);
                        neighTemp.AddRange(nCells.EqualIDCells); // neighTemp.AddRange(item.EqualNeighBornCells());
                    }
                    equalNeigh = neighTemp;
                    equalNeigh.Remove(res.cells);
                }
            }
            res.Remove(freeGCell);
            return res;
        }

        /// <summary>
        /// Return gridcells group with ids matched bubbles around free gridcell
        /// </summary>
        /// <param name="freeGCell"></param>
        /// <returns></returns>
        public MatchGroup GetIdArea(GridCell freeGCell, List<int> ids)
        {
            MatchGroup res = new MatchGroup();

            for (int i = 0; i < ids.Count; i++)
            {
                res.Merge(GetIdArea(freeGCell, ids[i]));
            }
            res.Remove(freeGCell);
            return res;
        }

        /// <summary>
        /// Return gridcells group with all existing mainobject ids matched bubbles around free gridcell
        /// </summary>
        /// <param name="freeGCell"></param>
        /// <returns></returns>
        public MatchGroup GetIdArea(GridCell freeGCell)
        {
            List<int> ids = new List<int>(goSet.MainObjects.Count);
            foreach (var item in goSet.MainObjects)
            {
                ids.Add(item.ID);
            }
            return GetIdArea(freeGCell, ids);
        }

        /// <summary>
        /// Return not empty gridcells from column
        /// </summary>
        /// <param name="gridCell"></param>
        /// <returns></returns>
        public MatchGroup GetColumnArea(GridCell gridCell)
        {
            MatchGroup res = new MatchGroup();
            int row = gridCell.Row;
            int column = gridCell.Column;
            for (int i = row; i >= 0; i -= 2)
            {
                GridCell gCell = this[i, column];

                if (!gCell.IsEmpty) res.Add(gCell);
            }
            return res;
        }

        /// <summary>
        /// Return all closed not intersected areas
        /// </summary>
        /// <returns></returns>
        public List<GridCell> GetDetacheCells()
        {
            CellsGroup main = new CellsGroup(); // main group from 0 row
            CellsGroup neigh = new CellsGroup();
            CellsGroup neighTemp;
            main.AddRange(Rows[serviceRowsCount].GetNotEmptyCells()); // start at service rows
            NeighBors nCells;
            for (int i = 0; i < main.Length; i++)
            {
                nCells = new NeighBors(main.cells[i]);
                neigh.AddRange(nCells.NotEmptyCells);// neigh.AddRange(main.cells[i].NotEmptyNeighBornCells());
            }

            while (neigh.Length > 0) // find and add to group not empty neighborns
            {
                main.AddRange(neigh.cells);
                neighTemp = new CellsGroup();
                foreach (var item in neigh.cells)
                {
                    nCells = new NeighBors(item);
                    neighTemp.AddRange(nCells.NotEmptyCells); 
                }
                neigh = neighTemp;
                neigh.Remove(main.cells);
            }

            CellsGroup all = new CellsGroup();
            all.AddRange(GetNotEmptyCells());

            all.Remove(main.cells);
           // Debug.Log("detouched: " + all.ToString());
            return all.cells;
        }

        /// <summary>
        /// Return all not empty cells
        /// </summary>
        public List<GridCell> GetNotEmptyCells()
        {
            List<GridCell> res = new List<GridCell>();

            for (int i = 0; i < Rows.Count; i++)
            {
                res.AddRange(Rows[i].GetNotEmptyCells());
            }
            return res;
        }

        public List<GridCell> GetEqualCells(GridCell gCell)
        {
            List<GridCell> gCells = new List<GridCell>();
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].IsMainObjectEquals(gCell))
                {
                    gCells.Add(Cells[i]);
                }
            }
            return gCells;
        }

        /// <summary>
        /// Return objects count on grid with selected ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetObjectsCountByID(int id)
        {
            int res = 0;
            GridObject[] bds = CellsParent.GetComponentsInChildren<GridObject>();
            foreach (var item in bds)
            {
                if (item.ID == id) res++;
            }
            return res;
        }

        /// <summary>
        /// Return all  main objects data from grid
        /// </summary>
        public List<MainObject> GetMainObjectsData()
        {
            List<MainObject> res = new List<MainObject>();

            for (int i = 0; i < Cells.Count; i++)
            {
                if(Cells[i].Mainobject)
                res.Add(Cells[i].Mainobject);
            }
            return res;
        }

        /// <summary>
        /// Return dict of main objects id with count
        /// </summary>
        public Dictionary<int, int> GetMainObjectsDataDict()
        {
            Dictionary<int, int> res = new Dictionary<int, int>();
            for (int i = 0; i < Cells.Count; i++)
            {
                if (Cells[i].Mainobject)
                {
                    int id = Cells[i].Mainobject.ID;
                    if (res.ContainsKey(id))
                    {
                        res[id]++;
                    }
                    else
                    {
                        res.Add(id, 1);
                    }
                }
            }
            return res;
        }

        public void CalcObjects()
        {
            GridObject[] bds = CellsParent.GetComponentsInChildren<GridObject>();
            Debug.Log("Objects count: " + bds.Length);
        }

        public Row<GridCell> GetRow(int row)
        {
            return (row >= 0 && row < Rows.Count) ? Rows[row] : null;
        }

        public int GetObjectCountWithScore()
        {
            int count = 0;
            Cells.ForEach((c) =>
            {
                if (!c.IsEmpty)
                {
                    GridObject [] gOs =  c.GetGridObjects();
                    foreach (var gO in gOs)
                    {
                        count += (gO.WithScore ? 1 : 0);
                    }
                }
            });
            return count;
        }

        public Row<GridCell> GetBottomRow()
        {
            return Rows[Rows.Count - 1];
        }

        #endregion get data from grid

        #region move 
        public void MoveToVisible(Action completeCallBack)
        {
            int fER = GetFirstEmptyRowNumber();
            if (fER < 0)
            {
                fER = Rows.Count-1;
            }
            int topVisRow = fER - visibleRowsCount;
            topVisRow = (topVisRow < 0) ? 0 : topVisRow;
            float topVisRowCurrPos = Rows[topVisRow][0].transform.position.y;
            float topVisRowTargPos = yStart;
            float dY = topVisRowTargPos - topVisRowCurrPos; //Debug.Log("first empty row from top: " + fER);
            Vector3 stPos = CellsParent.position;
            Vector3 endPos = new Vector3(stPos.x, stPos.y + dY, stPos.z); //Debug.Log("endpos: " + stPos.y+ dY);
            SimpleTween.Move(CellsParent.gameObject, stPos, endPos, 0.7f).AddCompleteCallBack(completeCallBack);
        }

        public void MoveStep(bool up, float time, Action completeCallBack)
        {
            SimpleTween.Cancel(CellsParent.gameObject, true, SimpleTween.CancelCondition.SetEndValue);
            if (((Rows[0][0].transform.position.y > yStart) && !up) || ((Rows[Rows.Count - 1][0].transform.position.y < yStart) && up))
            {
                float step = (up) ? yStep : -yStep;
                Vector3 stPos = CellsParent.position;
                Vector3 endPos = new Vector3(stPos.x, stPos.y + step, stPos.z);

                SimpleTween.Move(CellsParent.gameObject, stPos, endPos, time).AddCompleteCallBack(() =>
                {
                    Debug.Log("move " + (up ? "up" : "down"));
                    if (completeCallBack != null) completeCallBack();
                });
            }
        }
        #endregion move

        public void DestroyGrid()
        {
            if (Cells != null && Cells.Count > 0)
            {
                for (int i = 0; i < Cells.Count; i++)
                {
                    UnityEngine.Object.DestroyImmediate(Cells[i].gameObject);
                }
                Cells = null;
            }
        }
    }
}
