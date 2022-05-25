using System.Text.Json.Serialization;

using MassTransit;

using MediatR;

using Microsoft.AspNetCore.Http.Json;

using Payments.Application;
using Payments.Application.Commands;
using Payments.Application.Common.Interfaces;
using Payments.Application.Queries;
using Payments.Application.Services;
using Payments.Domain.Enums;
using Payments.Hubs;
using Payments.Infrastructure;
using Payments.Infrastructure.Persistence;
using Payments.Services;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

builder.Services
    .AddApplication()
    .AddInfrastructure(Configuration);

builder.Services.AddControllers();

// Set the JSON serializer options
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options =>
{
    // options.SerializerOptions.WriteIndented = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddScoped<IPaymentsHubClient, PaymentsHubClient>();

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
    c.Title = "Payments API";
    c.Version = "0.1";
});

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    //x.AddConsumers(typeof(Program).Assembly);

    x.AddRequestClient<Payments.Contracts.PaymentBatch>();

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

app.MapGet("/", () => "Hello World!");

/*
app.MapGet("/payments", async (int page, int pageSize, IMediator mediator) => await mediator.Send(new GetPayments(page, pageSize)))
    .WithName("Payments_GetPayments")
    .WithTags("Payments")
    //.RequireAuthorization()
    .Produces<ItemsResult<PaymentDto>>(StatusCodes.Status200OK);
*/

app.MapPost("/payments", async (PaymentDto[] payments, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(new PostPayments(payments), cancellationToken))
    .WithName("Payments_PostPayments")
    .WithTags("Payments")
    //.RequireAuthorization()
    .Produces(StatusCodes.Status200OK); ;

app.MapPut("/payments/{paymentId}/status", async (string paymentId, PaymentStatus status, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(new SetPaymentStatus(paymentId, status), cancellationToken))
    .WithName("Payments_SetPaymentStatus")
    .WithTags("Payments")
    .Produces(StatusCodes.Status200OK);

app.MapPut("/payments/{paymentId}/reference", async (string paymentId, string reference, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(new UpdatePaymentReference(paymentId, reference), cancellationToken))
    .WithName("Payments_SetPaymentReference")
    .WithTags("Payments")
    .Produces(StatusCodes.Status200OK);

app.MapPut("/payments/{paymentId}/invoiceId", async (string paymentId, int invoiceId, IMediator mediator, CancellationToken cancellationToken)
    => await mediator.Send(new SetPaymentInvoiceId(paymentId, invoiceId), cancellationToken))
    .WithName("Payments_SetPaymentInvoiceId")
    .WithTags("Payments")
    .Produces(StatusCodes.Status200OK);

app.MapHub<PaymentsHub>("/hubs/payments");

app.MapControllers();

//await SeedData.EnsureSeedData(app);

app.Run();