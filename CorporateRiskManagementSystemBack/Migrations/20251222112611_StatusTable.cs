using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CorporateRiskManagementSystemBack.Migrations
{
    public partial class StatusTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "statuses",
                schema: "corp_risk_management",
                columns: table => new
                {
                    status_id = table.Column<int>(type: "integer", nullable: false, defaultValueSql: "nextval('statuses_status_id_seq')"),
                    risk_id = table.Column<int>(type: "integer", nullable: false),
                    status_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status_description = table.Column<string>(type: "text", nullable: true),
                    changed_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    changed_by_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("statuses_pkey", x => x.status_id);
                    table.ForeignKey(
                        name: "statuses_changed_by_id_fkey",
                        column: x => x.changed_by_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "statuses_risk_id_fkey",
                        column: x => x.risk_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "risks",
                        principalColumn: "risk_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_statuses_changed_at",
                schema: "corp_risk_management",
                table: "statuses",
                column: "changed_at");

            migrationBuilder.CreateIndex(
                name: "idx_statuses_risk_id",
                schema: "corp_risk_management",
                table: "statuses",
                column: "risk_id");

            migrationBuilder.CreateIndex(
                name: "idx_statuses_status_name",
                schema: "corp_risk_management",
                table: "statuses",
                column: "status_name");

            migrationBuilder.CreateIndex(
                name: "IX_statuses_changed_by_id",
                schema: "corp_risk_management",
                table: "statuses",
                column: "changed_by_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "statuses",
                schema: "corp_risk_management");
        }
    }
}
