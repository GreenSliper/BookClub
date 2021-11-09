﻿// <auto-generated />
using System;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DAL.DTO.Ban", b =>
                {
                    b.Property<int>("ClubID")
                        .HasColumnType("integer");

                    b.Property<string>("BannedUserID")
                        .HasColumnType("text");

                    b.Property<string>("BannedByID")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.HasKey("ClubID", "BannedUserID");

                    b.HasIndex("BannedByID");

                    b.HasIndex("BannedUserID");

                    b.ToTable("Ban");
                });

            modelBuilder.Entity("DAL.DTO.Book", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AddedByUserId")
                        .HasColumnType("text");

                    b.Property<string>("Author")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("AddedByUserId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("DAL.DTO.Club", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("AvatarImageID")
                        .HasColumnType("integer");

                    b.Property<string>("CreatorId")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<bool>("IsPublic")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.HasIndex("AvatarImageID");

                    b.HasIndex("CreatorId");

                    b.ToTable("Clubs");
                });

            modelBuilder.Entity("DAL.DTO.ClubBook", b =>
                {
                    b.Property<int>("ClubID")
                        .HasColumnType("integer");

                    b.Property<int>("BookID")
                        .HasColumnType("integer");

                    b.Property<string>("AddedByUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("AddedTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("ReadUntil")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ClubID", "BookID");

                    b.HasIndex("AddedByUserId");

                    b.HasIndex("BookID");

                    b.ToTable("ClubBook");
                });

            modelBuilder.Entity("DAL.DTO.ClubDiscussion", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ClubID")
                        .HasColumnType("integer");

                    b.Property<string>("CreatorId")
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("ID");

                    b.HasIndex("ClubID");

                    b.HasIndex("CreatorId");

                    b.ToTable("ClubDiscussion");
                });

            modelBuilder.Entity("DAL.DTO.ClubDiscussionBook", b =>
                {
                    b.Property<int>("BookID")
                        .HasColumnType("integer");

                    b.Property<int>("DiscussionID")
                        .HasColumnType("integer");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.HasKey("BookID", "DiscussionID");

                    b.HasIndex("DiscussionID");

                    b.ToTable("ClubDiscussionBook");
                });

            modelBuilder.Entity("DAL.DTO.ClubInvite", b =>
                {
                    b.Property<int>("ClubID")
                        .HasColumnType("integer");

                    b.Property<string>("ReceiverID")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("GivenPermissions")
                        .HasColumnType("integer");

                    b.Property<string>("InviterID")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.HasKey("ClubID", "ReceiverID");

                    b.HasIndex("InviterID");

                    b.HasIndex("ReceiverID");

                    b.ToTable("ClubInvite");
                });

            modelBuilder.Entity("DAL.DTO.ClubMember", b =>
                {
                    b.Property<int>("ClubID")
                        .HasColumnType("integer");

                    b.Property<string>("UserID")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastVisitTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PermissionLevel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClubID", "UserID");

                    b.HasIndex("UserID");

                    b.ToTable("ClubsMembers");
                });

            modelBuilder.Entity("DAL.DTO.DBImage", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<byte[]>("ImageData")
                        .HasColumnType("bytea");

                    b.HasKey("ID");

                    b.ToTable("DBImage");
                });

            modelBuilder.Entity("DAL.DTO.ReadBook", b =>
                {
                    b.Property<string>("ReaderID")
                        .HasColumnType("text");

                    b.Property<int>("BookID")
                        .HasColumnType("integer");

                    b.Property<int?>("Rating")
                        .HasColumnType("integer");

                    b.Property<int?>("RememberQuality")
                        .HasColumnType("integer");

                    b.HasKey("ReaderID", "BookID");

                    b.HasIndex("BookID");

                    b.ToTable("ReadBook");
                });

            modelBuilder.Entity("DAL.Data.ReaderUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DAL.DTO.Ban", b =>
                {
                    b.HasOne("DAL.Data.ReaderUser", "BannedBy")
                        .WithMany("GivenBans")
                        .HasForeignKey("BannedByID");

                    b.HasOne("DAL.Data.ReaderUser", "BannedUser")
                        .WithMany("ReceivedBans")
                        .HasForeignKey("BannedUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.DTO.Club", "Club")
                        .WithMany("BanList")
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BannedBy");

                    b.Navigation("BannedUser");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("DAL.DTO.Book", b =>
                {
                    b.HasOne("DAL.Data.ReaderUser", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedByUserId");

                    b.Navigation("AddedByUser");
                });

            modelBuilder.Entity("DAL.DTO.Club", b =>
                {
                    b.HasOne("DAL.DTO.DBImage", "AvatarImage")
                        .WithMany()
                        .HasForeignKey("AvatarImageID");

                    b.HasOne("DAL.Data.ReaderUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("AvatarImage");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("DAL.DTO.ClubBook", b =>
                {
                    b.HasOne("DAL.Data.ReaderUser", "AddedByUser")
                        .WithMany()
                        .HasForeignKey("AddedByUserId");

                    b.HasOne("DAL.DTO.Book", "Book")
                        .WithMany("Clubs")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.DTO.Club", "Club")
                        .WithMany("Books")
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AddedByUser");

                    b.Navigation("Book");

                    b.Navigation("Club");
                });

            modelBuilder.Entity("DAL.DTO.ClubDiscussion", b =>
                {
                    b.HasOne("DAL.DTO.Club", "Club")
                        .WithMany("Discussions")
                        .HasForeignKey("ClubID");

                    b.HasOne("DAL.Data.ReaderUser", "Creator")
                        .WithMany("CreatedDiscussions")
                        .HasForeignKey("CreatorId");

                    b.Navigation("Club");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("DAL.DTO.ClubDiscussionBook", b =>
                {
                    b.HasOne("DAL.DTO.Book", "Book")
                        .WithMany("IncludedInDiscussions")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.DTO.ClubDiscussion", "Discussion")
                        .WithMany("Books")
                        .HasForeignKey("DiscussionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Discussion");
                });

            modelBuilder.Entity("DAL.DTO.ClubInvite", b =>
                {
                    b.HasOne("DAL.DTO.Club", "Club")
                        .WithMany("Invites")
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Data.ReaderUser", "Inviter")
                        .WithMany("SentInvites")
                        .HasForeignKey("InviterID");

                    b.HasOne("DAL.Data.ReaderUser", "Receiver")
                        .WithMany("ReceivedInvites")
                        .HasForeignKey("ReceiverID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("Inviter");

                    b.Navigation("Receiver");
                });

            modelBuilder.Entity("DAL.DTO.ClubMember", b =>
                {
                    b.HasOne("DAL.DTO.Club", "Club")
                        .WithMany("Members")
                        .HasForeignKey("ClubID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Data.ReaderUser", "User")
                        .WithMany("Memberships")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Club");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.DTO.ReadBook", b =>
                {
                    b.HasOne("DAL.DTO.Book", "Book")
                        .WithMany("ReadBy")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Data.ReaderUser", "Reader")
                        .WithMany("ReadBooks")
                        .HasForeignKey("ReaderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");

                    b.Navigation("Reader");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DAL.Data.ReaderUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DAL.Data.ReaderUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Data.ReaderUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("DAL.Data.ReaderUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.DTO.Book", b =>
                {
                    b.Navigation("Clubs");

                    b.Navigation("IncludedInDiscussions");

                    b.Navigation("ReadBy");
                });

            modelBuilder.Entity("DAL.DTO.Club", b =>
                {
                    b.Navigation("BanList");

                    b.Navigation("Books");

                    b.Navigation("Discussions");

                    b.Navigation("Invites");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("DAL.DTO.ClubDiscussion", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("DAL.Data.ReaderUser", b =>
                {
                    b.Navigation("CreatedDiscussions");

                    b.Navigation("GivenBans");

                    b.Navigation("Memberships");

                    b.Navigation("ReadBooks");

                    b.Navigation("ReceivedBans");

                    b.Navigation("ReceivedInvites");

                    b.Navigation("SentInvites");
                });
#pragma warning restore 612, 618
        }
    }
}
