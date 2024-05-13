using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class RestrictAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Restricts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RestrictedCount = table.Column<int>(type: "integer", nullable: false),
                    TicketTypeId = table.Column<int>(type: "integer", nullable: false),
                    WaterBodyId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restricts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restricts_TicketTypes_TicketTypeId",
                        column: x => x.TicketTypeId,
                        principalTable: "TicketTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restricts_WaterBodies_WaterBodyId",
                        column: x => x.WaterBodyId,
                        principalTable: "WaterBodies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restricts_TicketTypeId",
                table: "Restricts",
                column: "TicketTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Restricts_WaterBodyId",
                table: "Restricts",
                column: "WaterBodyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Restricts");
        }
    }
}
