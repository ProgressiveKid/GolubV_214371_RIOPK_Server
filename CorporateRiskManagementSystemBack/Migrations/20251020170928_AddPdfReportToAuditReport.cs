using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateRiskManagementSystemBack.Migrations
{
    public partial class AddPdfReportToAuditReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "pdf_report",
                schema: "corp_risk_management",
                table: "audit_reports",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "pdf_report",
                schema: "corp_risk_management",
                table: "audit_reports");
        }
    }
}
