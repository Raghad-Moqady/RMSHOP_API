using RMSHOP.DAL.Models;
using RMSHOP.DAL.Models.product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Response.Review
{
    public class ReviewResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } 
        public string Comment { get; set; }
        public int Rating { get; set; }
        public string CreatedAt { get; set; }
    }
}
