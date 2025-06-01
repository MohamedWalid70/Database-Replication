using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Replication.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class addingTheSampleTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnotherTransactionReplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnotherTransactionReplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AnotherTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnotherTransactions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnotherTransactionReplications");

            migrationBuilder.DropTable(
                name: "AnotherTransactions");
        }
    }
}
