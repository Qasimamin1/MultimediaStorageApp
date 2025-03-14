using System.Net;
using System.Text.Json.Serialization;

namespace Shared
//The Response class functions as a uniform container for all responses
//sent from your server to clients. It standardizes the structure of responses
//by consistently including a success status, a descriptive message, and any
//relevant data from the server, ensuring that clients always receive response
//s in a predictable format.
{
    public class Response
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Message { get; set; } = Shared.Message.Success;
        public dynamic Data { get; set; }
        public bool Status { get { return StatusCode == HttpStatusCode.OK; } }
    }
}
