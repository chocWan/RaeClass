using Microsoft.EntityFrameworkCore.Migrations;

namespace RaeClass.Migrations
{
    public partial class raedb2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
           
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "ReadContentSet",
                newName: "ReadContent",
                newSchema: "dbo");

            migrationBuilder.RenameTable(
                name: "ListenContentSet",
                newName: "ListenContent",
                newSchema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ReadContent",
                schema: "dbo",
                table: "ReadContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ListenContent",
                schema: "dbo",
                table: "ListenContent");

            migrationBuilder.RenameTable(
                name: "ReadContent",
                schema: "dbo",
                newName: "ReadContentSet");

            migrationBuilder.RenameTable(
                name: "ListenContent",
                schema: "dbo",
                newName: "ListenContentSet");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReadContentSet",
                table: "ReadContentSet",
                column: "FId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ListenContentSet",
                table: "ListenContentSet",
                column: "FId");
        }
    }
}
