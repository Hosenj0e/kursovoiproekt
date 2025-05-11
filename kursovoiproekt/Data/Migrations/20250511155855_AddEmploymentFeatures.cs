using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace kursovoiproekt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddEmploymentFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_ApplicantId",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_EmployerId",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_job_ads_JobAdId",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_job_ads_JobAdId1",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_JobAdId1",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "JobAdId1",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Messages",
                newSchema: "kursovoi");

            migrationBuilder.RenameColumn(
                name: "JobAdId",
                schema: "kursovoi",
                table: "Chats",
                newName: "jobadid");

            migrationBuilder.RenameColumn(
                name: "EmployerId",
                schema: "kursovoi",
                table: "Chats",
                newName: "employerid");

            migrationBuilder.RenameColumn(
                name: "ApplicantId",
                schema: "kursovoi",
                table: "Chats",
                newName: "applicantid");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_JobAdId",
                schema: "kursovoi",
                table: "Chats",
                newName: "IX_Chats_jobadid");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_EmployerId",
                schema: "kursovoi",
                table: "Chats",
                newName: "IX_Chats_employerid");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_ApplicantId",
                schema: "kursovoi",
                table: "Chats",
                newName: "IX_Chats_applicantid");

            migrationBuilder.AddColumn<int>(
                name: "ExperienceYears",
                schema: "kursovoi",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                schema: "kursovoi",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserType",
                schema: "kursovoi",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "EducationRequirements",
                schema: "kursovoi",
                table: "job_ads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MinExperienceYears",
                schema: "kursovoi",
                table: "job_ads",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RequiredSkills",
                schema: "kursovoi",
                table: "job_ads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "character varying(10000)",
                maxLength: 10000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "EmploymentRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    JobAdId = table.Column<int>(type: "integer", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmploymentRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmploymentRecords_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "kursovoi",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmploymentRecords_job_ads_JobAdId",
                        column: x => x.JobAdId,
                        principalSchema: "kursovoi",
                        principalTable: "job_ads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentRecords_JobAdId",
                table: "EmploymentRecords",
                column: "JobAdId");

            migrationBuilder.CreateIndex(
                name: "IX_EmploymentRecords_UserId",
                table: "EmploymentRecords",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_applicantid",
                schema: "kursovoi",
                table: "Chats",
                column: "applicantid",
                principalSchema: "kursovoi",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_employerid",
                schema: "kursovoi",
                table: "Chats",
                column: "employerid",
                principalSchema: "kursovoi",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_job_ads_jobadid",
                schema: "kursovoi",
                table: "Chats",
                column: "jobadid",
                principalSchema: "kursovoi",
                principalTable: "job_ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_applicantid",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_Users_employerid",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_job_ads_jobadid",
                schema: "kursovoi",
                table: "Chats");

            migrationBuilder.DropTable(
                name: "EmploymentRecords");

            migrationBuilder.DropColumn(
                name: "ExperienceYears",
                schema: "kursovoi",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Skills",
                schema: "kursovoi",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserType",
                schema: "kursovoi",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EducationRequirements",
                schema: "kursovoi",
                table: "job_ads");

            migrationBuilder.DropColumn(
                name: "MinExperienceYears",
                schema: "kursovoi",
                table: "job_ads");

            migrationBuilder.DropColumn(
                name: "RequiredSkills",
                schema: "kursovoi",
                table: "job_ads");

            migrationBuilder.RenameTable(
                name: "Messages",
                schema: "kursovoi",
                newName: "Messages");

            migrationBuilder.RenameColumn(
                name: "jobadid",
                schema: "kursovoi",
                table: "Chats",
                newName: "JobAdId");

            migrationBuilder.RenameColumn(
                name: "employerid",
                schema: "kursovoi",
                table: "Chats",
                newName: "EmployerId");

            migrationBuilder.RenameColumn(
                name: "applicantid",
                schema: "kursovoi",
                table: "Chats",
                newName: "ApplicantId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_jobadid",
                schema: "kursovoi",
                table: "Chats",
                newName: "IX_Chats_JobAdId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_employerid",
                schema: "kursovoi",
                table: "Chats",
                newName: "IX_Chats_EmployerId");

            migrationBuilder.RenameIndex(
                name: "IX_Chats_applicantid",
                schema: "kursovoi",
                table: "Chats",
                newName: "IX_Chats_ApplicantId");

            migrationBuilder.AlterColumn<string>(
                name: "Text",
                table: "Comments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(10000)",
                oldMaxLength: 10000);

            migrationBuilder.AddColumn<int>(
                name: "JobAdId1",
                schema: "kursovoi",
                table: "Chats",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chats_JobAdId1",
                schema: "kursovoi",
                table: "Chats",
                column: "JobAdId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_ApplicantId",
                schema: "kursovoi",
                table: "Chats",
                column: "ApplicantId",
                principalSchema: "kursovoi",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_Users_EmployerId",
                schema: "kursovoi",
                table: "Chats",
                column: "EmployerId",
                principalSchema: "kursovoi",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_job_ads_JobAdId",
                schema: "kursovoi",
                table: "Chats",
                column: "JobAdId",
                principalSchema: "kursovoi",
                principalTable: "job_ads",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_job_ads_JobAdId1",
                schema: "kursovoi",
                table: "Chats",
                column: "JobAdId1",
                principalSchema: "kursovoi",
                principalTable: "job_ads",
                principalColumn: "Id");
        }
    }
}
