using System;

namespace Etourney.Models.WebSocket
{
    [Serializable]
    internal class WsCallProcedureBase
    {
        public string G;
        public ushort C;

        public WsCallProcedureBase()
        {
        }

        public WsCallProcedureBase(string guid, ushort context)
        {
            G = guid;
            C = context;
        }
    }
}