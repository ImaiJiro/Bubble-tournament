using System;

namespace Etourney.Models.Api.Game
{
    [Serializable]
    public class GameTokenByKey
    {
        public string Key;
        public string Name;
        public string Description;
        public string Icon;
        public short Orientation;

        public string Token;
    }
}