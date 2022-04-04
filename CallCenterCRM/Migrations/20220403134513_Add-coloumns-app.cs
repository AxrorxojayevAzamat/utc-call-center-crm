using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class Addcoloumnsapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsChanged",
                table: "application",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelayed",
                table: "application",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 3, 18, 45, 13, 145, DateTimeKind.Local).AddTicks(6957),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 4, 3, 17, 33, 1, 418, DateTimeKind.Local).AddTicks(6648));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsChanged",
                table: "application");

            migrationBuilder.DropColumn(
                name: "IsDelayed",
                table: "application");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 3, 17, 33, 1, 418, DateTimeKind.Local).AddTicks(6648),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 4, 3, 18, 45, 13, 145, DateTimeKind.Local).AddTicks(6957));
        }
    }
}
