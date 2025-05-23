﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using edusync.Models;

#nullable disable
namespace edusync.Migrations
{
    [DbContext(typeof(EduSyncDbContext))]
    partial class EduSyncDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("edusync.Models.Assessment", b =>
            {
                b.Property<Guid>("id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("(newid())");

                b.Property<Guid>("course_id")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime?>("created_at")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdate())");

                b.Property<string>("description")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("pass_score")
                    .HasColumnType("int");

                b.Property<string>("questions")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("time_limit")
                    .HasColumnType("int");

                b.Property<string>("title")
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("nvarchar(255)");

                b.Property<DateTime?>("updated_at")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdate())");

                b.HasKey("id")
                    .HasName("PK__Assessme__3213E83FA29318BB");

                b.ToTable("Assessments");
            });

            modelBuilder.Entity("edusync.Models.Course", b =>
            {
                b.Property<Guid>("id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("(newid())");

                b.Property<string>("category")
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<DateTime?>("created_at")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdate())");

                b.Property<string>("description")
                    .HasColumnType("nvarchar(max)");

                b.Property<int>("duration")
                    .HasColumnType("int");

                b.Property<Guid>("teacher_id")
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("thumbnail")
                    .HasMaxLength(500)
                    .HasColumnType("nvarchar(500)");

                b.Property<string>("title")
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar(200)");

                b.Property<DateTime?>("updated_at")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("datetime2")
                    .HasDefaultValueSql("(getdate())");

                b.HasKey("id")
                    .HasName("PK__Courses__3213E83FA61986A5");

                b.ToTable("Courses");
            });

            modelBuilder.Entity("edusync.Models.Result", b =>
            {
                b.Property<Guid>("ResultId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<Guid>("AssessmentId")
                    .HasColumnType("uniqueidentifier");

                b.Property<DateTime>("AttemptDate")
                    .HasColumnType("datetime2");

                b.Property<int>("Score")
                    .HasColumnType("int");

                b.Property<Guid>("UserId")
                    .HasColumnType("uniqueidentifier");

                b.HasKey("ResultId");

                b.ToTable("Results");
            });

            modelBuilder.Entity("edusync.Models.User", b =>
            {
                b.Property<Guid>("UserId")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier")
                    .HasDefaultValueSql("(newid())");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("nvarchar(100)");

                b.Property<string>("PasswordHash")
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnType("nvarchar(256)");

                b.Property<string>("Role")
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("nvarchar(20)");

                b.HasKey("UserId")
                    .HasName("PK__Users__1788CC4CC2586F00");

                b.HasIndex(new[] { "Email" }, "UQ__Users__A9D1053491DA832C")
                    .IsUnique();

                b.ToTable("Users");
            });
        }
    }
}
