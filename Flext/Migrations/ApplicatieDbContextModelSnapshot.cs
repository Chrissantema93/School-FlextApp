﻿// <auto-generated />
using System;
using Flext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Flext.Migrations
{
    [DbContext(typeof(ApplicatieDbContext))]
    partial class ApplicatieDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.0-rtm-30799")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Flext.Models.ImageDescription", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FileName");

                    b.Property<string>("Format");

                    b.Property<int>("ImageHeihgt");

                    b.Property<int>("ImageWidth");

                    b.Property<string>("RequestId");

                    b.Property<string>("Tags");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("ID");

                    b.ToTable("Detecties");
                });
#pragma warning restore 612, 618
        }
    }
}
