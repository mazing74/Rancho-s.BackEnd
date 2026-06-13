using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Specfction
{
    public interface Ispecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get;  } // related to where clause in sql
        public List<Expression<Func<T, object>>> Includes { get; } // related to include in sql


    }
}
