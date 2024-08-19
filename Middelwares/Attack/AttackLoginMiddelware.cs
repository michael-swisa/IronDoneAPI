namespace IronDoneAPI.Middelwares.Attack
{
    public class AttackLoginMiddelware
    {
        private readonly RequestDelegate _next;

        public AttackLoginMiddelware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"Inside AttackLoginMiddelware");
            await _next(context);
        }
    }
}
