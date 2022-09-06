using System;
using Etourney.Enums.WebSocket;
using Etourney.Exceptions;
using Etourney.Models.WebSocket.Procedures.Tournament.JoinTournament;
using Etourney.Scripts.EtourneyGame;
using Etourney.Scripts.EtourneyPlayer;
using Etourney.Scripts.PublishSubscribe;
using Etourney.Scripts.PublishSubscribe.Enums;
using Etourney.Scripts.WebSocket;
using UnityEngine;

namespace Etourney.Scripts.EtourneyTournaments
{
    public class Tournaments
    {
        private PlayerCredentials _playerCredentials;
        private GameCredentials _gameCredentials;
        private RoomCredentials _roomCredentials;

        public Tournaments()
        {
            Debug.Log("aaa---1>");
            GlobalMediator.AddListener(EQueue.Game, EChannel.ChanelGame, GameCredentials);
            GlobalMediator.AddListener(EQueue.Player, EChannel.ChanelPlayer, PlayerCredentials);
            GlobalMediator.AddListener(EQueue.Room, EChannel.ChanelRoom, RoomCredentials);
            GlobalMediator.AddListener(EQueue.WebSocket, EChannel.ChanelOutWebSocket, WebSocketData);
        }

        private void RoomCredentials(object roomCredentials)
        {
            Debug.Log("aaa---4>");
            _roomCredentials = (RoomCredentials) roomCredentials;
        }

        private void PlayerCredentials(object playerCredentials)
        {
            Debug.Log("aaa---2>");
            _playerCredentials = (PlayerCredentials)playerCredentials;
        }

        private void GameCredentials(object gameCredentials)
        {
            Debug.Log("aaa---3>" + gameCredentials.ToString());
            _gameCredentials = (GameCredentials) gameCredentials;
        }

        public bool JoinToTournament(long tournamentId, Action<WebSocketStatus, object> result)
        {
            Debug.Log("Here is credentials*********" + _gameCredentials + "****PlayerCredentials******" + _playerCredentials);
            if (_gameCredentials == null)
                throw new EtourneyGameKeyException("The game is not authorized.");

            if (_playerCredentials == null)
                throw new EtourneyGameKeyException("The player is not authorized");

            var guid = GlobalWebSocketCallBack.Add(result);
            if (!string.IsNullOrEmpty(guid))
            {
                var wsCallProcedureArgs = new DtoWsProcedureInJoinTournament
                {
                    IdTournament = tournamentId
                };

                var dataInChannel = new CallProcedureData(guid, WebSocketProcedureContext.JoinTournament, wsCallProcedureArgs);

                GlobalMediator.PublishInListeners(EQueue.WebSocket, EChannel.ChanelInWebSocket, dataInChannel);

                return true;
            }

            return false;
        }

        private void WebSocketData(object data)
        {
            Debug.Log("aaa---5>" + data.ToString());
        }
    }
}