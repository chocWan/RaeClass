using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RaeClass.Migrations
{
    public partial class rae1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "BaseFormContent",
                schema: "dbo",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FContentType = table.Column<string>(nullable: true),
                    FDocStatus = table.Column<string>(nullable: true),
                    FNumber = table.Column<string>(nullable: true),
                    FName = table.Column<string>(nullable: true),
                    FLevel = table.Column<string>(nullable: true),
                    FJsonData = table.Column<string>(nullable: true),
                    FCreateTime = table.Column<DateTime>(nullable: false),
                    FModifyTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseFormContent", x => x.FId);
                });

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
                name: "BaseFormContent",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SerialNumber",
                schema: "dbo");
        }
    }
}
