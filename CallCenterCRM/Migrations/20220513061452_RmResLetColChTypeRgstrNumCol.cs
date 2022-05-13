using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class RmResLetColChTypeRgstrNumCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseLetter",
                table: "answers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 13, 6, 14, 52, 804, DateTimeKind.Utc).AddTicks(7749),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 9, 10, 25, 33, 459, DateTimeKind.Utc).AddTicks(904));

            migrationBuilder.AlterColumn<string>(
                name: "RegisterNumber",
                table: "answers",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 9, 10, 25, 33, 459, DateTimeKind.Utc).AddTicks(904),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 13, 6, 14, 52, 804, DateTimeKind.Utc).AddTicks(7749));

            migrationBuilder.AlterColumn<int>(
                name: "RegisterNumber",
                table: "answers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ResponseLetter",
                table: "answers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
