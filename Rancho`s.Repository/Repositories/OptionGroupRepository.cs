using Rancho_s.core.Entities;
using Rancho_s.core.Interfaces;
using Rancho_s.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.Repository.Repositories
{
    public class OptionGroupRepository : GenericRepository<OptionGroup>, IOptionGroupRepository
    {

        public OptionGroupRepository(Rancho_sDbContext _Context):base(_Context)
        {
            
        }
        public Task<IReadOnlyList<OptionGroup>> GetByProductIdAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<OptionGroup?> GetWithOptionsAsync(int groupId)
        {
            throw new NotImplementedException();
        }
    }
}
