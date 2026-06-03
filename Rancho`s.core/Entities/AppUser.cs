using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class AppUser : IdentityUser<int>
    {
        // int = we want int IDs not the default string GUIDs
        // Much cleaner for foreign keys in orders, etc.
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt
        {
            get; set;
        }
    }
}
