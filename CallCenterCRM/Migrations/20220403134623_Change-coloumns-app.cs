using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class Changecoloumnsapp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNote",
                table: "application",
                type: "text",
                nullable: true,
                collation: "utf8mb4_unicode_ci",
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("Relational:Collation", "utf8mb4_unicode_ci");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2022, 4, 3, 18, 46, 23, 410, DateTimeKind.Local).AddTicks(9516),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 4, 3, 18, 45, 13, 145, DateTimeKind.Local).AddTicks(6957));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "application",
                keyColumn: "AdditionalNote",
                keyValue: null,
                column: "AdditionalNote",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalNote",
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
                defaultValue: new DateTime(2022, 4, 3, 18, 45, 13, 145, DateTimeKind.Local).AddTicks(6957),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2022, 4, 3, 18, 46, 23, 410, DateTimeKind.Local).AddTicks(9516));
        }
    }
}
