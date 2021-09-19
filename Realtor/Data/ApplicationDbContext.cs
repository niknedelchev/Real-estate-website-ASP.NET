using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Realtor.Models;
using Realtor.Areas.Identity.Pages.Data;

namespace Realtor.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Realtor.Models.Property> Property { get; set; }
        public DbSet<Realtor.Models.City> City { get; set; }
        public DbSet<Realtor.Models.Image> Image { get; set; }

     
    }
}
