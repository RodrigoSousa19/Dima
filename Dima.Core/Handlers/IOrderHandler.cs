using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Handlers
{
    public interface IOrderHandler
    {
        Task<Response<Order?>> CreateOrderAsync(CreateOrderRequest request);
        Task<Response<Order?>> ConfirmOrderAsync(ConfirmOrderRequest request);
    }
}
