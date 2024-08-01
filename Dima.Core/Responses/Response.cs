using System.Text.Json.Serialization;

namespace Dima.Core.Responses
{
    public class Response<T>
    {
        private readonly int _code;

        [JsonConstructor]
        public Response() => _code = Configurations.DefaultStatusCode;


        public Response(T? data, int code = Configurations.DefaultStatusCode, string? message = null)
        {
            Data = data;
            Message = message;
            _code = code;
        }

        public T? Data { get; set; }
        public string? Message { get; set; }

        [JsonIgnore]
        public bool IsSuccess => _code is >= 200 and <= 299;
    }
}
