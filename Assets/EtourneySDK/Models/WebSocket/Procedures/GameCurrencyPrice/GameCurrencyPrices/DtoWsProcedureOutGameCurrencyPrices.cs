using System;
using System.Collections.Generic;

namespace Etourney.Models.WebSocket.Procedures.GameCurrencyPrice.GameCurrencyPrices
{
    [Serializable]
    public class DtoWsProcedureOutGameCurrencyPrices
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
            public decimal CostInCurrency;
            public decimal Currency;
        }
    }
}