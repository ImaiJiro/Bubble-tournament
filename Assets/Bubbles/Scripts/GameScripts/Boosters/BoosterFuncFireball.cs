namespace Mkey
{
    public class BoosterFuncFireball : BoosterFunc
    {
        /*
        Description:
        
        collect verical column with hitTarget
        show target selector and shoot collection area
        */

        #region override
        public override void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            GameEvents.ApplyBoosterEvent?.Invoke(ID);
            Destroy(gameObject);
        }

        public override CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            return grid.GetColumnArea(hitGridCell);
        }
        #endregion override
    }
}