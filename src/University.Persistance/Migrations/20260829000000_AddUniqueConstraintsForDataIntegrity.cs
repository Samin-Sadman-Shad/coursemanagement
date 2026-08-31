using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintsForDataIntegrity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create unique composite index for CourseCreditWork (CourseId, CreditWorkId)
            migrationBuilder.CreateIndex(
                name: "IX_CourseId_CreditWorkId_Unique",
                table: "CreditWorksInCourses",
                columns: new[] { "CourseId", "CreditWorkId" },
                unique: true);

            // Create unique composite index for CourseEnrollment (StudentId, CourseId)
            migrationBuilder.CreateIndex(
                name: "IX_StudentId_CourseId_Unique",
                table: "CourseEnrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            // Create unique composite index for CreditWorkEnrollment (StudentId, CreditWorkId)
            migrationBuilder.CreateIndex(
                name: "IX_StudentId_CreditWorkId_Unique",
                table: "CreditWorkEnrollments",
                columns: new[] { "StudentId", "CreditWorkId" },
                unique: true);

            // Create unique index for Course Title
            migrationBuilder.CreateIndex(
                name: "IX_Course_Title_Unique",
                table: "Courses",
                column: "Title",
                unique: true);

            // Create unique composite index for CreditWork (Heading, Code)
            migrationBuilder.CreateIndex(
                name: "IX_CreditWork_Heading_Code_Unique",
                table: "CreditWorks",
                columns: new[] { "Heading", "Code" },
                unique: true);

            // Create unique index for Student Email
            migrationBuilder.CreateIndex(
                name: "IX_Student_Email_Unique",
                table: "Students",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CourseId_CreditWorkId_Unique",
                table: "CreditWorksInCourses");

            migrationBuilder.DropIndex(
                name: "IX_StudentId_CourseId_Unique",
                table: "CourseEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_StudentId_CreditWorkId_Unique",
                table: "CreditWorkEnrollments");

            migrationBuilder.DropIndex(
                name: "IX_Course_Title_Unique",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_CreditWork_Heading_Code_Unique",
                table: "CreditWorks");

            migrationBuilder.DropIndex(
                name: "IX_Student_Email_Unique",
                table: "Students");
        }
    }
}
