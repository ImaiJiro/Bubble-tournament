using System;

namespace Etourney.Models.Api.Player.SignIn
{
    [Serializable]
    internal class RequestSignIn
    {
        public string Email;
        public string Password;
    }
}