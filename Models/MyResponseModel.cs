using System.Net;

namespace InboundApi.Models
{
    public class MyResponseModel
    {
        public HttpStatusCode ReturnCode { get; set; }
        public string FileName { get; set; }
        public string Status { get; set; }
    }
}
