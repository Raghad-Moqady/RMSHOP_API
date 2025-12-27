using RMSHOP.DAL.DTO.Request;
using RMSHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSHOP.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public Status Status { get; set; }
        //way1 to return some info about user who create this category
        //public ApplicationUserResponse User { get; set; }

        //way2 using mapster config
        public string CreatedByUserName { get; set; }
        
        public List<CategoryTranslationResponse> Translations { get; set; }

    }
}
