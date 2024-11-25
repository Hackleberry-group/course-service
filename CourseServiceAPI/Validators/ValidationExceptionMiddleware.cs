using System.Net;
using FluentValidation;
using Newtonsoft.Json;

namespace CourseServiceAPI.Middleware
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationExceptionMiddleware> _logger;

        public ValidationExceptionMiddleware(RequestDelegate next, ILogger<ValidationExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogError(ex, "Validation error occurred.");
                await HandleValidationExceptionAsync(context, ex);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new ValidationErrorResponse
            {
                Message = "One or more validation errors occurred.",
                Errors = ex.Errors.Select(e => new ValidationError
                {
                    Field = e.PropertyName,
                    Error = e.ErrorMessage
                }).ToList()
            };

            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }

    public class ValidationErrorResponse
    {
        public string Message { get; set; }
        public List<ValidationError> Errors { get; set; }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Error { get; set; }
    }
}