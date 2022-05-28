using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class AddDirectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Users_fk1",
                table: "users");

            migrationBuilder.DropColumn(
                name: "Direction",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "classification");

            migrationBuilder.RenameColumn(
                name: "ClassificationId",
                table: "users",
                newName: "DirectionId");

            migrationBuilder.RenameIndex(
                name: "Users_fk1",
                table: "users",
                newName: "Users_fk2");

            migrationBuilder.AddColumn<int>(
                name: "DirectionId",
                table: "classification",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 27, 14, 25, 36, 7, DateTimeKind.Utc).AddTicks(2980),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 13, 6, 14, 52, 804, DateTimeKind.Utc).AddTicks(7749));

            migrationBuilder.CreateTable(
                name: "direction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Consequence = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_direction", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "Classification_fk0",
                table: "classification",
                column: "DirectionId");

            migrationBuilder.AddForeignKey(
                name: "Classification_fk0",
                table: "classification",
                column: "DirectionId",
                principalTable: "direction",
                principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "Users_fk2",
            //    table: "users",
            //    column: "DirectionId",
            //    principalTable: "direction",
            //    principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Classification_fk0",
                table: "classification");

            migrationBuilder.DropForeignKey(
                name: "Users_fk2",
                table: "users");

            migrationBuilder.DropTable(
                name: "direction");

            migrationBuilder.DropIndex(
                name: "Classification_fk0",
                table: "classification");

            migrationBuilder.DropColumn(
                name: "DirectionId",
                table: "classification");

            migrationBuilder.RenameColumn(
                name: "DirectionId",
                table: "users",
                newName: "ClassificationId");

            migrationBuilder.RenameIndex(
                name: "Users_fk2",
                table: "users",
                newName: "Users_fk1");

            migrationBuilder.AddColumn<string>(
                name: "Direction",
                table: "classification",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "classification",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "applicants",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(2022, 5, 13, 6, 14, 52, 804, DateTimeKind.Utc).AddTicks(7749),
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldDefaultValue: new DateTime(2022, 5, 27, 14, 25, 36, 7, DateTimeKind.Utc).AddTicks(2980));

            migrationBuilder.AddForeignKey(
                name: "Users_fk1",
                table: "users",
                column: "ClassificationId",
                principalTable: "classification",
                principalColumn: "Id");
        }
    }
}
