using System.Collections.Generic;

namespace BragaPets.Domain.DTOs.Responses
{
    public class BaseErrorResponse
    {
        public IEnumerable<ErrorResponse> Errors { get; set; }

        public BaseErrorResponse(IEnumerable<ErrorResponse> errors)
        {
            Errors = errors;
        }
    }
}