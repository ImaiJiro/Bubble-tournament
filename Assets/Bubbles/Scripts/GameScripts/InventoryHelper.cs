using UnityEngine;

namespace Mkey
{
    public class InventoryHelper : MonoBehaviour
    {
        private BubblesPlayer MPlayer => BubblesPlayer.Instance;

        public void AddCoins(int count)
        {
            if (MPlayer != null)
            {
                MPlayer.AddCoins(count);
            }
        }

        public void AddLife(int count)
        {
            if (MPlayer != null)
            {
                MPlayer.AddLifes(count);
            }
        }

        public void SetInfiniteLife(int count)
        {
            if (MPlayer != null)
            {
                MPlayer.StartInfiniteLife(count);
            }
        }

        /// <summary>
        /// Buy 1 booster in ingameshop
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="prodName"></param>
        /// <param name="count"></param>
        /// <param name="price"></param>
        public void AddBooster(BoosterFunc boosterFunc)
        {
            if (!boosterFunc) return;
            boosterFunc.AddCount(1);
        }

        /// <summary>
        /// Buy 5 boosters in ingameshop
        /// </summary>
        /// <param name="prodID"></param>
        /// <param name="prodName"></param>
        /// <param name="count"></param>
        /// <param name="price"></param>
        public void AddBooster_5(BoosterFunc boosterFunc)
        {
            if (!boosterFunc) return;
            boosterFunc.AddCount(5);
        }

        public void AddBoosterPack(BoosterPack boosterPack)
        {
            if (!boosterPack || !boosterPack.boosterFunc) return;
            boosterPack.boosterFunc.AddCount(boosterPack.count);
        }
    }
}