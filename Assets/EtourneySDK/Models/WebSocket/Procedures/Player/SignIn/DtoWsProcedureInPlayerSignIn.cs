using System;

namespace Etourney.Models.WebSocket.Procedures.Player.SignIn
{
    [Serializable]
    public class DtoWsProcedureInPlayerSignIn
    {
        public bool IsSandbox;
        public string GameKey;

        public string Email;
        public string Password;
    }
}