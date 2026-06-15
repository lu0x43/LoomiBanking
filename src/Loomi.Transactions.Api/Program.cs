using Loomi.Transactions.Application.Interfaces;
using Loomi.Transactions.Infrastructure.Data;
using Loomi.Transactions.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TransactionsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .Or<Polly.Timeout.TimeoutRejectedException>()
    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

var circuitBreakerPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .Or<Polly.Timeout.TimeoutRejectedException>()
    .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));

var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(3));

builder.Services.AddHttpClient<IClientApiService, ClientApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7001");
})
.AddPolicyHandler(retryPolicy)
.AddPolicyHandler(circuitBreakerPolicy)
.AddPolicyHandler(timeoutPolicy);

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConfig = builder.Configuration.GetSection("RabbitMq");
        cfg.Host(rabbitMqConfig["Host"] ?? "localhost", "/", h =>
        {
            h.Username(rabbitMqConfig["Username"] ?? "guest");
            h.Password(rabbitMqConfig["Password"] ?? "guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();