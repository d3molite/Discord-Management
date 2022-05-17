using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace DiscordApi.Services
{
    public class SerilogExceptionLogger : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            
            Log.Error("Unhandled Exception in Application", exception);

            context.ExceptionHandled = true;
        }
    }
}
