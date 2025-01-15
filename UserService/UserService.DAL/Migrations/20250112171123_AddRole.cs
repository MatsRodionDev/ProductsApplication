using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("852b97b2-3bb1-4c4d-b154-8fb7c807cac6"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cb31fa7e-e922-486f-8572-5313b4eb5f87"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { new Guid("49e71118-35bd-4321-b87e-9c7b3ea37aff"), "Admin" },
                    { new Guid("7cfefc26-1d21-4be9-b554-fe9659b53d70"), "Worker" },
                    { new Guid("cba0e59b-ed28-44eb-9654-143fd8f89e14"), "User" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("49e71118-35bd-4321-b87e-9c7b3ea37aff"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("7cfefc26-1d21-4be9-b554-fe9659b53d70"));

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "Id",
                keyValue: new Guid("cba0e59b-ed28-44eb-9654-143fd8f89e14"));

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { new Guid("852b97b2-3bb1-4c4d-b154-8fb7c807cac6"), "Admin" },
                    { new Guid("cb31fa7e-e922-486f-8572-5313b4eb5f87"), "User" }
                });
        }
    }
}
