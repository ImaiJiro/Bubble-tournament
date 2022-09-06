using System;

namespace Etourney.Models.Api.GameCurrencyPrice.GameCurrencyPrices
{
    [Serializable]
    public class RequestGameCurrencyPrices
    {
        public int Start;
        public int Count;

        public bool OrderByDesc;
    }
}