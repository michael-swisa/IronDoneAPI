using System.Text.Json;

namespace IronDoneAPI.Middelwares.Attack
{
    public class CreateAttackValidtion
    {
        private readonly RequestDelegate _next;

        public CreateAttackValidtion(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var body = GetBodyAsync(request.Body);
            if (!string.IsNullOrEmpty(body))
            {
                var document = JsonDocument.Parse(body);
                if (!document.RootElement.TryGetProperty("Origin", out var Origin)) { }
            }

            await _next(context);
        }

        private string GetBodyAsync(object body)
        {
            return body.ToString();
        }
    }
}
