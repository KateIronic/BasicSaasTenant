using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicSaasTenent.Migrations
{
    /// <inheritdoc />
    public partial class InitialAuth4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "LoginModel",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "LoginModel");
        }
    }
}
