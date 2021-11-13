namespace BragaPets.Domain.DTOs.Responses
{
    public class ErrorResponse
    {
        public object Source { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
    }
}