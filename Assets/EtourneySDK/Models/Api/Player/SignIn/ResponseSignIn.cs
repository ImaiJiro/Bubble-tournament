using System;

namespace Etourney.Models.Api.Player.SignIn
{
    [Serializable]
    internal class ResponseSignIn
    {
        public string Name;
        public string Email;
        public string Avatar;
        public long Rating;

        public string Token;
    }
}