using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Request.Checkout
{
    public class CheckoutRequest
    {
        //لان وصلنا لهذا الزر من صفحة السلة ، اذاً انا متأكدة مية بالمية انه اليوزر مسجل دخوله وبقدر اوصل لمعلوماته من التوكن
        //لذلك ما بطلب منه اي معلومات خاصة باليوزر
        public string PaymentMethod { get; set; }
    }
}
