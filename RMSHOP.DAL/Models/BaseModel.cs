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

        //audit
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get;set; }
    }
}
