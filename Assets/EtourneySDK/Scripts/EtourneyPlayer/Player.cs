using System;
using System.Collections.Generic;
using Etourney.Enums;
using Etourney.Enums.WebSocket;
using Etourney.Models.Api.Player.GetBalance;
using Etourney.Models.Api.Shop.TopUp;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Models.WebSocket.Procedures.Player.SignUp;
using Etourney.Scripts.Http;
using Etourney.Scripts.PublishSubscribe;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Scripts.WebSocket;
using Etourney.Settings;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Environment = Etourney.Enums.Environment;

namespace Etourney.Scripts.EtourneyPlayer
{
    public class Player
    {
        private readonly HttpWorker _httpWorker;

        private PlayerCredentials _credentials;
        public PlayerData Data { get; private set; }

        public Player(string host, int timeout)
        {
            _httpWorker = new HttpWorker(host, timeout);

            GlobalMediator.AddListener(EQueue.WebSocket, EChannel.ChanelOutWebSocket, Listener);
        }

        public bool SignUpWithEmailAndPassword(string email, string password, string name, string avatar, Action<WebSocketStatus, object> result)
        {
            var guid = GlobalWebSocketCallBack.Add(result);

            if (!string.IsNullOrEmpty(guid))
            {
                string gameKey = EtourneySettings.Instance.LocalGameKey;
                //string gameKey = "dJfLAMpcoN+TwyyxTnp7B2+71WxOXlKYGBg86r3CnGRqwjXhQEYC7XghZcx92kc595qbBZcoWBpaUu6xaj6M9etJe0fRmvKQeB8s2j8q/XqEvXtBLVaaG2mjzWkqlW2wjzmVv2lOdJngLFCoKKbvjpZ7rC+fjyODTD9q3UAI1K2DkISZtHxjva7CDs4talxOdMWCNchKDqJct/gJIxPo9cgEbVaYMV+MBaEoLJVDaDYmzwgEdGodAdsO1c8WmcKLmgJrbtwukk+9HmuNPmcTKP6+fb6FRe3KJO3z1coKsRPslbPZdc4Lr6sx8ZyqC6zxJxv4VPVrtRY6rUfSE4w+g9BkhucR+liAu9DQOOMjIU4UAVkpiM3KwbRCh+6a7aiLAFEHpbRPWLMhccbJ28KMlA==";
                var isSandbox = EtourneySettings.Instance.Environment == Environment.Sandbox;

                if (EtourneySettings.Instance.ServerLocation == ServerLocation.Remote)
                    gameKey = EtourneySettings.Instance.GameKey;

                var dto = new DtoWsProcedureInPlayerSignUp
                {
                    IsSandbox = isSandbox,
                    GameKey = gameKey,
                    Email = email,
                    Password = password,
                    Name = name,
                    Avatar = avatar
                };

                var dataInChannel = new CallProcedureData(guid, WebSocketProcedureContext.SignUpByEmail, dto);

                GlobalMediator.PublishInListeners(EQueue.WebSocket, EChannel.ChanelInWebSocket, dataInChannel);

                return true;
            }

            return false;
        }

        public bool SignInWithEmailAndPassword(string email, string password, Action<WebSocketStatus, object> result)
        {
            Debug.Log("****Here is Login Data****--->" + email + "," + password);
            Debug.Log("****Here is Etourney settings - Local Game Key****" + EtourneySettings.Instance.LocalGameKey);
            Debug.Log("****Here is Etourney settings - Remote Game Key****" + EtourneySettings.Instance.GameKey);
            var guid = GlobalWebSocketCallBack.Add(result);

            if (!string.IsNullOrEmpty(guid))
            {
                string gameKey = EtourneySettings.Instance.LocalGameKey;
                //string gameKey = "dJfLAMpcoN+TwyyxTnp7B2+71WxOXlKYGBg86r3CnGRqwjXhQEYC7XghZcx92kc595qbBZcoWBpaUu6xaj6M9etJe0fRmvKQeB8s2j8q/XqEvXtBLVaaG2mjzWkqlW2wjzmVv2lOdJngLFCoKKbvjpZ7rC+fjyODTD9q3UAI1K2DkISZtHxjva7CDs4talxOdMWCNchKDqJct/gJIxPo9cgEbVaYMV+MBaEoLJVDaDYmzwgEdGodAdsO1c8WmcKLmgJrbtwukk+9HmuNPmcTKP6+fb6FRe3KJO3z1coKsRPslbPZdc4Lr6sx8ZyqC6zxJxv4VPVrtRY6rUfSE4w+g9BkhucR+liAu9DQOOMjIU4UAVkpiM3KwbRCh+6a7aiLAFEHpbRPWLMhccbJ28KMlA==";
                var isSandbox = EtourneySettings.Instance.Environment == Environment.Sandbox;

                if (EtourneySettings.Instance.ServerLocation == ServerLocation.Remote)
                    gameKey = EtourneySettings.Instance.GameKey;

                var dto = new DtoWsProcedureInPlayerSignIn
                {
                    IsSandbox = isSandbox,
                    GameKey = gameKey,

                    Email = email,
                    Password = password
                };

                var dataInChannel = new CallProcedureData(guid, WebSocketProcedureContext.SignInByEmail, dto);

                GlobalMediator.PublishInListeners(EQueue.WebSocket, EChannel.ChanelInWebSocket, dataInChannel);

                return true;
            }

            return false;
        }

        public async UniTask<ResponseTopUpBalance> TopUpBalance(long id, Action<string> onError = null)
        {
            var isSandbox = SettingsLoader.Settings.Environment == Environment.Sandbox;

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {_credentials.Token}" }
            };

            var parameters = new Dictionary<string, string>
            {
                { "isSandbox", isSandbox.ToString() },
                { "id", id.ToString() }
            };

            var result = await _httpWorker.GetHttpRequest<ResponseTopUpBalance>(
                "shop/currency/top-up", parameters, headers, onError);

            return result.Data;
        }

        public async UniTask<ResponseGetBalance> GetBalance(Action<string> onError = null)
        {
            var isSandbox = SettingsLoader.Settings.Environment == Environment.Sandbox;

            var headers = new Dictionary<string, string>
            {
                { "Authorization", $"Bearer {_credentials.Token}" }
            };

            var parameters = new Dictionary<string, string>
            {
                { "isSandbox", isSandbox.ToString() }
            };

            var result = await _httpWorker.GetHttpRequest<ResponseGetBalance>(
                "player/get/balance", parameters, headers, onError);

            return result.Data;
        }

        private void Listener(object obj)
        {
            try
            {
                var str = (string) obj;
                var parseBase = JsonUtility.FromJson<WsAnswerProcedureBase>(str);

                if (parseBase.C == (ushort) WebSocketProcedureContext.SignUpByEmail)
                {
                    var parseBody = JsonUtility.FromJson<WsAnswerProcedure<DtoWsProcedureOutPlayerSignUp>>(str);

                    _credentials = new PlayerCredentials(parseBody.Result.Token);
                    Data = new PlayerData(parseBody.Result.Name, parseBody.Result.Email, parseBody.Result.Avatar);
                }

                if (parseBase.C == (ushort) WebSocketProcedureContext.SignInByEmail)
                {
                    var parseBody = JsonUtility.FromJson<WsAnswerProcedure<DtoWsProcedureOutPlayerSignIn>>(str);

                    _credentials = new PlayerCredentials(parseBody.Result.Token);
                    Debug.Log("here is player credendials***********>" + _credentials);
                    Data = new PlayerData(parseBody.Result.Name, parseBody.Result.Email, parseBody.Result.Avatar);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }
        }
    }
}