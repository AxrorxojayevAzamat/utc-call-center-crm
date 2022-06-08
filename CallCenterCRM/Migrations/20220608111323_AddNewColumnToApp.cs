using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddNewColumnToApp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorName",
                table: "application",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 8, 11, 13, 23, 143, DateTimeKind.Utc).AddTicks(528),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 1, 8, 23, 10, 816, DateTimeKind.Utc).AddTicks(7007));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "application");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 1, 8, 23, 10, 816, DateTimeKind.Utc).AddTicks(7007),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 8, 11, 13, 23, 143, DateTimeKind.Utc).AddTicks(528));
        }
    }
}
