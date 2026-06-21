using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Specfction
{
    public class ProductWithCategorySpec : Basespecification<Product>
    {

        public ProductWithCategorySpec() 
        {
           Includes.Add(p => p.Category);
        }

        public ProductWithCategorySpec(int id ) :base(p=>p.Id==id) 
        {
            Includes.Add(p => p.Category);
        }
    }
}
