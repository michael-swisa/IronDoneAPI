namespace IronDoneAPI.Middelwares.Global
{
    public class GlobalLoginMiddelware
    {
        private readonly RequestDelegate _next;

        public GlobalLoginMiddelware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            Console.WriteLine(
                $"Got Request to server: {request.Method} {request.Path}"
                    + $"From IP: {request.HttpContext.Connection.RemoteIpAddress}"
            );
            await this._next(context);
        }
    }
}
