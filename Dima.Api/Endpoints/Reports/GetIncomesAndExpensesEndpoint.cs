﻿using Dima.Api.Common.Api;
using Dima.Core.Handlers;
using Dima.Core.Reports;
using Dima.Core.Requests.Reports;
using Dima.Core.Responses;
using System.Security.Claims;

namespace Dima.Api.Endpoints.Reports
{
    public class GetIncomesAndExpensesEndpoint : IEndpoint
    {
        public static void Map(IEndpointRouteBuilder app) => app.MapGet("/incomesandexpenses", HandleAsync).Produces<Response<List<IncomesAndExpenses>?>>();

        private static async Task<IResult> HandleAsync(ClaimsPrincipal user, IReportHandler handler)
        {
            var request = new GetIncomesAndExpensesRequest
            {
                UserId = user.Identity?.Name ?? string.Empty
            };

            var result = await handler.GetIncomesAndExpensesReportAsync(request);
            return result.IsSuccess ? TypedResults.Ok(result) : TypedResults.BadRequest(result);
        }
    }
}