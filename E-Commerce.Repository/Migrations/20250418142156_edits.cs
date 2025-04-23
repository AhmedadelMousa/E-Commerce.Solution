using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Repository.Migrations
{
    /// <inheritdoc />
    public partial class edits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakeReviews_Products_ProductId",
                table: "MakeReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MakeReviews",
                table: "MakeReviews");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "MakeReviews",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "MakeReviews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MakeReviews",
                table: "MakeReviews",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MakeReviews_AppUserId",
                table: "MakeReviews",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MakeReviews_Products_ProductId",
                table: "MakeReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MakeReviews_Products_ProductId",
                table: "MakeReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MakeReviews",
                table: "MakeReviews");

            migrationBuilder.DropIndex(
                name: "IX_MakeReviews_AppUserId",
                table: "MakeReviews");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MakeReviews");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "MakeReviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MakeReviews",
                table: "MakeReviews",
                columns: new[] { "AppUserId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_MakeReviews_Products_ProductId",
                table: "MakeReviews",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
