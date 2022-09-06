using System;

namespace Etourney.Models.WebSocket
{
    [Serializable]
    internal class WsAnswerProcedure<T> : WsAnswerProcedureBase
    {
        public T Result;
    }
}