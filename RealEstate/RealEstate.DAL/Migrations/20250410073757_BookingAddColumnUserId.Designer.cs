﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RealEstate.DAL.Enums;
using RealEstate.DAL.Repositories;

#nullable disable

namespace RealEstate.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250410073757_BookingAddColumnUserId")]
    partial class BookingAddColumnUserId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "EstateAction", new[] { "none", "rent", "sell" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "EstateStatus", new[] { "for_rent", "for_sale", "none" });
            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "EstateType", new[] { "apartment", "house", "none", "office" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RealEstate.DAL.Entities.BookingEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<EstateAction>("EstateAction")
                        .HasColumnType("\"EstateAction\"");

                    b.Property<string>("Proposal")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("RealEstateId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RealEstateId");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.HistoryEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CompletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<EstateAction>("EstateAction")
                        .HasColumnType("\"EstateAction\"");

                    b.Property<Guid?>("RealEstateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("RealEstateId");

                    b.HasIndex("UserId");

                    b.ToTable("Histories");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.RealEstateEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<EstateStatus>("EstateStatus")
                        .HasColumnType("\"EstateStatus\"");

                    b.Property<EstateType>("EstateType")
                        .HasColumnType("\"EstateType\"");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("RealEstates");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.ReviewEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<short>("Rating")
                        .HasColumnType("smallint");

                    b.Property<Guid>("RecipientId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("RecipientId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.BookingEntity", b =>
                {
                    b.HasOne("RealEstate.DAL.Entities.RealEstateEntity", "RealEstate")
                        .WithMany("Bookings")
                        .HasForeignKey("RealEstateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RealEstate.DAL.Entities.UserEntity", "User")
                        .WithMany("Bookings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RealEstate");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.HistoryEntity", b =>
                {
                    b.HasOne("RealEstate.DAL.Entities.RealEstateEntity", "RealEstate")
                        .WithMany("Histories")
                        .HasForeignKey("RealEstateId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("RealEstate.DAL.Entities.UserEntity", "User")
                        .WithMany("Histories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RealEstate");

                    b.Navigation("User");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.RealEstateEntity", b =>
                {
                    b.HasOne("RealEstate.DAL.Entities.UserEntity", "Owner")
                        .WithMany("RealEstates")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.ReviewEntity", b =>
                {
                    b.HasOne("RealEstate.DAL.Entities.UserEntity", "Author")
                        .WithMany("AuthoredReviews")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("RealEstate.DAL.Entities.UserEntity", "Recipient")
                        .WithMany("ReceivedReviews")
                        .HasForeignKey("RecipientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.RealEstateEntity", b =>
                {
                    b.Navigation("Bookings");

                    b.Navigation("Histories");
                });

            modelBuilder.Entity("RealEstate.DAL.Entities.UserEntity", b =>
                {
                    b.Navigation("AuthoredReviews");

                    b.Navigation("Bookings");

                    b.Navigation("Histories");

                    b.Navigation("RealEstates");

                    b.Navigation("ReceivedReviews");
                });
#pragma warning restore 612, 618
        }
    }
}
