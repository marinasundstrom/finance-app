using Microsoft.AspNetCore.SignalR;

namespace Payments.Hubs;

public class PaymentsHub : Hub<IPaymentsHubClient>
{

}
