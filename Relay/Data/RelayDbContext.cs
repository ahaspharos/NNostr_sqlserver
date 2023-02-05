using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NBitcoin;
using Newtonsoft.Json;
using NNostr.Client;
using Relay.Models;

namespace Relay.Data
{
    public class Balance
    {
        [Key]
        public string PublicKey { get; set; }
        public long CurrentBalance { get; set; }
        public List<BalanceTransaction> BalanceTransactions { get; set; }
    }

    public class BalanceTransaction
    {
        public string Id { get; set; }
        public string BalanceId { get; set; }
        public string? BalanceTopupId { get; set; }
        public string? EventId { get; set; }
        public long Value { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public NostrEvent? Event { get; set; }
        public BalanceTopup? Topup { get; set; }
        public Balance Balance { get; set; }
    }


    public class BalanceTopup
    {

        public string Id { get; set; }
        public string BalanceId { get; set; }
        public TopupStatus Status { get; set; }

        public List<BalanceTransaction> BalanceTransactions { get; set; }

        public Balance Balance { get; set; }
        public enum TopupStatus
        {
            Pending,
            Complete,
            Expired,

        }
    }


    public class RelayDbContext : DbContext
    {
        public DbSet<Balance> Balances { get; set; }
        public DbSet<BalanceTopup> BalanceTopups { get; set; }
        public DbSet<BalanceTransaction> BalanceTransactions { get; set; }
        public DbSet<NostrEventTag> EventTags { get; set; }
        public DbSet<RelayNostrEvent> Events { get; set; }
        public const string DatabaseConnectionStringName = "RelayDatabase";


        public DbSet<Whitelist> Whitelist { get; set; }

        public RelayDbContext(DbContextOptions<RelayDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<NostrEventTag>()
                .HasOne(p => p.Event)
                .WithMany(b => b.Tags);

            

            modelBuilder.Entity<NostrEventTag>()
            .Property(p => p.Data)
            .HasConversion(
                v => JsonConvert.SerializeObject(v),
                v => JsonConvert.DeserializeObject<List<string>>(v));

            var valueComparer = new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual<string>(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

            modelBuilder
            .Entity<NostrEventTag>()
            .Property(e => e.Data)
            .Metadata
            .SetValueComparer(valueComparer);

        }
    }
}