using System;

namespace Etourney.Models.WebSocket.Procedures.GameCurrencyPrice.GameCurrencyPrices
{
    [Serializable]
    public class DtoWsProcedureInGameCurrencyPrices
    {
        public int Start;
        public int Count;

        public bool OrderByDesc;
    }
}