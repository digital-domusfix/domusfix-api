using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DomusFix.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddWizardConfigWithConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NDAConfigTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ConfigType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NDAConfigTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "NDAConfigTypes",
                columns: new[] { "Id", "ConfigType", "Key", "Value" },
                values: new object[,]
                {
                    { 1, "relationship", "Employer-Employee", null },
                    { 2, "relationship", "Client-Contractor", null },
                    { 3, "relationship", "Seller-Buyer", null },
                    { 4, "relationship", "Other", null },
                    { 5, "jurisdiction", "England", "Select this for contracts under English law." },
                    { 6, "jurisdiction", "Scotland", "Select this for contracts under Scottish law." }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NDAConfigTypes_ConfigType_Key",
                table: "NDAConfigTypes",
                columns: new[] { "ConfigType", "Key" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NDAConfigTypes");
        }
    }
}
