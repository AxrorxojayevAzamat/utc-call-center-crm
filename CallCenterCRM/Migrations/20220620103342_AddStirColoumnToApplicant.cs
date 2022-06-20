using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddStirColoumnToApplicant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 20, 10, 33, 42, 878, DateTimeKind.Utc).AddTicks(4797),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 20, 6, 48, 7, 481, DateTimeKind.Utc).AddTicks(2602));

            migrationBuilder.AddColumn<string>(
                name: "Stir",
                table: "applicants",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stir",
                table: "applicants");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 6, 20, 6, 48, 7, 481, DateTimeKind.Utc).AddTicks(2602),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 6, 20, 10, 33, 42, 878, DateTimeKind.Utc).AddTicks(4797));
        }
    }
}
