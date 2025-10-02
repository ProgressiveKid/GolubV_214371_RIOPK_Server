using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CorporateRiskManagementSystemBack.Migrations
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "corp_risk_management");

            migrationBuilder.CreateTable(
                name: "departments",
                schema: "corp_risk_management",
                columns: table => new
                {
                    department_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_departments", x => x.department_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "corp_risk_management",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    full_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "audit_reports",
                schema: "corp_risk_management",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    author_id = table.Column<int>(type: "integer", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    department_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("audit_reports_pkey", x => x.report_id);
                    table.ForeignKey(
                        name: "audit_reports_author_id_fkey",
                        column: x => x.author_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "audit_reports_department_id_fkey",
                        column: x => x.department_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "departments",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "risks",
                schema: "corp_risk_management",
                columns: table => new
                {
                    risk_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    severity = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    likelihood = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    created_by_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_risks", x => x.risk_id);
                    table.ForeignKey(
                        name: "risks_created_by_id_fkey",
                        column: x => x.created_by_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "risk_assessments",
                schema: "corp_risk_management",
                columns: table => new
                {
                    assessment_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    risk_id = table.Column<int>(type: "integer", nullable: false),
                    assessed_by_id = table.Column<int>(type: "integer", nullable: false),
                    assessment_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_DATE"),
                    impact_score = table.Column<short>(type: "smallint", nullable: true),
                    probability_score = table.Column<short>(type: "smallint", nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("risk_assessments_pkey", x => x.assessment_id);
                    table.ForeignKey(
                        name: "risk_assessments_assessed_by_id_fkey",
                        column: x => x.assessed_by_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "risk_assessments_risk_id_fkey",
                        column: x => x.risk_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "risks",
                        principalColumn: "risk_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "risk_departments",
                schema: "corp_risk_management",
                columns: table => new
                {
                    risk_id = table.Column<int>(type: "integer", nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("risk_departments_pkey", x => new { x.risk_id, x.department_id });
                    table.ForeignKey(
                        name: "risk_departments_department_id_fkey",
                        column: x => x.department_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "departments",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "risk_departments_risk_id_fkey",
                        column: x => x.risk_id,
                        principalSchema: "corp_risk_management",
                        principalTable: "risks",
                        principalColumn: "risk_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_audit_author",
                schema: "corp_risk_management",
                table: "audit_reports",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "idx_audit_department",
                schema: "corp_risk_management",
                table: "audit_reports",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "departments_name_key",
                schema: "corp_risk_management",
                table: "departments",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_assessment_risk",
                schema: "corp_risk_management",
                table: "risk_assessments",
                column: "risk_id");

            migrationBuilder.CreateIndex(
                name: "idx_assessment_user",
                schema: "corp_risk_management",
                table: "risk_assessments",
                column: "assessed_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_risk_departments_department_id",
                schema: "corp_risk_management",
                table: "risk_departments",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "idx_risks_created_by",
                schema: "corp_risk_management",
                table: "risks",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "users_email_key",
                schema: "corp_risk_management",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_username_key",
                schema: "corp_risk_management",
                table: "users",
                column: "username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_reports",
                schema: "corp_risk_management");

            migrationBuilder.DropTable(
                name: "risk_assessments",
                schema: "corp_risk_management");

            migrationBuilder.DropTable(
                name: "risk_departments",
                schema: "corp_risk_management");

            migrationBuilder.DropTable(
                name: "departments",
                schema: "corp_risk_management");

            migrationBuilder.DropTable(
                name: "risks",
                schema: "corp_risk_management");

            migrationBuilder.DropTable(
                name: "users",
                schema: "corp_risk_management");
        }
    }
}
