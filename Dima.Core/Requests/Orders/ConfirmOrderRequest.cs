using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dima.Core.Requests.Orders
{
    public class ConfirmOrderRequest : BaseRequest
    {
        public string Number { get; set; } = string.Empty;
    }
}
