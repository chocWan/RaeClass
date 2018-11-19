using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RaeClass.Migrations
{
    public partial class raedb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.RenameTable(
                name: "ReadContent",
                newName: "ReadContentSet");

            migrationBuilder.CreateTable(
                name: "ListenContentSet",
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
                    table.PrimaryKey("PK_ListenContentSet", x => x.FId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListenContentSet");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadContentSet",
                table: "ReadContentSet");

            migrationBuilder.RenameTable(
                name: "ReadContentSet",
                newName: "ReadContent");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadContent",
                table: "ReadContent",
                column: "FId");
        }
    }
}
