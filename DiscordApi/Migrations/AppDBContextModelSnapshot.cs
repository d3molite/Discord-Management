﻿// <auto-generated />
using System;
using DiscordApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DiscordManager.Migrations
{
    [DbContext(typeof(AppDBContext))]
    partial class AppDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("DiscordApi.Models.AntiSpamConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("IgnorePrefixes")
                        .HasColumnType("TEXT");

                    b.Property<int>("LinkLimit")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MessageLimit")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MutedRoleID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ResetTime")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("MutedRoleID");

                    b.ToTable("AntiSpamConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.Bot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsDebug")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

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

                    b.Property<int?>("AntiSpamID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ESportsEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FeedbackConfigID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ImageManipulationEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LastCommitPosted")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Locale")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("ModnotesEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReactionConfigID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedBotID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedGuildId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RelatedLoggerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("AntiSpamID");

                    b.HasIndex("FeedbackConfigID");

                    b.HasIndex("ReactionConfigID");

                    b.HasIndex("RelatedBotID");

                    b.HasIndex("RelatedGuildId");

                    b.HasIndex("RelatedLoggerID");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("DiscordApi.Models.Emoji", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmojiString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("Emojis");
                });

            modelBuilder.Entity("DiscordApi.Models.FeedbackConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("AddReactions")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("FeedbackChannelID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("FeedbackConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("GuildID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

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

                    b.Property<ulong?>("StatusChannelID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("LoggingConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.Message", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("MessageGuildID")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("MessageID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("MessageGuildID");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("DiscordApi.Models.MessageReaction", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EmojiOnly")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MessageReactionConfigID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReactionChance")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ReactionEmojiID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ReactionMessage")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReactionTrigger")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.HasIndex("MessageReactionConfigID");

                    b.HasIndex("ReactionEmojiID");

                    b.ToTable("MessageReaction");
                });

            modelBuilder.Entity("DiscordApi.Models.MessageReactionConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("MessageReactionConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.Modnote", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AuthorId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GuildId")
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Logged")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("AuthorId");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserID");

                    b.ToTable("Modnotes");
                });

            modelBuilder.Entity("DiscordApi.Models.ReactionRoleConfig", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BotConfigID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedEmojiID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedGuildId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedMessageID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RelatedRoleID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("BotConfigID");

                    b.HasIndex("RelatedEmojiID");

                    b.HasIndex("RelatedGuildId");

                    b.HasIndex("RelatedMessageID");

                    b.HasIndex("RelatedRoleID");

                    b.ToTable("ReactionRoleConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("RoleGuildID")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("RoleID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.HasIndex("RoleGuildID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("DiscordApi.Models.SocialMediaConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BotConfigID")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("ChannelId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("LastPosted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserHandle")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BotConfigID");

                    b.ToTable("SocialConfigs");
                });

            modelBuilder.Entity("DiscordApi.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("UserID")
                        .HasColumnType("INTEGER");

                    b.HasKey("ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DiscordApi.Models.VoiceChannelConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BotConfigID")
                        .HasColumnType("INTEGER");

                    b.Property<ulong>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<ulong?>("OnlyAllowedIn")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VoiceGuildId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BotConfigID");

                    b.HasIndex("VoiceGuildId");

                    b.ToTable("VoiceChannelConfigs");
                });

            modelBuilder.Entity("DiscordApi.Models.AntiSpamConfig", b =>
                {
                    b.HasOne("DiscordApi.Models.Role", "MutedRole")
                        .WithMany()
                        .HasForeignKey("MutedRoleID");

                    b.Navigation("MutedRole");
                });

            modelBuilder.Entity("DiscordApi.Models.BotConfig", b =>
                {
                    b.HasOne("DiscordApi.Models.AntiSpamConfig", "AntiSpam")
                        .WithMany()
                        .HasForeignKey("AntiSpamID");

                    b.HasOne("DiscordApi.Models.FeedbackConfig", "FeedbackConfig")
                        .WithMany()
                        .HasForeignKey("FeedbackConfigID");

                    b.HasOne("DiscordApi.Models.MessageReactionConfig", "ReactionConfig")
                        .WithMany()
                        .HasForeignKey("ReactionConfigID");

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

                    b.Navigation("AntiSpam");

                    b.Navigation("FeedbackConfig");

                    b.Navigation("ReactionConfig");

                    b.Navigation("RelatedBot");

                    b.Navigation("RelatedGuild");

                    b.Navigation("RelatedLogger");
                });

            modelBuilder.Entity("DiscordApi.Models.Message", b =>
                {
                    b.HasOne("DiscordApi.Models.Guild", "RelatedGuild")
                        .WithMany()
                        .HasForeignKey("MessageGuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedGuild");
                });

            modelBuilder.Entity("DiscordApi.Models.MessageReaction", b =>
                {
                    b.HasOne("DiscordApi.Models.MessageReactionConfig", null)
                        .WithMany("MessageReactions")
                        .HasForeignKey("MessageReactionConfigID");

                    b.HasOne("DiscordApi.Models.Emoji", "ReactionEmoji")
                        .WithMany()
                        .HasForeignKey("ReactionEmojiID");

                    b.Navigation("ReactionEmoji");
                });

            modelBuilder.Entity("DiscordApi.Models.Modnote", b =>
                {
                    b.HasOne("DiscordApi.Models.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Guild");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DiscordApi.Models.ReactionRoleConfig", b =>
                {
                    b.HasOne("DiscordApi.Models.BotConfig", null)
                        .WithMany("RoleConfigs")
                        .HasForeignKey("BotConfigID");

                    b.HasOne("DiscordApi.Models.Emoji", "RelatedEmoji")
                        .WithMany()
                        .HasForeignKey("RelatedEmojiID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.Guild", "RelatedGuild")
                        .WithMany()
                        .HasForeignKey("RelatedGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.Message", "RelatedMessage")
                        .WithMany()
                        .HasForeignKey("RelatedMessageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DiscordApi.Models.Role", "RelatedRole")
                        .WithMany()
                        .HasForeignKey("RelatedRoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedEmoji");

                    b.Navigation("RelatedGuild");

                    b.Navigation("RelatedMessage");

                    b.Navigation("RelatedRole");
                });

            modelBuilder.Entity("DiscordApi.Models.Role", b =>
                {
                    b.HasOne("DiscordApi.Models.Guild", "RelatedGuild")
                        .WithMany()
                        .HasForeignKey("RoleGuildID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedGuild");
                });

            modelBuilder.Entity("DiscordApi.Models.SocialMediaConfig", b =>
                {
                    b.HasOne("DiscordApi.Models.BotConfig", null)
                        .WithMany("SocialMediaConfigs")
                        .HasForeignKey("BotConfigID");
                });

            modelBuilder.Entity("DiscordApi.Models.VoiceChannelConfig", b =>
                {
                    b.HasOne("DiscordApi.Models.BotConfig", null)
                        .WithMany("VoiceConfig")
                        .HasForeignKey("BotConfigID");

                    b.HasOne("DiscordApi.Models.Guild", "VoiceGuild")
                        .WithMany()
                        .HasForeignKey("VoiceGuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("VoiceGuild");
                });

            modelBuilder.Entity("DiscordApi.Models.BotConfig", b =>
                {
                    b.Navigation("RoleConfigs");

                    b.Navigation("SocialMediaConfigs");

                    b.Navigation("VoiceConfig");
                });

            modelBuilder.Entity("DiscordApi.Models.MessageReactionConfig", b =>
                {
                    b.Navigation("MessageReactions");
                });
#pragma warning restore 612, 618
        }
    }
}
