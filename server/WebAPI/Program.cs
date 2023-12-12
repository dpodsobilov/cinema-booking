using Data;
using Logic;
using Logic.Hubs;
using WebAPI.Extensions;
using WebAPI.Middlewares;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LogicPointer>());
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigins",
        policyBuilder =>
        {
            policyBuilder
                .AllowCredentials()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .WithOrigins(
                    "http://localhost:4200"
                );
        });
});

builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.ConfigureExceptionHandler();

app.UseCors("AllowMyOrigins");

app.UseMiddleware<AuthMiddleware>();

app.MapHub<PlacesHub>("/Places");

app.Run();