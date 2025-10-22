using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactList.Migrations
{
    /// <inheritdoc />
    public partial class CategorySubcategoryReferenceObjects : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Contacts",
                type: "int",
                nullable: false,
                defaultValue: 3);

            migrationBuilder.AddColumn<int>(
                name: "SubcategoryId",
                table: "Contacts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VisibleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subcategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VisibleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subcategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subcategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name", "VisibleName" },
                values: new object[,]
                {
                    { 1, "Private", "Private" },
                    { 2, "Business", "Business" },
                    { 3, "Other", "Other" }
                });

            migrationBuilder.InsertData(
                table: "Subcategories",
                columns: new[] { "Id", "CategoryId", "Name", "VisibleName" },
                values: new object[,]
                {
                    { 1, 2, "Boss", "Boss" },
                    { 2, 2, "Client", "Client" },
                    { 3, 2, "Coworker", "Coworker" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_CategoryId",
                table: "Contacts",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_SubcategoryId",
                table: "Contacts",
                column: "SubcategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_CategoryId",
                table: "Subcategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subcategories_Name_CategoryId",
                table: "Subcategories",
                columns: new[] { "Name", "CategoryId" },
                unique: true);

            // Ensure no invalid default values remain before adding FK
            migrationBuilder.Sql("UPDATE [Contacts] SET [CategoryId] = 3 WHERE [CategoryId] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Categories_CategoryId",
                table: "Contacts",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Subcategories_SubcategoryId",
                table: "Contacts",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Categories_CategoryId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Subcategories_SubcategoryId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Subcategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_CategoryId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_SubcategoryId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "SubcategoryId",
                table: "Contacts");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
