using System;

namespace Etourney.Models.Api.Player.SignUp
{
    [Serializable]
    internal class ResponseSignUp
    {
        public string Name;
        public string Email;
        public string Avatar;

        public string Token;
    }
}