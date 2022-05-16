using Accounting.Client;

using MassTransit;

using Transactions.Consumers;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);

    x.AddConsumer<TransactionBatchConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
})
.AddMassTransitHostedService(true)
.AddGenericRequestClient();

builder.Services.AddAccountingClients((sp, http) =>
{
    http.BaseAddress = new Uri($"{Configuration.GetServiceUri("nginx")}/api/");
});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

/* app.MapGet("/greetings", (string name) => new GreetingsResponse($"Greetings, {name}!"))
        .WithName("Greetings_Hi")
        .WithTags("Greetings")
        //.RequireAuthorization()
        .Produces<GreetingsResponse>(StatusCodes.Status200OK); */

app.Run();