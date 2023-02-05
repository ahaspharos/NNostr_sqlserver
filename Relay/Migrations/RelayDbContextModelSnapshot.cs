﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Relay.Data;

#nullable disable

namespace Relay.Migrations
{
    [DbContext(typeof(RelayDbContext))]
    partial class RelayDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("NNostr.Client.NostrEvent", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Signature")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Events");

                    b.HasDiscriminator<string>("Discriminator").HasValue("NostrEvent");
                });

            modelBuilder.Entity("NNostr.Client.NostrEventTag", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EventId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TagIdentifier")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventTags");
                });

            modelBuilder.Entity("Relay.Data.Balance", b =>
                {
                    b.Property<string>("PublicKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long>("CurrentBalance")
                        .HasColumnType("bigint");

                    b.HasKey("PublicKey");

                    b.ToTable("Balances");
                });

            modelBuilder.Entity("Relay.Data.BalanceTopup", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BalanceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId");

                    b.ToTable("BalanceTopups");
                });

            modelBuilder.Entity("Relay.Data.BalanceTransaction", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BalanceId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("BalanceTopupId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EventId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("BalanceId");

                    b.HasIndex("BalanceTopupId");

                    b.HasIndex("EventId");

                    b.ToTable("BalanceTransactions");
                });

            modelBuilder.Entity("Relay.Models.Whitelist", b =>
                {
                    b.Property<string>("PubKey")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PubKey");

                    b.ToTable("Whitelist");
                });

            modelBuilder.Entity("Relay.RelayNostrEvent", b =>
                {
                    b.HasBaseType("NNostr.Client.NostrEvent");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.ToTable("Events");

                    b.HasDiscriminator().HasValue("RelayNostrEvent");
                });

            modelBuilder.Entity("NNostr.Client.NostrEventTag", b =>
                {
                    b.HasOne("NNostr.Client.NostrEvent", "Event")
                        .WithMany("Tags")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Relay.Data.BalanceTopup", b =>
                {
                    b.HasOne("Relay.Data.Balance", "Balance")
                        .WithMany()
                        .HasForeignKey("BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Balance");
                });

            modelBuilder.Entity("Relay.Data.BalanceTransaction", b =>
                {
                    b.HasOne("Relay.Data.Balance", "Balance")
                        .WithMany("BalanceTransactions")
                        .HasForeignKey("BalanceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Relay.Data.BalanceTopup", "Topup")
                        .WithMany("BalanceTransactions")
                        .HasForeignKey("BalanceTopupId");

                    b.HasOne("NNostr.Client.NostrEvent", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Balance");

                    b.Navigation("Event");

                    b.Navigation("Topup");
                });

            modelBuilder.Entity("NNostr.Client.NostrEvent", b =>
                {
                    b.Navigation("Tags");
                });

            modelBuilder.Entity("Relay.Data.Balance", b =>
                {
                    b.Navigation("BalanceTransactions");
                });

            modelBuilder.Entity("Relay.Data.BalanceTopup", b =>
                {
                    b.Navigation("BalanceTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
