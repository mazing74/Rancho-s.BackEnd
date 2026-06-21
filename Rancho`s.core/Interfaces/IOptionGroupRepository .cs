using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Interfaces
{
    public interface IOptionGroupRepository :IGenericRepository<OptionGroup>
    {
        Task<IReadOnlyList<OptionGroup>> GetByProductIdAsync(int productId);
        Task<OptionGroup?> GetWithOptionsAsync(int groupId);

    }
}
