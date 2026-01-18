using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Models.order
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        Cancelled=2,
        Approved=3,
        Shipped=4,
        Delivered =5
    }
}
