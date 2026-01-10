using RMSHOP.DAL.Models;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Request.Cart
{
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
        public int Count { get; set; } = 1;
    }
}
