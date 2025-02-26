﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pokemon.Data;

#nullable disable

namespace Pokemon.Migrations
{
    [DbContext(typeof(PokemonDbContext))]
    [Migration("20250226170433_AddPokemonNamesToBattleHistory")]
    partial class AddPokemonNamesToBattleHistory
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.2");

            modelBuilder.Entity("Pokemon.Models.BattleHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Pokemon1Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pokemon1Picture")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pokemon2Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Pokemon2Picture")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Result")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("BattleHistories");
                });

            modelBuilder.Entity("Pokemon.Models.FavoritePokemon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PokemonId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Favorites");
                });
#pragma warning restore 612, 618
        }
    }
}
