using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class inicijalnaBaza_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OdrzaniCas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumOdrzanogCasa = table.Column<DateTime>(nullable: false),
                    NastavnikID = table.Column<int>(nullable: false),
                    OdjeljenjeID = table.Column<int>(nullable: false),
                    PredmetID = table.Column<int>(nullable: false),
                    SadrzajCasa = table.Column<string>(nullable: true),
                    SkolaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdrzaniCas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdrzaniCas_Nastavnik_NastavnikID",
                        column: x => x.NastavnikID,
                        principalTable: "Nastavnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OdrzaniCas_Odjeljenje_OdjeljenjeID",
                        column: x => x.OdjeljenjeID,
                        principalTable: "Odjeljenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OdrzaniCas_Predmet_PredmetID",
                        column: x => x.PredmetID,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OdrzaniCas_Skola_SkolaID",
                        column: x => x.SkolaID,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OdrzanCasDetalji",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Napomena = table.Column<string>(nullable: true),
                    Ocjena = table.Column<int>(nullable: false),
                    OdjeljenjeStavkaID = table.Column<int>(nullable: false),
                    OdrzaniCasID = table.Column<int>(nullable: false),
                    OpravdanoOdsutan = table.Column<bool>(nullable: false),
                    Prisutan = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdrzanCasDetalji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdrzanCasDetalji_OdjeljenjeStavka_OdjeljenjeStavkaID",
                        column: x => x.OdjeljenjeStavkaID,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdrzanCasDetalji_OdrzaniCas_OdrzaniCasID",
                        column: x => x.OdrzaniCasID,
                        principalTable: "OdrzaniCas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OdrzanCasDetalji_OdjeljenjeStavkaID",
                table: "OdrzanCasDetalji",
                column: "OdjeljenjeStavkaID");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzanCasDetalji_OdrzaniCasID",
                table: "OdrzanCasDetalji",
                column: "OdrzaniCasID");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCas_NastavnikID",
                table: "OdrzaniCas",
                column: "NastavnikID");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCas_OdjeljenjeID",
                table: "OdrzaniCas",
                column: "OdjeljenjeID");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCas_PredmetID",
                table: "OdrzaniCas",
                column: "PredmetID");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCas_SkolaID",
                table: "OdrzaniCas",
                column: "SkolaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdrzanCasDetalji");

            migrationBuilder.DropTable(
                name: "OdrzaniCas");
        }
    }
}
