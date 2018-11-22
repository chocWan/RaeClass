using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RaeClass.Migrations
{
    public partial class raedb5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FNumber",
                schema: "dbo",
                table: "ReadContent",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FNumber",
                schema: "dbo",
                table: "ListenContent",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SerialNumber",
                schema: "dbo",
                columns: table => new
                {
                    FID = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FContentType = table.Column<string>(nullable: true),
                    FCurrentGeneratedIndex = table.Column<int>(nullable: false),
                    FModifyTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SerialNumber", x => x.FID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SerialNumber",
                schema: "dbo");

            migrationBuilder.DropColumn(
                name: "FNumber",
                schema: "dbo",
                table: "ReadContent");

            migrationBuilder.DropColumn(
                name: "FNumber",
                schema: "dbo",
                table: "ListenContent");
        }
    }
}
