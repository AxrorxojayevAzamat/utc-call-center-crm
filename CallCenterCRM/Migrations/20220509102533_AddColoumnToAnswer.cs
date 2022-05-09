using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddColoumnToAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 9, 10, 25, 33, 459, DateTimeKind.Utc).AddTicks(904),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 6, 9, 16, 39, 991, DateTimeKind.Utc).AddTicks(4465));

            migrationBuilder.AddColumn<bool>(
                name: "IsGot",
                table: "answers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGot",
                table: "answers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 6, 9, 16, 39, 991, DateTimeKind.Utc).AddTicks(4465),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 9, 10, 25, 33, 459, DateTimeKind.Utc).AddTicks(904));
        }
    }
}
