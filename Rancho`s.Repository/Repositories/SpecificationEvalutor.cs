using Microsoft.EntityFrameworkCore;
using Rancho_s.core.Entities;
using Rancho_s.core.Specfction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Repository.Repositories
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, Ispecification<T> specification) 
        {
            var query = inputQuery;

            if(specification.Criteria != null)
            {
                query = query.Where(specification.Criteria);
            }

            query = specification.Includes.Aggregate(query, (current, include) => current.Include(include));

            return query;


        }


    }
}
