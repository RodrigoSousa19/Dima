﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Requests.Transactions;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Orders
{
    public class CreateOrderEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app)
        => app.MapPost("/", HandleAsync)
            .WithName("Orders: Create")
            .WithSummary("Cria um novo pedido")
            .WithDescription("Cria um novo pedido")
            .WithOrder(1)
            .Produces<Response<Order?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IOrderHandler handler, CreateOrderRequest request)
        {
            request.UserId = user.Identity?.Name ?? string.Empty;

            var result = await handler.CreateOrderAsync(request);

            return result.IsSuccess
                ? TypedResults.Created("", result)
                : TypedResults.BadRequest(result.Data);
        }
    }
}
