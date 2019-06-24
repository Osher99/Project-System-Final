using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerSideJWT.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerSideJWT.Data
{
    public class AuthContext : IdentityDbContext
    {
        public AuthContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Duty> Duties { get; set; }

    }
}
