using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InforceTestTask.Migrations
{
    /// <inheritdoc />
    public partial class RenamedColumnContentAtUrlContentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Content",
                table: "UrlContents",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "UrlContents",
                newName: "Content");
        }
    }
}
