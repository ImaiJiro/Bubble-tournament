using System;
using System.Collections.Generic;
using Etourney.Enums;
using Etourney.Enums.Api;
using Etourney.Models.Api.Game;
using Etourney.Models.Api.GameCurrencyPrice.GameCurrencyPrices;
using Etourney.Models.Api.Tournaments.Tournaments;
using Etourney.Scripts.Http;
using Etourney.Scripts.PublishSubscribe;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Settings;
using Cysharp.Threading.Tasks;

namespace Etourney.Scripts.EtourneyGame
{
    public class Game
    {
        private readonly HttpWorker _httpWorker;

        private GameCredentials _credentials;
        public GameData Data { get; private set; }

        public Game(string host, int timeout)
        {
            _httpWorker = new HttpWorker(host, timeout);
        }

        public async UniTask<bool> SignInWithKey(Action<string> onError = null)
        {
            var gameKey = EtourneySettings.Instance.ServerLocation == ServerLocation.Local ?
                EtourneySettings.Instance.LocalGameKey :
                EtourneySettings.Instance.GameKey;
            //var gameKey = EtourneySettings.Instance.GameKey;


            if (string.IsNullOrEmpty(gameKey))
                return false;

            var parameters = new Dictionary<string, string>
            {
                { "key", gameKey }
            };

            var result = await _httpWorker.GetHttpRequest<GameTokenByKey>(
                "game/token", parameters, null, onError);

            if (result != null && result.Status == (ushort) ApiStatus.Ok)
            {
                Data = new GameData
                {
                    Name = result.Data.Name,
                    Description = result.Data.Description,
                    Icon = result.Data.Icon,
                    Orientation = result.Data.Orientation
                };

                _credentials = new GameCredentials
                {
                    Token = result.Data.Token
                };

                GlobalMediator.PublishInListeners(EQueue.Game, EChannel.ChanelGame, _credentials);

                return true;
            }

            return false;
        }

        public async UniTask<ResponseGameCurrencyPrices> GameCurrencyPrices(Action<string> onError = null)
        {
            var body = new RequestGameCurrencyPrices
            {
                Start = 0,
                Count = 1000,
                OrderByDesc = false
            };

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {_credentials.Token}" }
            };

            var result = await _httpWorker.PostHttpRequest<ResponseGameCurrencyPrices, RequestGameCurrencyPrices>(
                "game/currency/price", null, headers, body, onError);

            if (result != null && result.Data != null)
            {
                return result.Data;
            }

            return null;
        }

        public async UniTask<ResponseTournaments> GetTournaments(Action<string> onError = null)
        {
            var body = new RequestTournaments
            {
                Start = 0,
                Count = 1000,
                OrderByDesc = false
            };

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {_credentials.Token}" }
            };

            var result = await _httpWorker.PostHttpRequest<ResponseTournaments, RequestTournaments>(
                "tournament/tournaments", null, headers, body, onError);

            if (result != null && result.Data != null)
            {
                return result.Data;
            }

            return null;
        }
    }
}