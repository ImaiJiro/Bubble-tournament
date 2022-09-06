using UnityEngine;
using System;
using System.Collections.Generic;

namespace Mkey
{
    public enum Match {ById, NotMatch }
    [CreateAssetMenu]
    public class GameObjectsSet : BaseScriptable, ISerializationCallbackReceiver
    {
        public Sprite[] backGrounds;
        public Sprite selector;

        public GameObject gridCellPrefab;

        [SerializeField]
        private List<MainObject> mainObjects;
        [SerializeField]
        private List<OverlayObject> overlayObjects;
        [SerializeField]
        private List<BoosterFunc> boosterObjects;

        private List<MainObject> shootBubbles; // temporary store

        private List<GridObject> targetObjects;

        #region temp vars
        private bool enumerated = false;
        private bool targetsCreated = false;
        #endregion temp vars

        #region properties
        public IList<MainObject> MainObjects
        {
            get { if (!enumerated) Enumerate(); return mainObjects.AsReadOnly(); }
        }

        public IList<OverlayObject> OverlayObjects
        {
            get { if (!enumerated) Enumerate(); return overlayObjects.AsReadOnly(); }
        }

        public IList<BoosterFunc> BoosterObjects { get { if (!enumerated) Enumerate(); return boosterObjects.AsReadOnly(); } }

        public IList<GridObject> TargetObjects { get { CreateTargets(); return targetObjects.AsReadOnly(); } }

        public int RegularLength
        {
            get { return MainObjects.Count; }
        }
        #endregion properties

        void OnEnable()
        {
            enumerated = false;
            targetsCreated = false;
        }

        #region serialization
        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            //   Debug.Log("deserialized ");
        }
        #endregion serialization

        #region get object
        /// <summary>
        /// Returns reference  object from set.
        /// </summary>
        /// <returns>Reference letter</returns>
        public MainObject GetMainObject(int id)
        {
            foreach (var item in MainObjects)
            {
                if (item && id == item.ID) return item;
            }
            return null;
        }

        public List<MainObject> GetMainRandomObjects(int count)
        {
            List<MainObject> r = new List<MainObject>(count);
            IList<MainObject> source = MainObjects;

            for (int i = 0; i < count; i++)
            {
                int rndNumber = UnityEngine.Random.Range(0, source.Count);
                r.Add(source[rndNumber]);
            }
            return r;
        }

        /// <summary>
        /// Returns random MainObjectData array without "notInclude" list featured objects .
        /// </summary>
        public List<MainObject> GetMainRandomObjects(int count, List<GridObject> notInclude)
        {
            List<MainObject> r = new List<MainObject>(count);
            List<MainObject> source = new List<MainObject>(MainObjects);

            if (notInclude != null)
                for (int i = 0; i < notInclude.Count; i++)
                {
                    source.RemoveAll((mOD) => { return mOD.ID == notInclude[i].ID; });
                }

            for (int i = 0; i < count; i++)
            {
                int rndNumber = UnityEngine.Random.Range(0, source.Count);
                r.Add(source[rndNumber]);
            }
            return r;
        }

        /// <summary>
        /// Returns random MainObjectData array only shoot bubbles.
        /// </summary>
        public MainObject GetMainRandomObjectsShootBubble()
        {
            if (shootBubbles == null)
            {
                shootBubbles = new List<MainObject>(MainObjects.Count);
                foreach (var item in MainObjects)
                {
                    if (item.CanUseAsShootBubbles)
                    {
                        shootBubbles.Add(item);
                    }
                }
            }

            int rndNumber = UnityEngine.Random.Range(0, shootBubbles.Count);
            return shootBubbles[rndNumber];
        }

        /// <summary>
        /// Returns MainObjectData array only shoot bubbles.
        /// </summary>
        public List<MainObject> GetMainObjectsShootBubbles()
        {
            if (shootBubbles == null)
            {
                shootBubbles = new List<MainObject>(MainObjects.Count);
                foreach (var item in MainObjects)
                {
                    if (item.CanUseAsShootBubbles)
                    {
                        shootBubbles.Add(item);
                    }
                }
            }
            return shootBubbles;
        }

        /// <summary>
        /// Returns reference  object from set.
        /// </summary>
        /// <returns>Reference letter</returns>
        public OverlayObject GetOverlayObject(int id)
        {
            foreach (var item in OverlayObjects)
            {
                if (item && id == item.ID) return item;
            }
            return null;
        }

        /// <summary>
        /// Returns reference  object from set.
        /// </summary>
        /// <returns>Reference letter</returns>
        public GridObject GetObject(int id)
        {
            foreach (var item in MainObjects)
            {
                if (id == item.ID) return item;
            }

            foreach (var item in OverlayObjects)
            {
                if (id == item.ID) return item;
            }
            return null;
        }

        #endregion get object

        #region contain
        public bool ContainID(int id)
        {
            return
                (
                   ContainMatchID(id)
                || ContainOverlayID(id)
                || ContainBoosterID(id)
                );
        }

        public bool ContainMatchID(int id)
        {
            return ContainID(MainObjects, id);
        }

        public bool ContainBoosterID(int id)
        {
            if (BoosterObjects == null || BoosterObjects.Count == 0) return false;
            foreach (var item in BoosterObjects)
            {
                if (item.ID == id) return true;
            }
            return false;
        }

        public bool ContainOverlayID(int id)
        {
            return ContainID(OverlayObjects, id);
        }

        public bool ContainTargetID(int id)
        {
            return ContainID(TargetObjects, id);
        }

        #endregion contain

        public Sprite GetBackGround(int index)
        {
            index = (int)Mathf.Repeat(index, backGrounds.Length);
            return backGrounds[index];
        }

        public int BackGroundsCount
        {
            get { return backGrounds.Length; }
        }

        private void CreateTargets()
        {
            if (targetsCreated) return;
            targetObjects = new List<GridObject>();

            if (OverlayObjects != null)
                foreach (var item in OverlayObjects)
                {
                    if (item.canUseAsTarget) targetObjects.Add(item);
                }

            if (MainObjects != null)
                foreach (var item in MainObjects)
                {
                    if (item && item.canUseAsTarget) targetObjects.Add(item);
                }

            Debug.Log("Targets Created");
            targetsCreated = true;
        }

        private void Enumerate()
        {
            if (enumerated) return;
            // set ids for game objects
            EnumerateArray(mainObjects, 10);
            EnumerateArray(overlayObjects, 100000);

            int startIndex = 300000;
            foreach (var item in boosterObjects)
            {
                item.Enumerate(startIndex++);
            }
        }

        #region utils
        private void EnumerateArray<T>(ICollection<T> a, int startIndex) where T : GridObject
        {
            if (a != null && a.Count > 0)
            {
                foreach (var item in a)
                {
                    item.Enumerate(startIndex++);
                }
            }
        }

        private bool ContainID<T>(ICollection<T> a, int id) where T : GridObject
        {
            if (a == null || a.Count == 0) return false;
            foreach (var item in a)
            {
                if (item.ID == id) return true;
            }
            return false;
        }
        #endregion utils
    }

    [Serializable]
    public class CellData
    {
        [SerializeField]
        private int id;
        [SerializeField]
        private int row;
        [SerializeField]
        private int column;

        public int ID { get { return id; } }
        public int Row { get { return row; } }
        public int Column { get { return column; } }

        public CellData(int id, int row, int column)
        {
            this.row = row;
            this.column = column;
            this.id = id;
        }
    }

    /// <summary>
    /// Helper serializable class object with the equal ID
    /// </summary>
    [Serializable]
    public class ObjectsSetData
    {
        public Action<int> ChangeEvent;

        [SerializeField]
        private int id;
        [SerializeField]
        private int count;

        public int ID { get { return id; } }
        public int Count { get { return count; } }

        public ObjectsSetData(int id, int count)
        {
            this.id = id;
            this.count = count;
        }

        public Sprite GetImage(GameObjectsSet mSet)
        {
            return mSet.GetMainObject(id).ObjectImage;
        }

        public void IncCount()
        {
            SetCount(count + 1);
        }

        public void DecCount()
        {
            SetCount(count - 1);
        }

        public void SetCount(int newCount)
        {
            newCount = Mathf.Max(0, newCount);
            bool changed = (Count != newCount);
            count = newCount;
            if (changed) ChangeEvent?.Invoke(count);
        }
    }

    /// <summary>
    /// Helper class that contains list of object set data 
    /// </summary>
    [Serializable]
    public class ObjectSetCollection
    {
        [SerializeField]
        private List<ObjectsSetData> list;

        public IList<ObjectsSetData> ObjectsList { get { return list.AsReadOnly(); } }

        public ObjectSetCollection()
        {
            list = new List<ObjectsSetData>();
        }

        public ObjectSetCollection(ObjectSetCollection oSCollection)
        {
            list = new List<ObjectsSetData>();
            Add(oSCollection);
        }

        public ObjectSetCollection(List<ObjectsSetData> oSCollection)
        {
            list = new List<ObjectsSetData>();
            Add(oSCollection);
        }

        public uint Count
        {
            get { return(list == null) ? 0 : (uint)list.Count; }
        }

        public void AddById(int id, int count)
        {

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list[i].SetCount(list[i].Count + count);
                    return;
                }
            }
            list.Add(new ObjectsSetData(id, count));
        }

        public void RemoveById(int id, int count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list[i].SetCount(list[i].Count - count);
                    return;
                }
            }
        }

        public void SetCountById(int id, int count)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list[i].SetCount(count);
                    return;
                }
            }
            list.Add(new ObjectsSetData(id, count));
        }

        public void CleanById(int id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list.RemoveAt(i);
                    return;
                }
            }
        }

        public int CountByID(int id) 
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                    return  list[i].Count;
            }
            return 0;
        }

        public bool ContainObjectID(int id)
        {
            return CountByID(id)>0;
        }

        public void Add(ObjectSetCollection oSCollection)
        {
            if (oSCollection != null)
            {
                foreach (var item in oSCollection.ObjectsList)
                {
                    AddById(item.ID, item.Count);
                }
            }
        }

        public void Add(List<ObjectsSetData> oSCollection)
        {
            if (oSCollection != null)
            {
                foreach (var item in oSCollection)
                {
                    AddById(item.ID, item.Count);
                }
            }
        }

    }
}

