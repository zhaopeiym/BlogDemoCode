using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplication1;

namespace WebApplication1.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20170804230202_db")]
    partial class db
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("WebApplication1.ChatInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("IP")
                        .HasMaxLength(20);

                    b.Property<bool>("Sex");

                    b.Property<string>("UserName")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("ChatInfo");
                });

            modelBuilder.Entity("WebApplication1.LogInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("IP")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("LogInfos");
                });

            modelBuilder.Entity("WebApplication1.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(80);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("IP")
                        .HasMaxLength(20);

                    b.Property<string>("UserName")
                        .HasMaxLength(20);

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });
        }
    }
}
