using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class Replacecoloumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelevantApplications",
                table: "application");

            migrationBuilder.DropColumn(
                name: "AdditionalNote",
                table: "applicants");

            migrationBuilder.DropColumn(
                name: "MeaningOfApplication",
                table: "applicants");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNote",
                table: "application",
                type: "text",
                nullable: false,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MeaningOfApplication",
                table: "application",
                type: "text",
                nullable: false,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 3, 17, 33, 1, 418, DateTimeKind.Local).AddTicks(6648),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 3, 17, 8, 47, 42, 754, DateTimeKind.Local).AddTicks(5565));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalNote",
                table: "application");

            migrationBuilder.DropColumn(
                name: "MeaningOfApplication",
                table: "application");

            migrationBuilder.AddColumn<string>(
                name: "RelevantApplications",
                table: "application",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 17, 8, 47, 42, 754, DateTimeKind.Local).AddTicks(5565),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 4, 3, 17, 33, 1, 418, DateTimeKind.Local).AddTicks(6648));

            migrationBuilder.AddColumn<string>(
                name: "AdditionalNote",
                table: "applicants",
                type: "text",
                nullable: false,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MeaningOfApplication",
                table: "applicants",
                type: "text",
                nullable: false,
                collation: "utf8mb4_unicode_ci")
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
