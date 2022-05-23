using System.Text.Json.Serialization;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Http.Json;

using Transactions.Application;
using Transactions.Application.Commands;
using Transactions.Application.Common.Interfaces;
using Transactions.Application.Queries;
using Transactions.Application.Services;
using Transactions.Domain.Enums;
using Transactions.Hubs;
using Transactions.Infrastructure;
using Transactions.Infrastructure.Persistence;
using Transactions.Services;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services
    .AddApplication()
    .AddInfrastructure(Configuration);

builder.Services.AddScoped<ITransactionsHubClient, TransactionsHubClient>();

builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    // options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Transactions API";
    c.Version = "0.1";
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);

    x.AddRequestClient<Transactions.Contracts.TransactionBatch>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
    });
})
.AddMassTransitHostedService(true)
.AddGenericRequestClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//await SeedData.EnsureSeedData(app);

app.MapGet("/", () => "Hello World!");

app.MapGet("/transactions", async (int page, int pageSize, IMediator mediator) => await mediator.Send(new GetTransactons(page, pageSize)))
    .WithName("Transactions_GetTransactions")
    .WithTags("Transactions")
    //.RequireAuthorization()
    .Produces<ItemsResult<TransactionDto>>(StatusCodes.Status200OK); ;

app.MapPost("/transactions", async (TransactionDto[] transactions, IMediator mediator)
    => await mediator.Send(new PostTransactions(transactions)))
    .WithName("Transactions_PostTransactions")
    .WithTags("Transactions")
    //.RequireAuthorization()
    .Produces(StatusCodes.Status200OK); ;

app.MapPut("/transactions/{transactionId}/status", async (string transactionId, TransactionStatus status, IMediator mediator)
    => await mediator.Send(new SetTransactionStatus(transactionId, status)))
    .WithName("Transactions_SetTransactionStatus")
    .WithTags("Transactions")
    .Produces(StatusCodes.Status200OK);

app.MapHub<TransactionsHub>("/hubs/transactions");

app.Run();