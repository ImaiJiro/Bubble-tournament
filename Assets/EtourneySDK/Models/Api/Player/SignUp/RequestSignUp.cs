using System;

namespace Etourney.Models.Api.Player.SignUp
{
    [Serializable]
    internal class RequestSignUp
    {
        public string Email;
        public string Password;
        public string Name;
        public string Avatar;
    }
}