using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class CrmUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Crm",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Crm",
                table: "AspNetUsers");
        }
    }
}
