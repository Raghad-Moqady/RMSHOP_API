using RMSHOP.DAL.DTO.Response.Products;
using RMSHOP.DAL.Models.order;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Response.Orders
{
    public class OrderItemResponse
    {  
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public List<ProductTranslationResponse> Translations { get; set; }
    }
}
