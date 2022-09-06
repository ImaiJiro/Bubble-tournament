using UnityEngine;

namespace Mkey
{
    public class BoosterFuncEye : BoosterFunc
    {
        /*
         Description:

         use regular object as shootbubble
         show free greedcell target selector 
         */

        #region temp vars
        private int mainID; // save main id from shootbubble
        #endregion temp vars

        #region reular
        public void Start()
        {
            ShootBubble hiddenShootBubble = BubblesShooter.hiddenShootBubble;

            //use activeShootBubble to create booster
            GameObject g = new GameObject("booster eye");
            SpriteRenderer sR = g.AddComponent<SpriteRenderer>();

            sR.sprite = hiddenShootBubble.sprite;
            sR.sortingOrder = hiddenShootBubble.GetComponent<SpriteRenderer>().sortingOrder;
            g.transform.parent = transform;
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = Vector3.zero;
       
            SRenderer.sortingOrder += 1;
            mainID = hiddenShootBubble.ID;
        }
        #endregion reular

        #region override
        public override void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            BS.DestroyHiddenShootBubble();
            if(group.Length < 2) freeGridCell.SetObject(mainID);
            GameEvents.ApplyBoosterEvent?.Invoke(ID);
            Destroy(gameObject);
        }

        public override CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            return grid.GetIdArea(freeGridCell, mainID);
        }
        #endregion override

    }
}