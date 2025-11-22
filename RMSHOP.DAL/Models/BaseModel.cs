using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Models
{
    public class BaseModel
    {
        public int Id { get; set; }
        public Status Status { get; set; } = Status.Active;
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    }
}
