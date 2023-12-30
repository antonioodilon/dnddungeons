using Microsoft.EntityFrameworkCore;
using DndDungeons.API.Models.Domain;
using static System.Net.WebRequestMethods;

namespace DndDungeons.API.Data
{
    public class DndDungeonsDbContext : DbContext
    {
        public DndDungeonsDbContext(DbContextOptions<DndDungeonsDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<DndRegion> Regions { get; set; }
        public DbSet<Dungeon> Dungeons { get; set; }
        public DbSet<Image> Images { get; set; }

        // Seeding some data into the database using Entity Framework Core:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Easy, Medium, Hard
            Difficulty easyDifficulty = new Difficulty()
            {
                Id = Guid.Parse("56f83232-0bf4-447d-b7c6-2bf5a4c818a6"),
                Name = "Easy",
            };
            Difficulty mediumDifficulty = new Difficulty()
            {
                Id = Guid.Parse("29f006ee-4961-4de1-867d-0797eb080e91"),
                Name = "Medium",
            };
            Difficulty hardDifficulty = new Difficulty()
            {
                Id = Guid.Parse("f28c19c1-ef2b-48f5-8741-e259080d0640"),
                Name = "Hard",
            };

            List<Difficulty> difficultiesList = new List<Difficulty>();
            difficultiesList.Add(easyDifficulty);
            difficultiesList.Add(mediumDifficulty);
            difficultiesList.Add(hardDifficulty);

            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficultiesList);

            // Seed data for Regions
            DndRegion menzoberranzan = new DndRegion()
            {
                Id = Guid.Parse("9b5aaaca-4280-4fb0-a72a-2a2e460f2c79"),
                Name = "Menzoberranzan",
                Code = "UND",
                RegionImageUrl = "https://db4sgowjqfwig.cloudfront.net/campaigns/97510/assets/1064433/The_City_of_Menzoberranzan.jpg?1586500934",
            };
            DndRegion baldursGate = new DndRegion()
            {
                Id = Guid.Parse("ed8a75d6-28c2-409d-b77a-47a4fa72e94b"),
                Name = "Baldur's Gate",
                Code = "BG",
                RegionImageUrl = "https://static.wikia.nocookie.net/forgottenrealms/images/c/c4/Baldur%27s_Gate_overview_BG3.png/revision/latest?cb=20190606171350",
            };
            DndRegion calimport = new DndRegion()
            {
                Id = Guid.Parse("a836c716-acdb-4961-b141-75f66b4703cc"),
                Name = "Calimport",
                Code = "CLP",
                RegionImageUrl = "https://db4sgowjqfwig.cloudfront.net/campaigns/230022/assets/1289938/Calimport.webp?1667532906",
            };
            DndRegion eltabbar = new DndRegion()
            {
                Id = Guid.Parse("54e9293e-b843-457d-9be3-8cb95d5e86ed"),
                Name = "Eltabbar",
                Code = "ETB",
                RegionImageUrl = "https://preview.redd.it/the-city-of-eltabbar-capital-city-of-thay-at-night-with-a-v0-46wdg9cm83fa1.png?auto=webp&s=28d424c1c7e6854d87f9d7e22e652d75b6217a2a",
            };
            DndRegion athkatla = new DndRegion()
            {
                Id = Guid.Parse("83b06436-c95c-400f-a8df-03037f407b42"),
                Name = "Athkatla",
                Code = "ATK",
                RegionImageUrl = "https://www.worldanvil.com/media/cache/cover/uploads/images/ac86f132ef5cffd6486891449e00a89e.webp",
            };


            List<DndRegion> regionsList = new List<DndRegion>();
            regionsList.Add(menzoberranzan);
            regionsList.Add(baldursGate);
            regionsList.Add(calimport);
            regionsList.Add(eltabbar);
            regionsList.Add(athkatla);

            // Seed regions data do the database:
            modelBuilder.Entity<DndRegion>().HasData(regionsList);
        }
    }
}
