using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CubosFinance.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddReversedFromTransactionIdToTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReversedFromTransactionId",
                table: "Transactions",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReversedFromTransactionId",
                table: "Transactions");
        }
    }
}
