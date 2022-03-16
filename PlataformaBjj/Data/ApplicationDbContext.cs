using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlataformaBjj.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlataformaBjj.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
       
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<VideoItem> VideoItems { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<NewsItem> NewsItems { get; set; }
        public DbSet<Phrases> Phrases { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
    }
}
