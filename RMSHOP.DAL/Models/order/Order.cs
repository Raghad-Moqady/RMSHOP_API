using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Models.order
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }= OrderStatusEnum.Pending; 

        public DateTime OrderDate { get; set; }= DateTime.UtcNow;
        public DateTime? ShippedDate {  get; set; }
        public PaymentMethodEnum PaymentMethod { get; set; }

        //from Stripe=> if payment method = visa
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; }
        public decimal? AmountPaid { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; } 
        public List<OrderItem> OrderItems { get; set; }
    }
}
