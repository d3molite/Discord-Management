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
    [Migration("20221016114733_GuildChannelExtension")]
    partial class GuildChannelExtension
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
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Presence")
                        .HasColumnType("TEXT");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Bots");
                });

            modelBuilder.Entity("DB.Models.Configs.Extensions.AntiSpamConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AntiSpamConfig");
                });

            modelBuilder.Entity("DB.Models.Configs.Extensions.FeedbackConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsReactionsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TargetChannelId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TargetChannelId");

                    b.ToTable("FeedbackConfig");
                });

            modelBuilder.Entity("DB.Models.Configs.GuildConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AntiSpamConfigId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BotId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FeedbackConfigId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LinkedGuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AntiSpamConfigId");

                    b.HasIndex("BotId");

                    b.HasIndex("FeedbackConfigId");

                    b.HasIndex("LinkedGuildId");

                    b.ToTable("GuildConfigs");
                });

            modelBuilder.Entity("DB.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("Snowflake")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Guild");
                });

            modelBuilder.Entity("DB.Models.GuildChannel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChannelType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LinkedGuildId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("Snowflake")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LinkedGuildId");

                    b.ToTable("GuildChannel");
                });

            modelBuilder.Entity("DB.Models.Configs.Extensions.FeedbackConfig", b =>
                {
                    b.HasOne("DB.Models.GuildChannel", "TargetChannel")
                        .WithMany()
                        .HasForeignKey("TargetChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("TargetChannel");
                });

            modelBuilder.Entity("DB.Models.Configs.GuildConfig", b =>
                {
                    b.HasOne("DB.Models.Configs.Extensions.AntiSpamConfig", "AntiSpamConfig")
                        .WithMany()
                        .HasForeignKey("AntiSpamConfigId");

                    b.HasOne("DB.Models.Bot", null)
                        .WithMany("Configs")
                        .HasForeignKey("BotId");

                    b.HasOne("DB.Models.Configs.Extensions.FeedbackConfig", "FeedbackConfig")
                        .WithMany()
                        .HasForeignKey("FeedbackConfigId");

                    b.HasOne("DB.Models.Guild", "LinkedGuild")
                        .WithMany()
                        .HasForeignKey("LinkedGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AntiSpamConfig");

                    b.Navigation("FeedbackConfig");

                    b.Navigation("LinkedGuild");
                });

            modelBuilder.Entity("DB.Models.GuildChannel", b =>
                {
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
