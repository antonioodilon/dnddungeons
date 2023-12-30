using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DndDungeons.API.Migrations.DndDungeonsDb
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Difficulties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Difficulties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FileSizeInBytes = table.Column<long>(type: "bigint", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegionImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Regions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LengthInKm = table.Column<double>(type: "float", nullable: false),
                    DungeonImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DifficultyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dungeons_Difficulties_DifficultyId",
                        column: x => x.DifficultyId,
                        principalTable: "Difficulties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Dungeons_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("29f006ee-4961-4de1-867d-0797eb080e91"), "Medium" },
                    { new Guid("56f83232-0bf4-447d-b7c6-2bf5a4c818a6"), "Easy" },
                    { new Guid("f28c19c1-ef2b-48f5-8741-e259080d0640"), "Hard" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("54e9293e-b843-457d-9be3-8cb95d5e86ed"), "ETB", "Eltabbar", "https://preview.redd.it/the-city-of-eltabbar-capital-city-of-thay-at-night-with-a-v0-46wdg9cm83fa1.png?auto=webp&s=28d424c1c7e6854d87f9d7e22e652d75b6217a2a" },
                    { new Guid("83b06436-c95c-400f-a8df-03037f407b42"), "ATK", "Athkatla", "https://www.worldanvil.com/media/cache/cover/uploads/images/ac86f132ef5cffd6486891449e00a89e.webp" },
                    { new Guid("9b5aaaca-4280-4fb0-a72a-2a2e460f2c79"), "UND", "Menzoberranzan", "https://db4sgowjqfwig.cloudfront.net/campaigns/97510/assets/1064433/The_City_of_Menzoberranzan.jpg?1586500934" },
                    { new Guid("a836c716-acdb-4961-b141-75f66b4703cc"), "CLP", "Calimport", "https://db4sgowjqfwig.cloudfront.net/campaigns/230022/assets/1289938/Calimport.webp?1667532906" },
                    { new Guid("ed8a75d6-28c2-409d-b77a-47a4fa72e94b"), "BG", "Baldur's Gate", "https://static.wikia.nocookie.net/forgottenrealms/images/c/c4/Baldur%27s_Gate_overview_BG3.png/revision/latest?cb=20190606171350" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_DifficultyId",
                table: "Dungeons",
                column: "DifficultyId");

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_RegionId",
                table: "Dungeons",
                column: "RegionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Difficulties");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
