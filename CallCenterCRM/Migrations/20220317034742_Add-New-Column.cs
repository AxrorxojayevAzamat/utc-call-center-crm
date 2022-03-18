using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddNewColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RelevantApplications",
                table: "application",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "application",
                type: "text",
                nullable: true,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "application",
                type: "text",
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
                oldDefaultValue: new DateTime(2022, 3, 8, 18, 23, 28, 591, DateTimeKind.Local).AddTicks(1642));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reason",
                table: "application");

            migrationBuilder.UpdateData(
                table: "application",
                keyColumn: "RelevantApplications",
                keyValue: null,
                column: "RelevantApplications",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "RelevantApplications",
                table: "application",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.UpdateData(
                table: "application",
                keyColumn: "Comment",
                keyValue: null,
                column: "Comment",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "application",
                type: "text",
                nullable: false,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 3, 8, 18, 23, 28, 591, DateTimeKind.Local).AddTicks(1642),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 3, 17, 8, 47, 42, 754, DateTimeKind.Local).AddTicks(5565));
        }
    }
}
