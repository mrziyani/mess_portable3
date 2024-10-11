﻿// <auto-generated />
using System;
using Messenger.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Messenger.Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241011182710_a")]
    partial class a
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.33")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Messenger.Shared.Models.Conv", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("Etat")
                        .HasColumnType("int");

                    b.Property<string>("IdEmet")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdRec")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("IdEmet");

                    b.HasIndex("IdRec");

                    b.ToTable("Convs");
                });

            modelBuilder.Entity("Messenger.Shared.Models.Friend", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("Etat")
                        .HasColumnType("bit");

                    b.Property<string>("IdEmet")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("IdRec")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("IdEmet");

                    b.HasIndex("IdRec");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("Messenger.Shared.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("DateNais")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Mdp")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Prenom")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Sexe")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Messenger.Shared.Models.Conv", b =>
                {
                    b.HasOne("Messenger.Shared.Models.User", "Sender")
                        .WithMany("SentConversations")
                        .HasForeignKey("IdEmet")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Messenger.Shared.Models.User", "Receiver")
                        .WithMany("ReceivedConversations")
                        .HasForeignKey("IdRec")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Messenger.Shared.Models.Friend", b =>
                {
                    b.HasOne("Messenger.Shared.Models.User", "Sender")
                        .WithMany("Friends")
                        .HasForeignKey("IdEmet")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Messenger.Shared.Models.User", "Receiver")
                        .WithMany()
                        .HasForeignKey("IdRec")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("Messenger.Shared.Models.User", b =>
                {
                    b.Navigation("Friends");

                    b.Navigation("ReceivedConversations");

                    b.Navigation("SentConversations");
                });
#pragma warning restore 612, 618
        }
    }
}