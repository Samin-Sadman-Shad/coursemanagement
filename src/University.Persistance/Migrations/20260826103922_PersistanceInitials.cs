using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class PersistanceInitials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CreadtedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_Staff_CreadtedById",
                        column: x => x.CreadtedById,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Staff_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "Staff",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CreditWorks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Heading = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreadtedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditWorks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditWorks_Staff_CreadtedById",
                        column: x => x.CreadtedById,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditWorks_Staff_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "Staff",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Roll = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    CreadtedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Students_Staff_CreadtedById",
                        column: x => x.CreadtedById,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Students_Staff_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "Staff",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "CreditWorksInCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditWorkId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditWorksInCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditWorksInCourses_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditWorksInCourses_CreditWorks_CreditWorkId",
                        column: x => x.CreditWorkId,
                        principalTable: "CreditWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreadtedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EnrolledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Staff_CreadtedById",
                        column: x => x.CreadtedById,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Staff_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "Staff",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditWorkEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreditWorkId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreadtedById = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModifiedByUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EnrolledAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    StaffId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditWorkEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditWorkEnrollments_CreditWorks_CreditWorkId",
                        column: x => x.CreditWorkId,
                        principalTable: "CreditWorks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditWorkEnrollments_Staff_CreadtedById",
                        column: x => x.CreadtedById,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditWorkEnrollments_Staff_LastModifiedByUserId",
                        column: x => x.LastModifiedByUserId,
                        principalTable: "Staff",
                        principalColumn: "UserId");
                    table.ForeignKey(
                        name: "FK_CreditWorkEnrollments_Staff_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Staff",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditWorkEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_CourseId",
                table: "CourseEnrollments",
                column: "CourseId");

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

            migrationBuilder.CreateIndex(
                name: "IX_CourseEnrollments_StudentId",
                table: "CourseEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreadtedById",
                table: "Courses",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_LastModifiedByUserId",
                table: "Courses",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_CreadtedById",
                table: "CreditWorkEnrollments",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_CreditWorkId",
                table: "CreditWorkEnrollments",
                column: "CreditWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_LastModifiedByUserId",
                table: "CreditWorkEnrollments",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_StaffId",
                table: "CreditWorkEnrollments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorkEnrollments_StudentId",
                table: "CreditWorkEnrollments",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorks_CreadtedById",
                table: "CreditWorks",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorks_LastModifiedByUserId",
                table: "CreditWorks",
                column: "LastModifiedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorksInCourses_CourseId",
                table: "CreditWorksInCourses",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditWorksInCourses_CreditWorkId",
                table: "CreditWorksInCourses",
                column: "CreditWorkId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CreadtedById",
                table: "Students",
                column: "CreadtedById");

            migrationBuilder.CreateIndex(
                name: "IX_Students_LastModifiedByUserId",
                table: "Students",
                column: "LastModifiedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseEnrollments");

            migrationBuilder.DropTable(
                name: "CreditWorkEnrollments");

            migrationBuilder.DropTable(
                name: "CreditWorksInCourses");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CreditWorks");

            migrationBuilder.DropTable(
                name: "Staff");
        }
    }
}
