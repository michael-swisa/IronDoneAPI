using IronDoneAPI.Middelwares.Attack;
using IronDoneAPI.Middelwares.Global;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.UseMiddleware<GlobalLoginMiddelware>();

app.UseWhen(
    context => context.Request.Path.StartsWithSegments("/api/Attacks"),
    appBuilder =>
    {
        appBuilder.UseMiddleware<AttackLoginMiddelware>();
    }
);

app.MapControllers();

app.Run();
