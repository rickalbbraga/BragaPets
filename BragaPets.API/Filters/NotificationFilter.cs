using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BragaPets.Domain.DTOs.Responses;
using BragaPets.Domain.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace BragaPets.API.Filters
{
    public class NotificationFilter : IAsyncResultFilter
    {
        private const string ContextType = "application/json";
        private const string TitleResponse = "Bad Request";
        private readonly NotificationContext _notificationContext;
        
        public NotificationFilter(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }
        
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (_notificationContext.HasNotifications)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.HttpContext.Response.ContentType = ContextType;

                var errors = _notificationContext.Notifications.Select(e => new ErrorResponse
                {
                    Source = new { Pointer = context.HttpContext.Request.Path },
                    Title = TitleResponse,
                    Details = e.Message
                }).ToList();

                var notifications = JsonConvert.SerializeObject(new BaseErrorResponse(errors));
                await context.HttpContext.Response.WriteAsync(notifications);

                return;
            }

            await next();
        }
    }
}