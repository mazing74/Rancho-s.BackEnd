using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rancho_s.core.Entities
{
    public class Role : BaseEntity
    {

        // first has name from BaseEntity         // "Admin", "Customer", "KitchenStaff"

        // Navigation — one role has many users
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
