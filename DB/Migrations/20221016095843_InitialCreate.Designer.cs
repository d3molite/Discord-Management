﻿// <auto-generated />
using System;
using DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DB.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    [Migration("20221016095843_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("DB.Models.Bot", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActiveInDebug")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActiveInRelease")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Presence")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bots");
                });

            modelBuilder.Entity("DB.Models.Configs.GuildConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BotId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LinkedGuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BotId");

                    b.HasIndex("LinkedGuildId");

                    b.ToTable("GuildConfig");
                });

            modelBuilder.Entity("DB.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ItemId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Guild");
                });

            modelBuilder.Entity("DB.Models.Configs.GuildConfig", b =>
                {
                    b.HasOne("DB.Models.Bot", null)
                        .WithMany("Configs")
                        .HasForeignKey("BotId");

                    b.HasOne("DB.Models.Guild", "LinkedGuild")
                        .WithMany()
                        .HasForeignKey("LinkedGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LinkedGuild");
                });

            modelBuilder.Entity("DB.Models.Bot", b =>
                {
                    b.Navigation("Configs");
                });
#pragma warning restore 612, 618
        }
    }
}
