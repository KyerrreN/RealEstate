﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstate.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddAuth0IdToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auth0Id",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auth0Id",
                table: "Users");
        }
    }
}
