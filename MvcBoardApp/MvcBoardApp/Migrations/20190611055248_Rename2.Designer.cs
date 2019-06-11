﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MvcBoardApp.Models;

namespace MvcBoardApp.Migrations
{
    [DbContext(typeof(MvcBoardAppContext))]
    [Migration("20190611055248_Rename2")]
    partial class Rename2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MvcBoardApp.Models.Board", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CommentCount");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<string>("Subject")
                        .IsRequired();

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<DateTime?>("WriteDate");

                    b.HasKey("ID");

                    b.ToTable("Board");
                });

            modelBuilder.Entity("MvcBoardApp.Models.Comment", b =>
                {
                    b.Property<int?>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BoardID");

                    b.Property<string>("CommentContent")
                        .IsRequired();

                    b.Property<string>("CommentUserName")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("BoardID");

                    b.ToTable("Comment");
                });

            modelBuilder.Entity("MvcBoardApp.Models.Comment", b =>
                {
                    b.HasOne("MvcBoardApp.Models.Board", "Board")
                        .WithMany()
                        .HasForeignKey("BoardID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
