using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAcceess.Migrations
{
    public partial class AddDecisionMadeField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DecisionMade",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecisionMade",
                table: "Persons");
        }
    }
}
