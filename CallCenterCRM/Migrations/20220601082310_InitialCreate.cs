using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CallCenterCRM.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "application");

            migrationBuilder.DropTable(
                name: "classification");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "applicants");

            migrationBuilder.DropTable(
                name: "citydistrict");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "direction");
            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    HashName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    OriginName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Extension = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attachments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "citydistrict",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_citydistrict", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "classification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DirectionId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classification", x => x.Id);
                    table.ForeignKey(
                        name: "Classification_fk0",
                        column: x => x.DirectionId,
                        principalTable: "direction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdentityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    Contact = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Surname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Firstname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Middlename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    PassportData = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    ModeratorId = table.Column<int>(type: "integer", nullable: true),
                    DirectionId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                    table.ForeignKey(
                        name: "Users_fk0",
                        column: x => x.ModeratorId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Users_fk2",
                        column: x => x.DirectionId,
                        principalTable: "direction",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "applicants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReferenceSource = table.Column<int>(type: "integer", nullable: false),
                    Surname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Firstname = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Middlename = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Contact = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ExtraContact = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Region = table.Column<int>(type: "integer", nullable: false),
                    CityDistrictId = table.Column<int>(type: "integer", nullable: true),
                    Maxalla = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Gender = table.Column<int>(type: "integer", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValue: new DateTime(2022, 6, 1, 8, 23, 10, 816, DateTimeKind.Utc).AddTicks(7007)),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Employment = table.Column<int>(type: "integer", nullable: false),
                    Confidentiality = table.Column<bool>(type: "boolean", nullable: false),
                    OrganizationId = table.Column<int>(type: "integer", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_applicants", x => x.Id);
                    table.ForeignKey(
                        name: "Applicants_fk0",
                        column: x => x.CityDistrictId,
                        principalTable: "citydistrict",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Applicants_fk1",
                        column: x => x.OrganizationId,
                        principalTable: "users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "application",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClassificationId = table.Column<int>(type: "integer", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    IsSelected = table.Column<bool>(type: "boolean", nullable: false),
                    RecipientId = table.Column<int>(type: "integer", nullable: false),
                    AttachmentId = table.Column<int>(type: "integer", nullable: true),
                    ApplicantId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    MeaningOfApplication = table.Column<string>(type: "text", nullable: false),
                    AdditionalNote = table.Column<string>(type: "text", nullable: true),
                    IsChanged = table.Column<bool>(type: "boolean", nullable: false),
                    IsDelayed = table.Column<bool>(type: "boolean", nullable: false),
                    IsGot = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application", x => x.Id);
                    table.ForeignKey(
                        name: "Application_fk0",
                        column: x => x.ClassificationId,
                        principalTable: "classification",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Application_fk1",
                        column: x => x.RecipientId,
                        principalTable: "users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Application_fk2",
                        column: x => x.AttachmentId,
                        principalTable: "attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Application_fk3",
                        column: x => x.ApplicantId,
                        principalTable: "applicants",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ResponsiblePerson = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Executor = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    AttachmentId = table.Column<int>(type: "integer", nullable: true),
                    RegisterNumber = table.Column<string>(type: "text", nullable: false),
                    Result = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Conclusion = table.Column<string>(type: "text", nullable: false),
                    AuthorId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false),
                    IsGot = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    UpdatedDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.Id);
                    table.ForeignKey(
                        name: "Answers_fk0",
                        column: x => x.AttachmentId,
                        principalTable: "attachments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "Answers_fk2",
                        column: x => x.ApplicationId,
                        principalTable: "application",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_answers_users_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Answers_fk1",
                table: "answers",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "ApplicationId",
                table: "answers",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "AttachmentId",
                table: "answers",
                column: "AttachmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Applicants_fk0",
                table: "applicants",
                column: "CityDistrictId");

            migrationBuilder.CreateIndex(
                name: "Applicants_fk1",
                table: "applicants",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "Application_fk0",
                table: "application",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "Application_fk1",
                table: "application",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "Application_fk3",
                table: "application",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "AttachmentId1",
                table: "application",
                column: "AttachmentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Classification_fk0",
                table: "classification",
                column: "DirectionId");

            migrationBuilder.CreateIndex(
                name: "Email",
                table: "users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Username",
                table: "users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Users_fk0",
                table: "users",
                column: "ModeratorId");

            migrationBuilder.CreateIndex(
                name: "Users_fk2",
                table: "users",
                column: "DirectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "application");

            migrationBuilder.DropTable(
                name: "classification");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "applicants");

            migrationBuilder.DropTable(
                name: "citydistrict");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "direction");
        }
    }
}
