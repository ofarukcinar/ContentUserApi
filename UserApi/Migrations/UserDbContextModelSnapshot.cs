﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UserApi.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 11, 21, 7, 3, 48, 710, DateTimeKind.Utc).AddTicks(7040),
                            Name = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 11, 21, 7, 3, 48, 710, DateTimeKind.Utc).AddTicks(7040),
                            Name = "User"
                        });
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2024, 11, 21, 7, 3, 48, 710, DateTimeKind.Utc).AddTicks(7140),
                            Email = "john@example.com",
                            Name = "John Doe",
                            Password = "Test",
                            RoleId = 1
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2024, 11, 21, 7, 3, 48, 710, DateTimeKind.Utc).AddTicks(7140),
                            Email = "jane@example.com",
                            Name = "Jane Doe",
                            Password = "Test",
                            RoleId = 2
                        });
                });

            modelBuilder.Entity("UserDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserDetails");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St",
                            CreatedAt = new DateTime(2024, 11, 21, 7, 3, 48, 710, DateTimeKind.Utc).AddTicks(7160),
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Elm St",
                            CreatedAt = new DateTime(2024, 11, 21, 7, 3, 48, 710, DateTimeKind.Utc).AddTicks(7160),
                            UserId = 2
                        });
                });

            modelBuilder.Entity("User", b =>
                {
                    b.HasOne("Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("UserDetails", b =>
                {
                    b.HasOne("User", "User")
                        .WithOne("UserDetails")
                        .HasForeignKey("UserDetails", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Navigation("UserDetails")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
