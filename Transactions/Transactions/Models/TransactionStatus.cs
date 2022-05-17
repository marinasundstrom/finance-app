namespace Transactions.Models;

[Newtonsoft.Json.JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
public enum TransactionStatus
{
    Unverified,
    Verified,
    Unknown,
}