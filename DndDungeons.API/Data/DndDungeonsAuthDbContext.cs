using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DndDungeons.API.Data
{
    public class DndDungeonsAuthDbContext: IdentityDbContext
    {
        public DndDungeonsAuthDbContext(DbContextOptions<DndDungeonsAuthDbContext> options) : base(options)
        {       
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            string readerRoleId = "f14e9af6-d9b6-4852-b43f-1985778aff7f";
            string writerRoleId = "e3dcfd46-79b8-4c2d-993c-c06209f684fa";

            List<IdentityRole> listOfRoles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id = writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                },
            };

            builder.Entity<IdentityRole>().HasData(listOfRoles);
        }
    }
}
