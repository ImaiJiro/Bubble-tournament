namespace Mkey
{
    public class BoosterFuncMulticolor : BoosterFunc
    {
        #region override
        public override void ApplyBooster(GridCell hitGridCell, GridCell freeGridCell, CellsGroup group)
        {
            GameEvents.ApplyBoosterEvent?.Invoke(ID);
            Destroy(gameObject);
        }

        public override CellsGroup GetShootArea(GridCell hitGridCell, GridCell freeGridCell, BubbleGrid grid)
        {
            return grid.GetIdArea(freeGridCell);
        }
        #endregion override
    }
}