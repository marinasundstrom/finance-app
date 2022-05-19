using MassTransit;
using MassTransit.MessageData;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Transactions;
using Transactions.Commands;
using Transactions.Models;
using Transactions.Data;
using Transactions.Queries;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Transactions API";
    c.Version = "0.1";
});

const string ConnectionStringKey = "mssql";

var connectionString = Configuration.GetConnectionString(ConnectionStringKey, "Transactions");

builder.Services.AddDbContext<TransactionsContext>((sp, options) =>
{
    options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

builder.Services.AddScoped<ITransactionsContext>(sp => sp.GetRequiredService<TransactionsContext>());

builder.Services.AddMediatR(typeof(Program));

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

//builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddScoped<IRazorTemplateCompiler, RazorTemplateCompiler>();
//builder.Services.AddScoped<IPdfGenerator, PdfGenerator>();

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

await SeedData.EnsureSeedData(app);

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

app.Run();