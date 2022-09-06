using System;
using System.Collections.Generic;

namespace Etourney.Models.Api.GameCurrencyPrice.GameCurrencyPrices
{
    [Serializable]
    public class ResponseGameCurrencyPrices
    {
        public List<GameCurrencyPrice> GameCurrencyPrices;

        public int Total;

        [Serializable]
        public class GameCurrencyPrice
        {
            public long Id;
            public string Name;
            public string Description;
            public string Icon;
            public string CostInCurrency;
            public string Currency;
        }
    }
}