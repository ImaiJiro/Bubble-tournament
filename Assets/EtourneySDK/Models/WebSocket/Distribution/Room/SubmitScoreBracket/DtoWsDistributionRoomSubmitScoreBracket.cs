using System;

namespace Etourney.Models.WebSocket.Distribution.Room.SubmitScoreBracket
{
    [Serializable]
    public class DtoWsDistributionRoomSubmitScoreBracket
    {
        /// <summary>
        /// Новый счёт игрока который установил счёт.
        /// </summary>
        public long Score;
        /// <summary>
        /// Имя игрока который установил счёт.
        /// </summary>
        public string Name;
    }
}