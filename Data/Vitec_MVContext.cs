using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Vitec_MV.Models
{
    public class Vitec_MVContext : DbContext
    {
        public Vitec_MVContext (DbContextOptions<Vitec_MVContext> options)
            : base(options)
        {
        }

        public DbSet<Vitec_MV.Models.Product> Product { get; set; }
    }
}
