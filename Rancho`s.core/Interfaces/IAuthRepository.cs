using Rancho_s.core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Interfaces
{
    public interface IAuthRepository
    {
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<AppUser?> GetUserByIdAsync(int id);
        Task<bool> EmailExistsAsync(string email);

    }
}
