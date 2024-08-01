using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Orders
{
    public class ConfirmOrderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPut("/", HandleAsync)
            .WithName("Orders: Update")
            .WithSummary("Confirma um pedido")
            .WithDescription("Confirma um pedido")
            .WithOrder(1)
            .Produces<Response<Order?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IOrderHandler handler, ConfirmOrderRequest request)
        {
            request.UserId = user.Identity?.Name ?? string.Empty;

            var result = await handler.ConfirmOrderAsync(request);

            return result.IsSuccess
                ? TypedResults.Created("", result)
                : TypedResults.BadRequest(result.Data);
        }
    }
}
