using Documents.Client;

using Invoices.Application;
using Invoices.Application.Common.Interfaces;
using Invoices.Application.Queries;
using Invoices.Commands;
using Invoices.Domain.Enums;
using Invoices.Infrastructure;
using Invoices.Infrastructure.Persistence;
using Invoices.Queries;
using Invoices.Services;

using MassTransit;
using MassTransit.MessageData;

using MediatR;

using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services
    .AddApplication()
    .AddInfrastructure(Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocument(c =>
{
    c.Title = "Invoices API";
    c.Version = "0.1";
});

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

builder.Services.AddDocumentsClients((sp, http) =>
{
    http.BaseAddress = new Uri($"{Configuration.GetServiceUri("nginx")}/documents/");
});

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

app.MapGet("/invoices", async (int page, int pageSize, IMediator mediator)
    => await mediator.Send(new GetInvoices(page, pageSize)))
    .WithName("Invoices_GetInvoices")
    .WithTags("Invoices")
    .Produces<ItemsResult<InvoiceDto>>(StatusCodes.Status200OK);

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