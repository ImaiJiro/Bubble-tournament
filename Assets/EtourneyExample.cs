using Etourney.Enums;
using Etourney.Enums.WebSocket;
using Etourney.Models.WebSocket;
using Etourney.Models.WebSocket.Procedures.Player.SignIn;
using Etourney.Models.WebSocket.Procedures.Player.SignUp;
using Etourney.Scripts;
using UnityEngine;

public class EtourneyExample : MonoBehaviour
{
    void Start()
    {
        EtourneySDK.OnStatus += status =>
        {
            Debug.Log(status);

            if (status == EtourneyStatus.Connected)
            {
                //EtourneySDK.Player.SignUpWithEmailAndPassword("Example@google.com", "1234567890", "Test_999", "None", Result);
                EtourneySDK.Player.SignInWithEmailAndPassword("Example@google.com", "1234567890", Result);
            }
        };

        EtourneySDK.WebSocketConnect();
    }

    private void Result(WebSocketStatus status, object data)
    {
        Debug.Log(status);

        if (status == WebSocketStatus.Ok)
        {
            var parseData = (DtoWsProcedureOutPlayerSignIn) data;
            //var parseData = (DtoWsProcedureOutPlayerSignUp) data;
        }
    }

    void Update()
    {
        
    }
}