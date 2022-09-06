using System;

namespace Etourney.Models.WebSocket
{
    [Serializable]
    internal class WsCallProcedure<T> : WsCallProcedureBase
    {
        public T A;

        public WsCallProcedure()
        {
        }

        public WsCallProcedure(string guid, ushort context, T args) : base(guid, context)
        {
            A = args;
        }
    }
}