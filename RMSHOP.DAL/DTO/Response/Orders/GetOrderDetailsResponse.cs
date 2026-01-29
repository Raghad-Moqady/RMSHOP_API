using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.DAL.Models;
using RMSHOP.DAL.Models.order;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Response.Orders
{
    public class GetOrderDetailsResponse
    {
        public int Id { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatusEnum OrderStatus { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum PaymentStatus { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public List<OrderItemResponse> OrderItems { get; set; }
       

    }
}
