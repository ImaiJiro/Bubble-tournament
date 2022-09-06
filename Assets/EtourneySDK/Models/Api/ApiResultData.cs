using System;

namespace Etourney.Models.Api
{
    [Serializable]
    internal class ApiResultData<T> : ApiResult
    {
        public T Data;
    }
}