using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Specfction
{
    public class Basespecification<T> : Ispecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; }
                = new List<Expression<Func<T, object>>>();


        public Basespecification()
        {
                
        }

        public Basespecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
    }
}
