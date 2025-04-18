using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomusFix.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDraftFileName1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Drafts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Drafts");
        }
    }
}
