﻿// <auto-generated />
using System;
using Appointment.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Appointment.Infra.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Appointment.Domain.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasColumnName("CreatedAt")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date")
                        .HasColumnName("Date");

                    b.Property<int>("DoctorId")
                        .HasColumnType("int")
                        .HasColumnName("DoctorId");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit")
                        .HasColumnName("IsActive");

                    b.Property<int>("PatientId")
                        .HasColumnType("int")
                        .HasColumnName("PatientId");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Status");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("time")
                        .HasColumnName("Time");

                    b.HasKey("Id");

                    b.ToTable("Appointment", (string)null);
                });

            modelBuilder.Entity("HealthMed.Domain.Entities.SeedHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("RunDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SeedId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SeedHistories");
                });
#pragma warning restore 612, 618
        }
    }
}
