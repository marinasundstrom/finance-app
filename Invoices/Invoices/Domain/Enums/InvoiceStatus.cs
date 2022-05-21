namespace Invoices.Domain.Enums;

//[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum InvoiceStatus
{
    Created,
    Sent,
    Paid,
    PartiallyPaid,
    Overpaid,
    Cancelled
}