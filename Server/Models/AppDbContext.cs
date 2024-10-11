using Messenger.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Server.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Conv> Convs { get; set; }
        public DbSet<Friend> Friends { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Conv>()
                .HasOne(c => c.Sender)
                .WithMany(u => u.SentConversations)
                .HasForeignKey(c => c.IdEmet)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Conv>()
                .HasOne(c => c.Receiver)
                .WithMany(u => u.ReceivedConversations)
                .HasForeignKey(c => c.IdRec)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Sender)
                .WithMany(u => u.Friends)
                .HasForeignKey(f => f.IdEmet)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Friend>()
                .HasOne(f => f.Receiver)
                .WithMany()
                .HasForeignKey(f => f.IdRec)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
