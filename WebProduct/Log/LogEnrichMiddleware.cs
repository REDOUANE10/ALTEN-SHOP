using System.Text.Json;

namespace Api_Store.Log
    {
    public class LogEnrichMiddleware
        {
        private readonly RequestDelegate _next;
        private readonly ILogger<LogEnrichMiddleware> _logger;

        public LogEnrichMiddleware(RequestDelegate next, ILogger<LogEnrichMiddleware> logger)
            {
            _next = next;
            _logger = logger;
            }

        public async Task InvokeAsync(HttpContext context)
            {
            var requestId = Guid.NewGuid().ToString();
            context.Items["RequestId"] = requestId;

            _logger.LogInformation("➡ Début de requête avec RequestId: {RequestId}", requestId);

            try
                {
                await _next(context);
                }
            catch (Exception ex)
                {
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var error = new
                    {
                    RequestId = requestId,                       // ID unique pour tracer la requête dans les logs
                    Message = "Une erreur interne est survenue.", // Message utilisateur générique
                    ExceptionMessage = ex.Message,               // Message de l’exception (technique)
                    ExceptionType = ex.GetType().Name,           // Type de l’exception (ex: NullReferenceException)
                    StackTrace = ex.StackTrace                   // Pile d’appel (utile pour debug, à éviter en prod)
                   
                   
                    };

                var json = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(json);

                                }
            }
        }
    }
