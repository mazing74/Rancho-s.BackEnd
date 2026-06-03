using Microsoft.EntityFrameworkCore;
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
    public class AuthRepository : IAuthRepository
    {
        private readonly Rancho_sDbContext _dbContext;

        public AuthRepository(Rancho_sDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Email!.ToLower() == email.ToLower());
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
              return await _dbContext.Users
                .FirstOrDefaultAsync(u =>
                    u.Email!.ToLower() == email.ToLower() && u.IsActive);
        }

        public async Task<AppUser?> GetUserByIdAsync(int id)
        {
            return await _dbContext.Users
                         .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
        }
    }
}
