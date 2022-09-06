using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mkey
{
    public class OverlayObject : GridObject
    {
        public GameObject hitAnimPrefab;
        public GameObject collectPrefab;

        #region override
        /// <summary>
        /// Create new OverlayObject for gridcell
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="oData"></param>
        /// <param name="addCollider"></param>
        /// <param name="radius"></param>
        /// <param name="isTrigger"></param>
        /// <returns></returns>
        public override GridObject Create(GridCell parent, Action<int> TargetCollectEvent)
        {
            if (!parent || !parent.Mainobject) return null;
            
            if (parent.Overlay)
            {
                GameObject old = parent.Overlay.gameObject;
                Destroy(old);
            }

            OverlayObject gridObject = Instantiate(this, parent.transform);
            if (!gridObject) return null;
            gridObject.transform.localScale = Vector3.one;
            gridObject.transform.localPosition = Vector3.zero;
            gridObject.SRenderer = gridObject.GetComponent<SpriteRenderer>();
#if UNITY_EDITOR
            gridObject.name = "overlay " + parent.ToString();
#endif
            gridObject.TargetCollectEvent = TargetCollectEvent;
            gridObject.SetToFront(false);

            return gridObject;
        }


        public override void ShootAreaCollect(Action completeCallBack, bool showPrivateScore, bool addPrivateScore, bool decProtection, int privateScore)
        {
            Transform parent = transform.parent;

            collectSequence = new TweenSeq();
            SetToFront(true);

            collectSequence.Add((callBack) =>
            {
                TweenExt.DelayAction(gameObject, 0.05f, callBack);
            });

            collectSequence.Add((callBack) =>
            {
                if (hitAnimPrefab)
                {
                    Creator.InstantiateAnimPrefab(hitAnimPrefab, parent, transform.position, SortingOrder.MainExplode);
                    
                }
                if (collectPrefab)
                {
                    Instantiate(collectPrefab, parent);
                }
                callBack();
            });

            collectSequence.Add((callBack) =>
            {
                TargetCollectEvent?.Invoke(ID);
                GameEvents.CollectGridObject?.Invoke(ID);
                completeCallBack?.Invoke();
                DestroyImmediate(gameObject);
                callBack();
            });

            collectSequence.Start();
        }

        public override void SetToFront(bool set)
        {
            if (!SRenderer) SRenderer = GetComponent<SpriteRenderer>();
            if (!SRenderer) return;
            if (set)
                SRenderer.sortingOrder = SortingOrder.OverToFront;
            else
                SRenderer.sortingOrder = SortingOrder.Over;
        }

        public override string ToString()
        {
            return "Overlay: " + ID;
        }
        #endregion override
    }
}
