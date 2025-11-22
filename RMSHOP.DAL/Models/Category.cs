using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.Models
{
    public class Category: BaseModel
    {
        
        public List<CategoryTranslation> Translations { get; set; }
    }
}
