using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Response.Checkout
{
    public class CheckoutResponse:BaseResponse
    {
        //if Payment method= visa :get values from stripe | else: null
        public string? Url { get; set; }
        public string? PaymentId { get; set; }
    }
} 
