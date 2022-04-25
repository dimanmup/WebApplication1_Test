using System.Net;

namespace WebApplication1_Test.Models
{
    public class ErrorModel
    {
        public string Title { get; set; } = "Internal server error";
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
        public string? Message { get; set; }
    }
}
