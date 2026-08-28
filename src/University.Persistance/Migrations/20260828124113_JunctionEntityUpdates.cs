using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class JunctionEntityUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                table: "CreditWorksInCourses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedById",
                table: "CreditWorksInCourses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EnrolledAt",
                table: "CreditWorksInCourses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "EnrolledById",
                table: "CreditWorksInCourses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LastModifiedAt",
                table: "CreditWorksInCourses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifiedById",
                table: "CreditWorksInCourses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CreditWorksInCourses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CreditWorksInCourses");

            migrationBuilder.DropColumn(
                name: "EnrolledAt",
                table: "CreditWorksInCourses");

            migrationBuilder.DropColumn(
                name: "EnrolledById",
                table: "CreditWorksInCourses");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "CreditWorksInCourses");

            migrationBuilder.DropColumn(
                name: "LastModifiedById",
                table: "CreditWorksInCourses");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Students",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
