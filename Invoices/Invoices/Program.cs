using Invoices;
using Invoices.Commands;
using Invoices.Contracts;
using Invoices.Data;
using Invoices.Models;
using Invoices.Queries;

using MassTransit;
using MassTransit.MessageData;

using MediatR;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Invoices API";
    c.Version = "0.1";
});

const string ConnectionStringKey = "mssql";

var connectionString = Configuration.GetConnectionString(ConnectionStringKey, "Invoices");

builder.Services.AddDbContext<InvoicesContext>((sp, options) =>
{
    options.UseSqlServer(connectionString, o => o.EnableRetryOnFailure());
#if DEBUG
    options.EnableSensitiveDataLogging();
#endif
});

builder.Services.AddScoped<IInvoicesContext>(sp => sp.GetRequiredService<InvoicesContext>());

builder.Services.AddMediatR(typeof(Program));

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);

    //x.AddRequestClient<TransactionBatch>();

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

app.MapGet("/invoices", async (IMediator mediator)
    => await mediator.Send(new GetInvoices()))
    .WithName("Invoices_GetInvoices")
    .WithTags("Invoices")
    .Produces<IEnumerable<InvoiceDto>>(StatusCodes.Status200OK);

app.MapGet("/invoices/{invoiceId}", async (int invoiceId, IMediator mediator)
    => await mediator.Send(new GetInvoice(invoiceId)))
    .WithName("Invoices_GetInvoice")
    .WithTags("Invoices")
    .Produces<InvoiceDto>(StatusCodes.Status200OK);

app.MapPost("/invoices", async (CreateInvoice command, IMediator mediator)
    => await mediator.Send(command))
    .WithName("Invoices_CreateInvoice")
    .WithTags("Invoices")
    .Produces<InvoiceDto>(StatusCodes.Status200OK);

app.MapPut("/invoices/{invoiceId}/status", async (int invoiceId, InvoiceStatus status, IMediator mediator)
    => await mediator.Send(new SetInvoiceStatus(invoiceId, status)))
    .WithName("Invoices_SetInvoiceStatus")
    .WithTags("Invoices")
    .Produces(StatusCodes.Status200OK);


app.MapPut("/invoices/{invoiceId}/paid", async (int invoiceId, decimal amount, IMediator mediator)
    => await mediator.Send(new SetPaidAmount(invoiceId, amount)))
    .WithName("Invoices_SetPaidAmount")
    .WithTags("Invoices")
    .Produces(StatusCodes.Status200OK);

app.Run();