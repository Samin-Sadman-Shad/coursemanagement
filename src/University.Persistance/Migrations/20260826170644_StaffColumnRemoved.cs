using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class StaffColumnRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseEnrollments_Staff_CreadtedById",
                table: "CourseEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEnrollments_Staff_LastModifiedByUserId",
                table: "CourseEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseEnrollments_Staff_StaffId",
                table: "CourseEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Staff_CreadtedById",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Staff_LastModifiedByUserId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditWorkEnrollments_Staff_CreadtedById",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditWorkEnrollments_Staff_LastModifiedByUserId",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditWorkEnrollments_Staff_StaffId",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditWorks_Staff_CreadtedById",
                table: "CreditWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_CreditWorks_Staff_LastModifiedByUserId",
                table: "CreditWorks");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Staff_CreadtedById",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Staff_LastModifiedByUserId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropIndex(
                name: "IX_Students_CreadtedById",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_LastModifiedByUserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_CreditWorks_CreadtedById",
                table: "CreditWorks");

            migrationBuilder.DropIndex(
                name: "IX_CreditWorks_LastModifiedByUserId",
                table: "CreditWorks");

            migrationBuilder.DropIndex(
                name: "IX_CreditWorkEnrollments_CreadtedById",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_CreditWorkEnrollments_LastModifiedByUserId",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_CreditWorkEnrollments_StaffId",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CreadtedById",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_LastModifiedByUserId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_CourseEnrollments_CreadtedById",
                table: "CourseEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_CourseEnrollments_LastModifiedByUserId",
                table: "CourseEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_CourseEnrollments_StaffId",
                table: "CourseEnrollments");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "CreditWorks");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "LastModifiedByUserId",
                table: "CourseEnrollments");

            migrationBuilder.RenameColumn(
                name: "CreadtedById",
                table: "Students",
                newName: "LastModifiedById");

            migrationBuilder.RenameColumn(
                name: "CreadtedById",
                table: "CreditWorks",
                newName: "LastModifiedById");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "CreditWorkEnrollments",
                newName: "LastModifiedById");

            migrationBuilder.RenameColumn(
                name: "CreadtedById",
                table: "CreditWorkEnrollments",
                newName: "EnrolledById");

            migrationBuilder.RenameColumn(
                name: "CreadtedById",
                table: "Courses",
                newName: "LastModifiedById");

            migrationBuilder.RenameColumn(
                name: "StaffId",
                table: "CourseEnrollments",
                newName: "LastModifiedById");

            migrationBuilder.RenameColumn(
                name: "CreadtedById",
                table: "CourseEnrollments",
                newName: "EnrolledById");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Students",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CreditWorks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CreditWorkEnrollments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "Courses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CourseEnrollments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CreditWorks");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CourseEnrollments");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "Students",
                newName: "CreadtedById");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "CreditWorks",
                newName: "CreadtedById");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "CreditWorkEnrollments",
                newName: "StaffId");

            migrationBuilder.RenameColumn(
                name: "EnrolledById",
                table: "CreditWorkEnrollments",
                newName: "CreadtedById");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "Courses",
                newName: "CreadtedById");

            migrationBuilder.RenameColumn(
                name: "LastModifiedById",
                table: "CourseEnrollments",
                newName: "StaffId");

            migrationBuilder.RenameColumn(
                name: "EnrolledById",
                table: "CourseEnrollments",
                newName: "CreadtedById");

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedByUserId",
                table: "Students",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedByUserId",
                table: "CreditWorks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedByUserId",
                table: "CreditWorkEnrollments",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedByUserId",
                table: "Courses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedByUserId",
                table: "CourseEnrollments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_CreadtedById",
                table: "Students",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastModifiedByUserId",
                table: "Students",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorks_CreadtedById",
                table: "CreditWorks",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorks_LastModifiedByUserId",
                table: "CreditWorks",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_CreadtedById",
                table: "CreditWorkEnrollments",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_LastModifiedByUserId",
                table: "CreditWorkEnrollments",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_StaffId",
                table: "CreditWorkEnrollments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreadtedById",
                table: "Courses",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LastModifiedByUserId",
                table: "Courses",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CreadtedById",
                table: "CourseEnrollments",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_LastModifiedByUserId",
                table: "CourseEnrollments",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_StaffId",
                table: "CourseEnrollments",
                column: "StaffId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEnrollments_Staff_CreadtedById",
                table: "CourseEnrollments",
                column: "CreadtedById",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEnrollments_Staff_LastModifiedByUserId",
                table: "CourseEnrollments",
                column: "LastModifiedByUserId",
                principalTable: "Staff",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseEnrollments_Staff_StaffId",
                table: "CourseEnrollments",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Staff_CreadtedById",
                table: "Courses",
                column: "CreadtedById",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Staff_LastModifiedByUserId",
                table: "Courses",
                column: "LastModifiedByUserId",
                principalTable: "Staff",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditWorkEnrollments_Staff_CreadtedById",
                table: "CreditWorkEnrollments",
                column: "CreadtedById",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditWorkEnrollments_Staff_LastModifiedByUserId",
                table: "CreditWorkEnrollments",
                column: "LastModifiedByUserId",
                principalTable: "Staff",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreditWorkEnrollments_Staff_StaffId",
                table: "CreditWorkEnrollments",
                column: "StaffId",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditWorks_Staff_CreadtedById",
                table: "CreditWorks",
                column: "CreadtedById",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditWorks_Staff_LastModifiedByUserId",
                table: "CreditWorks",
                column: "LastModifiedByUserId",
                principalTable: "Staff",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Staff_CreadtedById",
                table: "Students",
                column: "CreadtedById",
                principalTable: "Staff",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Staff_LastModifiedByUserId",
                table: "Students",
                column: "LastModifiedByUserId",
                principalTable: "Staff",
                principalColumn: "UserId");
        }
    }
}
