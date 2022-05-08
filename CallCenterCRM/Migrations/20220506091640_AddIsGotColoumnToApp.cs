using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddIsGotColoumnToApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpireTime",
                table: "application",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddColumn<bool>(
                name: "IsGot",
                table: "application",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 6, 9, 16, 39, 991, DateTimeKind.Utc).AddTicks(4465),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2022, 4, 11, 12, 18, 4, 613, DateTimeKind.Utc).AddTicks(7616));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGot",
                table: "application");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpireTime",
                table: "application",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 11, 12, 18, 4, 613, DateTimeKind.Utc).AddTicks(7616),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 6, 9, 16, 39, 991, DateTimeKind.Utc).AddTicks(4465));
        }
    }
}
