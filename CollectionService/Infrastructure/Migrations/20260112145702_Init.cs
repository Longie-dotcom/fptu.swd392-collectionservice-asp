using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CollectorProfiles",
                columns: table => new
                {
                    CollectorProfileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectorProfiles", x => x.CollectorProfileID);
                });

            migrationBuilder.CreateTable(
                name: "CollectionTasks",
                columns: table => new
                {
                    CollectionTaskID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CollectionReportID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountEstimated = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CollectorProfileID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CollectionTasks", x => x.CollectionTaskID);
                    table.ForeignKey(
                        name: "FK_CollectionTasks_CollectorProfiles_CollectorProfileID",
                        column: x => x.CollectorProfileID,
                        principalTable: "CollectorProfiles",
                        principalColumn: "CollectorProfileID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CollectionTasks_CollectorProfileID",
                table: "CollectionTasks",
                column: "CollectorProfileID");

            migrationBuilder.CreateIndex(
                name: "IX_CollectorProfiles_UserID",
                table: "CollectorProfiles",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CollectionTasks");

            migrationBuilder.DropTable(
                name: "CollectorProfiles");
        }
    }
}
