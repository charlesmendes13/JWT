using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using JWT.Models;
using JWT.Extensions;

namespace JWT.Data
{
    public class JWTDbContext : IdentityDbContext<User>
    {
        public JWTDbContext(DbContextOptions<JWTDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasData(
                new User
                {
                    UserName = "charles.mendes",
                    PasswordHash = HasherExtension.HashPassword("123"),
                });  
        }
    }
}
