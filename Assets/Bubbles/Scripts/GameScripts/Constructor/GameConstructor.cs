using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace Mkey
{
    public class GameConstructor : MonoBehaviour
    {
#if UNITY_EDITOR

        private List<RectTransform> openedPanels;

        [SerializeField]
        private Text editModeText;

        #region selected brush
        [Space(8, order = 0)]
        [Header("Grid Brushes", order = 1)]

        [SerializeField]
        private GridObject currentBrush;
        [SerializeField]
        private IncDecInputPanel IncDecPanelPrefab;
        [SerializeField]
        private PanelContainerController brushPanelContainerPrerfab;
        [SerializeField]
        private Transform brushContainersParent;
        [SerializeField]
        private PanelContainerController ShootBubbleContainer;
        #endregion selected brush

        #region mission
        [Space(8, order = 0)]
        [Header("Mission", order = 1)]
        [SerializeField]
        private PanelContainerController MissionPanelContainer;
        [SerializeField]
        private IncDecInputPanel InputTextPanelMissionPrefab;
        [SerializeField]
        private IncDecInputPanel IncDecTogglePanelMissionPrefab;
        [SerializeField]
        private IncDecInputPanel TogglePanelMissionPrefab;
        #endregion mission

        #region grid construct
        [Space(8, order = 0)]
        [Header("Grid", order = 1)]
        [SerializeField]
        private PanelContainerController GridPanelContainer;
        [SerializeField]
        private IncDecInputPanel IncDecGridPrefab;
        #endregion grid construct

        #region game construct
        [Space(8, order = 0)]
        [Header("Game construct", order = 0)]
        [SerializeField]
        private Button levelButtonPrefab;
        [SerializeField]
        private Button smallButtonPrefab;
        [SerializeField]
        private GameObject constructPanel;
        [SerializeField]
        private Button openConstructButton;
        [SerializeField]
        private ScrollRect LevelButtonsContainer;
        #endregion game construct

        #region temp vars
        private GameBoard MBoard { get { return GameBoard.Instance; } }
        private BubblesPlayer MPlayer { get { return BubblesPlayer.Instance; } }
        private GameConstructSet GCSet{get { return MPlayer.gcSet; } }
        private GameObjectsSet GOSet { get { return GCSet.GOSet; } }
        private LevelConstructSet LCSet { get { return MPlayer.LcSet; } }
        private BubbleGrid MGrid { get { return MBoard.grid; } }
        #endregion temp vars

        #region default data
        private string levelConstructSetSubFolder = "LevelConstructSets";
        private string pathToSets = "Assets/Bubbles/Resources/";
        private int minVertSize = 5;
        private int minHorSize = 5;
        private int maxHorSize = 15;
        #endregion default data

        public void InitStart()
        {
            if (GameBoard.gMode == GameMode.Edit)
            {
                if (!MBoard) return;
                if (!MPlayer) return;

                Debug.Log("Game Contructor init start");

                if (!GCSet)
                {
                    Debug.Log("Game construct set not found!!!");
                    return;
                }
                if (!GOSet)
                {
                    Debug.Log("GameObjectSet not found!!! - ");
                    return;
                }

                currentBrush  = null;

                // create brush panels
                CreateBrushContainer(brushContainersParent, brushPanelContainerPrerfab, "Main brush panel", new List<GridObject>(GOSet.MainObjects));
                CreateBrushContainer(brushContainersParent, brushPanelContainerPrerfab, "Overlay brush panel", new List<GridObject>(GOSet.OverlayObjects));

                if (editModeText) editModeText.text = "EDIT MODE" + '\n' + "Level " + (BubblesPlayer.CurrentLevel + 1);
                ShowLevelData(false);

                DeselectAllBrushes();
                CreateLevelButtons();
                ShowConstructMenu(true);
            }
        }

        #region show board
        private void ShowLevelData()
        {
            ShowLevelData(true);
        }

        private void ShowLevelData(bool rebuild)
        {
            GCSet.Clean();
            LCSet.Clean(GOSet);

            Debug.Log("Show level data: " + BubblesPlayer.CurrentLevel);
            if (rebuild) MBoard.CreateGameBoard(false);

            LevelButtonsRefresh();
            if (editModeText) editModeText.text = "EDIT MODE" + '\n' + "Level " + (BubblesPlayer.CurrentLevel+1);
        }
        #endregion show board

        #region board move
        public void MoveBoard(int steps)
        {
            bool up = steps > 0;
            int aSteps = Mathf.Abs(steps);
            TweenSeq tS = new TweenSeq();
            for (int i = 0; i < aSteps; i++)
            {
                tS.Add((callBack) => 
                {
                    GameBoard.Instance.grid.MoveStep(up,0.1f, callBack);
                });
            }
            tS.Start();
        }
        #endregion board move

        #region construct menus
        bool openedConstr = false;

        public void OpenConstructPanel()
        {
            SetConstructControlActivity(false);
            constructPanel.SetActive(true);

            RectTransform rt = constructPanel.GetComponent<RectTransform>();//Debug.Log(rt.offsetMin + " : " + rt.offsetMax);
            float startX = (!openedConstr) ? 0 : 1f;
            float endX = (!openedConstr) ? 1f : 0;

            SimpleTween.Value(constructPanel, startX, endX, 0.2f).SetEase(EaseAnim.EaseInCubic).
                               SetOnUpdate((float val) =>
                               {
                                   rt.transform.localScale = new Vector3(val, 1, 1);
                               // rt.offsetMax = new Vector2(val, rt.offsetMax.y);
                           }).AddCompleteCallBack(() =>
                           {
                               SetConstructControlActivity(true);
                               openedConstr = !openedConstr;
                               LevelButtonsRefresh();
                           });
        }

        private void SetConstructControlActivity(bool activity)
        {
            Button[] buttons = constructPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        private void ShowConstructMenu(bool show)
        {
            constructPanel.SetActive(show);
            openConstructButton.gameObject.SetActive(show);
        }

        public void CreateLevelButtons()
        {
            Debug.Log("create level buttons");
            GCSet.Clean();

            Transform parent = LevelButtonsContainer.content.transform;
            DestroyGOInChildrenWithComponent<Button>(parent);

            for (int i = 0; i < GCSet.LevelCount; i++)
            {
                int level = i + 1;

                Button button = CreateButton(levelButtonPrefab, parent, null, "" + level.ToString(), () =>
                {
                    BubblesPlayer.CurrentLevel = level - 1;
                    CloseOpenedPanels();
                    ShowLevelData();
                    ShowLevelData();
                    MBoard.grid.MoveToVisible(null);
                });
            }
        }

        public void RemoveLevel()
        {
            Debug.Log("Click on Button <Remove level...> ");
            if (GCSet.LevelCount < 2)
            {
                Debug.Log("Can't remove the last level> ");
                return;
            }
            Debug.Log("GCSet.LevelCount" + GCSet.LevelCount);
            GCSet.RemoveLevel(BubblesPlayer.CurrentLevel);
            CreateLevelButtons();
            BubblesPlayer.CurrentLevel = (BubblesPlayer.CurrentLevel <= GCSet.LevelCount-1) ? BubblesPlayer.CurrentLevel : BubblesPlayer.CurrentLevel - 1;
            ShowLevelData();
            MBoard.grid.MoveToVisible(null);
        }

        public void InsertBefore()
        {
            Debug.Log("Click on Button <Insert level before...> ");
            LevelConstructSet lcs = ScriptableObjectUtility.CreateResourceAsset<LevelConstructSet>(pathToSets, levelConstructSetSubFolder, "", " " + 1.ToString());
            GCSet.InsertBeforeLevel(BubblesPlayer.CurrentLevel, lcs);
            CreateLevelButtons();
            ShowLevelData();
            MBoard.grid.MoveToVisible(null);
        }

        public void InsertAfter()
        {
            Debug.Log("Click on Button <Insert level after...> ");
            LevelConstructSet lcs = ScriptableObjectUtility.CreateResourceAsset<LevelConstructSet>(pathToSets, levelConstructSetSubFolder, "", " " + 1.ToString());
            GCSet.InsertAfterLevel(BubblesPlayer.CurrentLevel, lcs);
            CreateLevelButtons();
            BubblesPlayer.CurrentLevel += 1;
            ShowLevelData();
            MBoard.grid.MoveToVisible(null);
        }

        private void LevelButtonsRefresh()
        {
            Button[] levelButtons = LevelButtonsContainer.content.gameObject.GetComponentsInChildren<Button>();
            for (int i = 0; i < levelButtons.Length; i++)
            {
                SelectButton(levelButtons[i], (i == BubblesPlayer.CurrentLevel));
            }
        }

        private void SelectButton(Button b, bool select)
        {
          if(b) b.GetComponent<Image>().color = (select) ? new Color(0.5f, 0.5f, 0.5f, 1) : new Color(1, 1, 1, 1);
        }
        #endregion construct menus

        #region grid settings
        private void ShowLevelSettingsMenu(bool show)
        {
            constructPanel.SetActive(show);
            openConstructButton.gameObject.SetActive(show);
        }

        public void OpenGridSettingsPanel_Click()
        {
            Debug.Log("open grid settings click");

            ScrollPanelController sRC = GridPanelContainer.ScrollPanel;
            if (sRC) // 
            {
                if (sRC) sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();

                //instantiate ScrollRectController
                sRC = GridPanelContainer.InstantiateScrollPanel();
                sRC.textCaption.text = "Grid panel";

                //create  vert size block
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "VertSize", MGrid.Rows.Count.ToString(),
                    () => { IncVertSize(); },
                    () => { DecVertSize(); },
                    (val) => {  },
                    () => { return MGrid.Rows.Count.ToString(); },
                    null);

                //create hor size block
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "HorSize", LCSet.HorSize.ToString(),
                    () => { IncHorSize(); },
                    () => { DecHorSize(); },
                    (val) => { },
                    () => { return LCSet.HorSize.ToString(); },
                    null);

                //create background block
                IncDecInputPanel.Create(sRC.scrollContent, IncDecGridPrefab, "BackGrounds", LCSet.backGroundNumber.ToString(),
                    () => { IncBackGround(); },
                    () => { DecBackGround(); },
                    (val) => { },
                    () => { return LCSet.backGroundNumber.ToString(); },
                    null);
                sRC.OpenScrollPanel(null);
            }
        }

        public void IncVertSize()
        {
            Debug.Log("Click on Button <VerticalSize...> ");
            int vertSize = LCSet.VertSize;
            vertSize += 1;
            LCSet.VertSize = vertSize;
            ShowLevelData();
        }

        public void DecVertSize()
        {
            int vertSize = LCSet.VertSize;
            vertSize = (vertSize > minVertSize) ? --vertSize : minVertSize;
            LCSet.VertSize = vertSize;
            ShowLevelData();
        }

        public void IncHorSize()
        {
            Debug.Log("Click on Button <HorizontalSize...> ");
            int horSize = LCSet.HorSize;
            horSize = (horSize < maxHorSize) ? ++horSize : maxHorSize;
            LCSet.HorSize = horSize;
            ShowLevelData();
        }

        public void DecHorSize()
        {
            Debug.Log("Click on Button <HorizontalSize...> ");
            int horSize = LCSet.HorSize;
            horSize = (horSize > minHorSize) ? --horSize : minHorSize;
            LCSet.HorSize = horSize;
            ShowLevelData();
        }

        public void IncDistX()
        {
            Debug.Log("Click on Button <DistanceX...> ");
            float dist = Mathf.RoundToInt(LCSet.DistX * 100f);
            dist += 5f;
            LCSet.DistX = (dist > 100) ? 1f : dist / 100f;
            ShowLevelData();
        }

        public void DecDistX()
        {
            Debug.Log("Click on Button <DistanceX...> ");
            float dist = Mathf.RoundToInt(LCSet.DistX * 100f);
            dist -= 5f;
            LCSet.DistX = (dist > 0f) ? dist / 100f : 0f;
            ShowLevelData();
        }

        public void IncDistY()
        {
            Debug.Log("Click on Button <DistanceY...> ");
            float dist = Mathf.RoundToInt(LCSet.DistY * 100f);
            dist += 5f;
            LCSet.DistY = (dist > 100) ? 1f : dist / 100f;
            ShowLevelData();
        }

        public void DecDistY()
        {
            Debug.Log("Click on Button <DistanceY...> ");
            float dist = Mathf.RoundToInt(LCSet.DistY * 100f);
            dist -= 5f;
            LCSet.DistY = (dist > 0f) ? dist / 100f : 0f;
            ShowLevelData();
        }

        public void DecScale()
        {
            Debug.Log("Click on Button <Scale...> ");
            int scale = Mathf.RoundToInt(LCSet.Scale * 100f);
            scale -= 5;
            LCSet.Scale = (scale > 0) ? scale/100f : 0f;
            ShowLevelData();
        }

        public void IncScale()
        {
            Debug.Log("Click on Button <Scale...> ");
            int scale = Mathf.RoundToInt(LCSet.Scale *100f);
            scale += 5;
            LCSet.Scale = scale/100f;
            ShowLevelData();
        }

        public void IncBackGround()
        {
            Debug.Log("Click on Button <BackGround...> ");
            LCSet.IncBackGround(GOSet.BackGroundsCount);
            ShowLevelData();
        }

        public void DecBackGround()
        {
            Debug.Log("Click on Button <BackGround...> ");
            LCSet.DecBackGround(GOSet.BackGroundsCount);
            ShowLevelData();
        }
        #endregion grid settings

        #region grid brushes
        public void Cell_Click(GridCell cell)
        {
            if (cell.GRow.IsSeviceRow)
            {
                Debug.Log("Click on cell <" + cell.ToString() + "...> - Is Service cell. Don't use it.");
                return;
            }

            Debug.Log("Click on cell <" + cell.ToString() + "...> ");

            if (currentBrush)
            {
                Debug.Log("object brush ID: " + currentBrush.ID);
                if (cell.HaveObjectWithID(currentBrush.ID))
                {
                    cell.RemoveObject(currentBrush.ID);
                }
                else
                {
                    cell.SetObject(currentBrush.ID, currentBrush.Hits);
                }
                LCSet.SaveObjects(cell);
            }

            CloseOpenedPanels();
        }

        private void CloseOpenedPanels()
        {
            ScrollPanelController[] sRCs = GetComponentsInChildren<ScrollPanelController>();
            foreach (var item in sRCs)
            {
                item.CloseScrollPanel(true, null);
            }

        }

        private void SetSpriteControlActivity(RectTransform panel, bool activity)
        {
            Button[] buttons = panel.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = activity;
            }
        }

        private void DeselectAllBrushes()
        {
            currentBrush = null;
            PanelContainerController[] panelContainerControllers = brushContainersParent.GetComponentsInChildren<PanelContainerController>();

            foreach (var item in panelContainerControllers)
            {
                if (item) item.selector.enabled = false;
            }
        }
        #endregion  brushes

        #region mission
        public void OpenMissionPanel_Click()
        {
            Debug.Log("open mission click");
            BubbleGrid grid = MBoard.grid;

            ScrollPanelController sRC = MissionPanelContainer.ScrollPanel;
            if (sRC) // 
            {
               sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();
                //instantiate ScrollRectController
                sRC = MissionPanelContainer.InstantiateScrollPanel();
                sRC.textCaption.text = "Mission panel";

                MissionConstruct levelMission = LCSet.levelMission;

                //create mission moves constrain
                IncDecInputPanel movesPanel = IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Moves", levelMission.MovesConstrain.ToString(),
                    () => { levelMission.AddMoves(1); },
                    () => { levelMission.AddMoves(-1); },
                    (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelMission.SetMovesCount(res); } },
                    () => { return levelMission.MovesConstrain.ToString(); },
                    null);

                //create time constrain
                IncDecInputPanel.Create(sRC.scrollContent, IncDecPanelPrefab, "Time", levelMission.TimeConstrain.ToString(),
                () => { levelMission.AddTime(1); },
                () => { levelMission.AddTime(-1); },
                (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelMission.SetTime(res); } },
                () => {return levelMission.TimeConstrain.ToString(); },
                null);

               // movesPanel.gameObject.SetActive(!levelMission.IsTimeLevel);

                //description input field
                IncDecInputPanel.Create(sRC.scrollContent, InputTextPanelMissionPrefab, "Description", levelMission.Description,
                null,
                null,
                (val) => { levelMission.SetDescription(val); },
                () => { return levelMission.Description; },
                null);

                // create clean top row check box condition
                IncDecInputPanel.Create(sRC.scrollContent, TogglePanelMissionPrefab, "Clean top row", null,
                  levelMission.LoopTopRow,
                  null,
                  null,
                  null,
                  (val) => { levelMission.SetLoopTopRow(val);},
                  null,
                  null);

                // create raise anchor check box condition
                IncDecInputPanel.Create(sRC.scrollContent, TogglePanelMissionPrefab, "Raise anchor", null,
                  levelMission.RaiseAnchor,
                  null,
                  null,
                  null,
                  (val) => { levelMission.SetRaiseAnchor(val); },
                  null,
                  null);


                //create object targets
                IList<GridObject> tDataL = GOSet.TargetObjects;
                foreach (var item in tDataL)
                {
                    if (item!=null)
                    {
                        Debug.Log("target ID: " + item.ID);
                        int id = item.ID;
                        IncDecInputPanel.Create(sRC.scrollContent, IncDecTogglePanelMissionPrefab, "Target", levelMission.GetTargetCount(id).ToString(),
                        levelMission.GetTargetCount(id) == 10000,
                        () => { levelMission.AddTarget(id, 1); },
                        () => { levelMission.RemoveTarget(id, 1); },
                        (val) => { int res; bool good = int.TryParse(val, out res); if (good) { levelMission.SetTargetCount(id, res); } },
                        (val) => { if (val) { levelMission.SetTargetCount(id, 10000); } else { levelMission.SetTargetCount(id, 0); } },
                        () => { return  levelMission.GetTargetCount(id).ToString(); }, // grid.GetObjectsCountByID(id).ToString()); },
                        item.GuiImage);
                    }
                }

                sRC.OpenScrollPanel(null);
            }
        }
        #endregion mission

        #region load assets
        private T[] LoadResourceAssets<T>(string subFolder) where T : BaseScriptable
        {
            T[] t = Resources.LoadAll<T>(subFolder);
            if (t != null && t.Length > 0)
            {
                string s = "";
                foreach (var m in t)
                {
                    s += m.ToString() + "; ";
                }
                Debug.Log("Scriptable assets <" + typeof(T).ToString() + "> loaded, count: " + t.Length + "; sets : " + s);
            }

            else
            {
                Debug.Log("Scriptable assets <" + typeof(T).ToString() + "> in " + subFolder + " folder"  + " not found!!!");
            }
            return t;
        }
        #endregion load assets

        #region utils
        private void DestroyGOInChildrenWithComponent<T>(Transform parent) where T : Component
        {
            if (!parent) return;
            T[] existComp = parent.GetComponentsInChildren<T>();
            for (int i = 0; i < existComp.Length; i++)
            {
                if(parent.gameObject != existComp[i].gameObject) DestroyImmediate(existComp[i].gameObject);
            }
        }

        private void CreateBrushContainer(Transform parent, PanelContainerController containerPrefab, string capital, List<GridObject> gridObjects)
        {
            if (gridObjects == null || gridObjects.Count == 0)
            {
                Debug.Log("Can't create: " + capital);
                return;
            }
            PanelContainerController c = Instantiate(containerPrefab, parent);
            c.capital = capital;
            c.gridObjects = gridObjects;
            c.OpenCloseButton.onClick.RemoveAllListeners();
            c.OpenCloseButton.onClick.AddListener(() => { CreateBrushPanel(c); });
            c.BrushSelectButton.onClick.RemoveAllListeners();
            c.BrushSelectButton.onClick.AddListener(() =>
            {
                GridObject gO = c.GetOrAddComponent<GridObject>();
                DeselectAllBrushes();
                currentBrush = GOSet.GetObject(gO.ID);//: GOSet.Disabled;
                currentBrush.Hits = gO.Hits;
                c.selector.enabled = true;
                //Debug.Log("current brush: " + currentBrush.ID + " ;hits: " + currentBrush.Hits);
            });
            c.brushImage.sprite = gridObjects[0].ObjectImage;
            c.GetOrAddComponent<GridObject>().Enumerate(gridObjects[0].ID);
            if (!string.IsNullOrEmpty(capital)) c.BrushName.text = capital[0].ToString();
        }

        private void CreateBrushPanel(PanelContainerController container)
        {
            ScrollPanelController sRC = container.ScrollPanel;
            if (sRC)
            {
                sRC.CloseScrollPanel(true, null);
            }
            else
            {
                CloseOpenedPanels();

                sRC = container.InstantiateScrollPanel();
                sRC.textCaption.text = container.capital;

                List<GridObject> mData = new List<GridObject>();
                if (container.gridObjects != null) mData.AddRange(container.gridObjects);
                CreateBrushButtons(mData, smallButtonPrefab, container, sRC.scrollContent, container.brushImage, container.selector);
                sRC.OpenScrollPanel(null);
            }
        }

        private void CreateBrushButtons(List<GridObject> mData, Button prefab, PanelContainerController container, Transform parent, Image objectImage, Image selector)
        {
            //create brushes
            if (mData == null || mData.Count == 0) return;

            for (int i = 0; i < mData.Count; i++)
            {
                GridObject mD = mData[i];
                Sprite[] protectionStateImages = mD.GetProtectionStateImages();

                CreateButton(smallButtonPrefab, parent, mD.ObjectImage, () =>
                {
                    Debug.Log("Click on Button <" + mD.ID + "...> ");
                    DeselectAllBrushes();
                    currentBrush = GOSet.GetObject(mD.ID);
                    objectImage.sprite = currentBrush.ObjectImage;
                    GridObject cGO = container.GetOrAddComponent<GridObject>();
                    cGO.Enumerate(currentBrush.ID);
                    cGO.Hits = 0;
                    currentBrush.Hits = 0;
                    selector.enabled = true;
                });

                if (protectionStateImages != null)
                {
                    int hits = 0;
                    foreach (var item in protectionStateImages)
                    {
                        hits += 1;
                        var tHits = hits;
                        CreateButton(smallButtonPrefab, parent, item, () =>
                        {
                            Debug.Log("Click on Button <" + mD.ID + " ;hits: " + tHits + "...> ");
                            DeselectAllBrushes();
                            currentBrush = GOSet.GetObject(mD.ID);
                            objectImage.sprite = item;
                            GridObject cGO = container.GetOrAddComponent<GridObject>();
                            cGO.Enumerate(currentBrush.ID);
                            cGO.Hits = tHits;
                            currentBrush.Hits = tHits;
                            selector.enabled = true;
                        });
                    }
                }
            }
        }

        private Button CreateButton(Button prefab, Transform parent, Sprite sprite, System.Action listener)
        {
            Button button = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            button.transform.SetParent(parent);
            button.transform.localScale = Vector3.one;
            button.onClick.RemoveAllListeners();
            if (sprite) button.GetComponent<Image>().sprite = sprite;
            if (listener != null) button.onClick.AddListener(() =>
            {
                listener();
            });

            return button;
        }

        private Button CreateButton(Button prefab, Transform parent, Sprite sprite, string text, System.Action listener)
        {
            Button button = CreateButton(prefab, parent, sprite, listener);
            Text t = button.GetComponentInChildren<Text>();
            if (t && text != null) t.text = text;
            return button;
        }

        private void SelectButton(Button b)
        {
            Text t = b.GetComponentInChildren<Text>();
            if (!t) return;
            t.enabled = true;
            t.gameObject.SetActive(true);
            t.text = "selected";
            t.color = Color.black;
        }

        private void DeselectButton(Button b)
        {
            Text t = b.GetComponentInChildren<Text>();
            if (!t) return;
            t.enabled = true;
            t.gameObject.SetActive(true);
            t.text = "";
        }
        #endregion utils
#endif
    }
}