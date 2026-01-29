using RMSHOP.DAL.Models.order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Request.Orders
{
    public class UpdateOrderStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum NewOrderStatus { get; set; } 
    }
}
