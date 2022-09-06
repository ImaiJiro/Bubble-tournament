using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Etourney.Models.Api;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Etourney.Scripts.Http
{
    internal class HttpWorker
    {
        private readonly Uri _host;
        private readonly int _requestTimeoutMilliseconds;

        public HttpWorker(string host, int timeout)
        {
            _host = new Uri(host);
            _requestTimeoutMilliseconds = timeout;
        }

        public async UniTask<ApiResult> GetHttpRequest(string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            Action<string> onError)
        {
            return await RequestWithoutData<object>(UnityWebRequest.kHttpVerbGET, apiName, parameters, headers, null, onError);
        }

        public async UniTask<ApiResultData<TResult>> GetHttpRequest<TResult>(string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            Action<string> onError)
            where TResult : class
        {
            return await RequestData<TResult, object>(UnityWebRequest.kHttpVerbGET, apiName, parameters, headers, null, onError);
        }

        public async UniTask<ApiResultData<TResult>> PostHttpRequest<TResult, TBody>(string apiName,
             Dictionary<string, string> parameters,
             Dictionary<string, string> headers,
             TBody body,
             Action<string> onError)
             where TResult : class
        {
            return await RequestData<TResult, TBody>(UnityWebRequest.kHttpVerbPOST, apiName, parameters, headers, body, onError);
        }

        public async UniTask<ApiResult> PostHttpRequest<TBody>(string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            TBody body,
            Action<string> onError)
        {
            return await RequestWithoutData(UnityWebRequest.kHttpVerbPOST, apiName, parameters, headers, body, onError);
        }

        public async UniTask<ApiResultData<TResult>> DeleteHttpRequest<TResult>(string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            Action<string> onError)
            where TResult : class
        {
            return await RequestData<TResult, object>(UnityWebRequest.kHttpVerbDELETE, apiName, parameters, headers, null, onError);
        }

        private async UniTask<ApiResultData<TResult>> RequestData<TResult, TBody>(string type,
            string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            TBody body,
            Action<string> onError)
            where TResult : class
        {
            var result = await Request(type, apiName, parameters, headers, body, onError);
            var data = !string.IsNullOrEmpty(result) ? JsonUtility.FromJson<ApiResultData<TResult>>(result) : null;

            return data;
        }

        private async UniTask<ApiResult> RequestWithoutData<TBody>(string type,
            string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            TBody body,
            Action<string> onError)
        {
            var result = await Request(type, apiName, parameters, headers, body, onError);

            return !string.IsNullOrEmpty(result) ? JsonUtility.FromJson<ApiResult>(result) : null;
        }

        private async UniTask<string> Request<TBody>(string type,
            string apiName,
            Dictionary<string, string> parameters,
            Dictionary<string, string> headers,
            TBody body,
            Action<string> onError)
        {
            Uri uri = GenerateUri(apiName, parameters);
            UnityWebRequest webRequest = null;

            try
            {
                if (type.Equals(UnityWebRequest.kHttpVerbGET))
                {
                    webRequest = UnityWebRequest.Get(uri);
                }

                if (type.Equals(UnityWebRequest.kHttpVerbPOST) && body != null)
                {
                    string stringBody = JsonUtility.ToJson(body);

                    webRequest = new UnityWebRequest(uri, "POST");
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(stringBody);
                    webRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
                    webRequest.downloadHandler = new DownloadHandlerBuffer();
                    webRequest.SetRequestHeader("Content-Type", "application/json-patch+json; charset=utf-8");
                    webRequest.SetRequestHeader("Accept", "*/*");
                }

                if (type.Equals(UnityWebRequest.kHttpVerbDELETE))
                {
                    webRequest = UnityWebRequest.Delete(uri);
                }

                if (webRequest != null)
                {
                    webRequest.certificateHandler = new AcceptAllCertificates();
                    webRequest.timeout = _requestTimeoutMilliseconds;

                    if (headers != null)
                    {
                        for (int i = 0; i < headers.Count; i++)
                        {
                            KeyValuePair<string, string> item = headers.ElementAt(i);

                            webRequest.SetRequestHeader(item.Key, item.Value);
                        }
                    }

                    var op = await webRequest.SendWebRequest();
                    var webRequestResult = op.downloadHandler.text;

                    Debug.Log(webRequest.result);

                    if (webRequest.result == UnityWebRequest.Result.Success)
                    {
                        return webRequestResult;
                    }
                    else
                    {
                        if (onError != null)
                            onError(webRequest.error);
                    }
                }

                return null;
            }
            catch (Exception exception)
            {
                if (onError != null)
                    onError(exception.Message);

                return null;
            }
            finally
            {
                if (webRequest != null)
                    webRequest.Dispose();
            }
        }

        private Uri GenerateUri(string apiName,
            Dictionary<string, string> parameters)
        {
            if (parameters != null && parameters.Count > 0)
            {
                string parametersString = "?";
                for (int i = 0; i < parameters.Count; i++)
                {
                    KeyValuePair<string, string> item = parameters.ElementAt(i);
                    parametersString += item.Key + "=" + WebUtility.UrlEncode(item.Value);
                    
                    if (i + 1 != parameters.Count)
                        parametersString += "&";
                }
                
                return new Uri(_host, apiName + parametersString);
            }

            return new Uri(_host, apiName);
        }
    }
}