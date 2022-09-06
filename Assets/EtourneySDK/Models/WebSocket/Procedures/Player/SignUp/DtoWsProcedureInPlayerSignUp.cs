using System;

namespace Etourney.Models.WebSocket.Procedures.Player.SignUp
{
    [Serializable]
    public class DtoWsProcedureInPlayerSignUp
    {
        public bool IsSandbox;
        public string GameKey;

        public string Email;
        public string Password;
        public string Name;
        public string Avatar;
    }
}