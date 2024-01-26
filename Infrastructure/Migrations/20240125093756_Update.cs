using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Albums_AlbumID",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "AlbumID",
                table: "Tracks",
                newName: "AlbumId");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_AlbumID",
                table: "Tracks",
                newName: "IX_Tracks_AlbumId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tracks_Albums_AlbumId",
                table: "Tracks");

            migrationBuilder.RenameColumn(
                name: "AlbumId",
                table: "Tracks",
                newName: "AlbumID");

            migrationBuilder.RenameIndex(
                name: "IX_Tracks_AlbumId",
                table: "Tracks",
                newName: "IX_Tracks_AlbumID");

            migrationBuilder.AddForeignKey(
                name: "FK_Tracks_Albums_AlbumID",
                table: "Tracks",
                column: "AlbumID",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
