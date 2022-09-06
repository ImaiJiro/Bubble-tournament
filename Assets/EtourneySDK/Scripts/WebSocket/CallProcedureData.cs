using Etourney.Enums.WebSocket;

namespace Etourney.Scripts.WebSocket
{
    internal struct CallProcedureData
    {
        public string Guid;
        public ushort Context;
        public object Data;

        public CallProcedureData(string guid, WebSocketProcedureContext context, object data)
        {
            Guid = guid;
            Context = (ushort) context;
            Data = data;
        }
    }
}