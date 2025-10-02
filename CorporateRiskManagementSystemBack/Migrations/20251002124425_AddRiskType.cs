using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateRiskManagementSystemBack.Migrations
{
    public partial class AddRiskType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RiskType",
                schema: "corp_risk_management",
                table: "risks",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RiskType",
                schema: "corp_risk_management",
                table: "risks");
        }
    }
}
