using System;

namespace Etourney.Models.WebSocket
{
    [Serializable]
    internal class WsAnswerProcedureBase
    {
        public string G;
        public ushort C;
        public ushort S;
    }
}