using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddAppNumColumntoApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppNum",
                table: "application",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(

                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 10, 9, 51, 6, 902, DateTimeKind.Utc).AddTicks(4518),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 8, 11, 13, 23, 143, DateTimeKind.Utc).AddTicks(528));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppNum",
                table: "application");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 8, 11, 13, 23, 143, DateTimeKind.Utc).AddTicks(528),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 10, 9, 51, 6, 902, DateTimeKind.Utc).AddTicks(4518));
        }
    }
}
