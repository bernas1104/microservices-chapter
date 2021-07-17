using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Users.Infra.Migrations
{
    public partial class UsersMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    fullName = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "varchar", maxLength: 100, nullable: false),
                    createdAt = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValue: new DateTime(2021, 7, 17, 18, 38, 52, 255, DateTimeKind.Local).AddTicks(4602)),
                    updatedAt = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValue: new DateTime(2021, 7, 17, 18, 38, 52, 262, DateTimeKind.Local).AddTicks(8785)),
                    deletedAt = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
