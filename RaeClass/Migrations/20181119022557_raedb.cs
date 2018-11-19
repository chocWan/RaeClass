using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RaeClass.Migrations
{
    public partial class raedb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReadContent",
                columns: table => new
                {
                    FId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FJsonData = table.Column<string>(nullable: true),
                    FCreateTime = table.Column<DateTime>(nullable: false),
                    FModifyTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadContent", x => x.FId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReadContent");
        }
    }
}
