using System;

namespace Etourney.Models.WebSocket.Procedures.Player.SignIn
{
    [Serializable]
    public class DtoWsProcedureOutPlayerSignIn
    {
        public string Name;
        public string Email;
        public string Avatar;
        public long Rating;

        public string Token;
    }
}