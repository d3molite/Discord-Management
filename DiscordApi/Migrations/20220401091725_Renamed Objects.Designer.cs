﻿// <auto-generated />
using System;
using DiscordApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DiscordManager.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20220401091725_Renamed Objects")]
    partial class RenamedObjects
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.2");

            modelBuilder.Entity("DiscordApi.Models.Bot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Presence")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Bots");
                });

            modelBuilder.Entity("DiscordApi.Models.BotConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedBotID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedGuildId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RelatedLoggerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("RelatedBotID");

                    b.HasIndex("RelatedGuildId");

                    b.HasIndex("RelatedLoggerID");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("DiscordApi.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BotID")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BotID");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("DiscordApi.Models.LoggingConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableLogging")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LogMessageDeleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LogUserBanned")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LogUserJoined")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LogUserKicked")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LogUserLeft")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("LoggingChannelID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("LoggingConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.BotConfig", b =>
                {
                    b.HasOne("DiscordApi.Models.Bot", "RelatedBot")
                        .WithMany()
                        .HasForeignKey("RelatedBotID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.Guild", "RelatedGuild")
                        .WithMany()
                        .HasForeignKey("RelatedGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.LoggingConfig", "RelatedLogger")
                        .WithMany()
                        .HasForeignKey("RelatedLoggerID");

                    b.Navigation("RelatedBot");

                    b.Navigation("RelatedGuild");

                    b.Navigation("RelatedLogger");
                });

            modelBuilder.Entity("DiscordApi.Models.Guild", b =>
                {
                    b.HasOne("DiscordApi.Models.Bot", null)
                        .WithMany("Servers")
                        .HasForeignKey("BotID");
                });

            modelBuilder.Entity("DiscordApi.Models.Bot", b =>
                {
                    b.Navigation("Servers");
                });
#pragma warning restore 612, 618
        }
    }
}
