using Microsoft.EntityFrameworkCore;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Models.cart
{
    [PrimaryKey(nameof(UserId),nameof(ProductId))]
    public class Cart
    {
        //User => Product (M-M)
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; } = 1;
    }
}
