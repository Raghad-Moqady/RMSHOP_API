using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Models
{
    [PrimaryKey(nameof(ImageName) ,nameof(ProductId))]
    public class ProductSubImage
    {
        public string ImageName { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
