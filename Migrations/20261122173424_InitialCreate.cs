using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlogMVC.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Content", "PublishedAt", "Summary", "Title" },
                values: new object[,]
                {
                    { 1, "C# modern ve nesne yönelimli bir programlama dilidir. Microsoft tarafından geliştirilmiştir. Web ve masaüstü uygulamalarında kullanılabilir.", new DateTime(2026, 11, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# dilinin temellerini öğreniyoruz.", "C# Programlama Dili" },
                    { 2, "ASP.NET Core MVC, web uygulamaları geliştirmek için kullanılan bir framework yapısıdır. Model, View ve Controller bileşenlerinden oluşur.", new DateTime(2026, 11, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "MVC mimarisini yakından tanıyalım.", "ASP.NET Core MVC Nedir?" },
                    { 3, "SQL Server, Microsoft tarafından geliştirilen ilişkisel bir veritabanı yönetim sistemidir. Verileri saklamak, sorgulamak ve yönetmek için kullanılır.", new DateTime(2026, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Veritabanı dünyasına ilk adım.", "SQL Server'a Giriş" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");
        }
    }
}
