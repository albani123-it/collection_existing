using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Collectium.Migrations
{
    public partial class three : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "agent_loan",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    loan_id = table.Column<int>(type: "int", nullable: true),
                    dist_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agent_loan", x => x.id);
                    table.ForeignKey(
                        name: "FK_agent_loan_distr_rule_dist_id",
                        column: x => x.dist_id,
                        principalTable: "distr_rule",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_agent_loan_master_loan_loan_id",
                        column: x => x.loan_id,
                        principalTable: "master_loan",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_agent_loan_dist_id",
                table: "agent_loan",
                column: "dist_id");

            migrationBuilder.CreateIndex(
                name: "IX_agent_loan_loan_id",
                table: "agent_loan",
                column: "loan_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agent_loan");
        }
    }
}
