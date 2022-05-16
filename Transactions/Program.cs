using Microsoft.EntityFrameworkCore;

using MassTransit;

using Transactions.Data;
using Transactions;
using Transactions.Contracts;
using MassTransit.MessageData;
using Transactions.Queries;
using MediatR;
using Transactions.Commands;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Accounting API";
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

    x.AddRequestClient<TransactionBatch>();

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

app.MapPost("/transactions", async (TransactionDto[] transactions, IMediator mediator)
    => await mediator.Send(new PostTransactions(transactions)))
    .WithName("Transactions_PostTransactions")
    .WithTags("Transactions")
    //.RequireAuthorization()
    .Produces(StatusCodes.Status200OK);;

app.Run();