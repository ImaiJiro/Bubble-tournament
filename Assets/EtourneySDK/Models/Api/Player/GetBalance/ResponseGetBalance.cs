using System;
using System.Collections.Generic;

namespace Etourney.Models.Api.Player.GetBalance
{
    [Serializable]
    public class ResponseGetBalance
    {
        public List<CurrencyBalance> PlayerBalances;

        [Serializable]
        public class CurrencyBalance
        {
            public string CurrencyCode;
            public string Amount;
        }
    }
}