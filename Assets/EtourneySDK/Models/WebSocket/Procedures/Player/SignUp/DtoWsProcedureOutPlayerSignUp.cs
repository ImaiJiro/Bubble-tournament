using System;

namespace Etourney.Models.WebSocket.Procedures.Player.SignUp
{
    [Serializable]
    public class DtoWsProcedureOutPlayerSignUp
    {
        public string Name;
        public string Email;
        public string Avatar;
        public long Rating;

        public string Token;
    }
}